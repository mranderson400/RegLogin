using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RegLogin.Models;

namespace RegLogin.Controllers;

public class UserController : Controller
{
    private MyContext db;         
    // Here we can "inject" our context service into the constructor 
    public UserController(MyContext context)    
    {        
     
        // When our PostController is instantiated, it will fill in db with context
        // Remember that when context is initialized, it brings in everything we need from DbContext
        // which comes from Entity Framework Core
        db = context;    
    }       


    
    [HttpGet("")]
public IActionResult Index()
    {
        return View("Index");
    }

    [HttpPost("/register")]
    public IActionResult Register(User newUser)
    {
        if (!ModelState.IsValid)
        {
            return View("Index");
        }
        PasswordHasher<User> hasher = new PasswordHasher<User>();
        newUser.Password = hasher.HashPassword(newUser, newUser.Password);
        db.Users.Add(newUser);
        db.SaveChanges();

    HttpContext.Session.SetInt32("UUID", newUser.UserId);

        return RedirectToAction("Welcome");
    }
        [SessionCheck]
        [HttpGet("/welcome")]
    public IActionResult Welcome()
    {

        return  View("Welcome");
    }

    [HttpPost("login")]
    public IActionResult Login(LoginUser userSubmission)
{    
    if(!ModelState.IsValid)
    {
        return View("Index");
    }
    RedirectToAction("Welcome");
        
        // If initial ModelState is valid, query for a user with the provided email        
        User? userInDb = db.Users.FirstOrDefault(u => u.Email == userSubmission.LoginEmail);        
        PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();                    
        // If no user exists with the provided email        
        if(userInDb == null)        
        {            
            // Add an error to ModelState and return to View!            
            ModelState.AddModelError("LoginEmail", "Invalid Email/Password");            
            return View("Index");        
        }   
        // Otherwise, we have a user, now we need to check their password                 
        // Initialize hasher object        
        // Verify provided password against hash stored in db        
        var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.LoginPassword);                                    // Result can be compared to 0 for failure        
        if(result == 0)        
        {            
            ModelState.AddModelError("LoginEmail", "Invaild Email/Password");
            // Handle failure (this should be similar to how "existing email" is handled)        
        } 
        // Handle success (this should route to an internal page)  
        HttpContext.Session.SetInt32("UUID", userInDb.UserId);

        return RedirectToAction("Welcome"); 
    
}
     [HttpPost("/logout")]
     public IActionResult Logout()
     {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
     }

public class SessionCheckAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Find the session, but remember it may be null so we need int?
        int? userId = context.HttpContext.Session.GetInt32("UUID");
        // Check to see if we got back null
        if(userId == null)
        {
            // Redirect to the Index page if there was nothing in session
            // "Home" here is referring to "HomeController", you can use any controller that is appropriate here
            context.Result = new RedirectToActionResult("Index", "Home", null);
        }
    }
  } 
}