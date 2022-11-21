using DSM.BAL.Abstraction;
using DSM.BAL.Service;
using DSM.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using DSM.Model;
using DSM.Model.DBModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DSM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly ILogger<UserService> _logger;
        private readonly IUserInterface _userService;
        private readonly DemoProjectContext dbContext;
        

        public UserController(DemoProjectContext dbcontext, ILogger<UserService> logger, IUserInterface userService)
        {
            dbContext = dbcontext;
            _logger = logger;
            _userService =userService;
        }

        public UserController(IUserInterface userService)
        {
            _userService = userService;
        }

        // GET: api/<UserController>
        [HttpGet]
        public JsonResult UserDetails()
        {
            try
            {
                return new JsonResult(_userService.GetUser().ToList()); 
            }
            catch(Exception ex)
            {
                return new JsonResult(ex);

            }
        }

        [HttpPost]
        [Route("SignUp")]

        public JsonResult SignUp(SignUp signUp)
        {
            try
            {
                return new JsonResult(_userService.SignUp(signUp));
                
            }
            catch(Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
        [HttpPost]
        [Route("LogIn")]

        public JsonResult LogIn(LogIn logIn)
        {
            try
            {
                bool result = _userService.LogIn(logIn);

                if (result == true)
                {
                    return new JsonResult("Log in successfully");
                }
                else
                {
                    return new JsonResult("Mail and Password not matched");
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
        // PUT api/<UserController>/5
        [HttpPut("{Mail}")]
        public JsonResult ForgetPassword(string Mail,ChangePassword changePassword)
        {
            try
            {
                changePassword.Email = Mail;
                return new JsonResult(_userService.ForgetPassword(changePassword));

            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
    }
}
