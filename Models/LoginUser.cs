// We can disable our warnings safely because we know the framework will assign non-null values 
// when it constructs this class for us.
#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[NotMapped]
//line 6 "[NotMapped]" prevents add a extra login user table to our db
public class LoginUser
{

[Required(ErrorMessage = "is required")] 
[EmailAddress]
[Display(Name = "Email")]
public string LoginEmail {get; set;}
// line 13 & 18 uses display so your form reads email/password rather than login email  login password
[Required(ErrorMessage = "is required")] 
[DataType(DataType.Password)]
[Display(Name = "Password")]
 public string LoginPassword {get; set;}
}