#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegLogin.Models;
public class User
{    
    [Key]    

    public int UserId { get; set; }
    
    [Required(ErrorMessage = "is required")] 
    [Display(Name ="First Name")]
    public string FirstName { get; set; }
    
    [Required(ErrorMessage = "is required")] 
        
    public string LastName { get; set; }     
    
    [Required(ErrorMessage = "is required")] 
    [EmailAddress]
    [UniqueEmail]
    // line 23 is your custom validation for email
    //line 22 "[EmailAddress]" validation/ makes sure the user uses @ 
    public string Email { get; set; }    
    
    [Required(ErrorMessage = "is required")] 
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    [DataType(DataType.Password)]
    // LinkedList 25 keeps password characters hidden example: **  
    public string Password { get; set; } 
    
    // line 32  is used for when you dont want an input being displayed in your create form 
    [NotMapped]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "doesn't match password, FIX IT OR ELSE! ")]
    public string PasswordConfirm { get; set; }   
    
    public DateTime CreatedAt {get;set;} = DateTime.Now;   
    public DateTime UpdatedAt {get;set;} = DateTime.Now;
}




//custom validation below

public class UniqueEmailAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
    	// Though we have Required as a validation, sometimes we make it here anyways
    	// In which case we must first verify the value is not null before we proceed
        if(value == null)
        {
    	    // If it was, return the required error
            return new ValidationResult("Email is required!");
        }
    
    	// This will connect us to our database since we are not in our Controller
        MyContext _context = (MyContext)validationContext.GetService(typeof(MyContext));
        // Check to see if there are any records of this email in our database
    	if(_context.Users.Any(e => e.Email == value.ToString()))
        // line 60 tells you if there is already a user with your desire email in the table, thus choose another
        {
    	    // If yes, throw an error
            return new ValidationResult("Email must be unique!");
        } else {
    	    // If no, proceed
            return ValidationResult.Success;
        }
    }
}
