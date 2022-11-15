using DSM.BAL.Abstraction;
using DSM.Entities.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSM.BAL.Abstraction;
using DSM.Model;
using DSM.Model.DBModel;
using Microsoft.EntityFrameworkCore;

namespace DSM.BAL.Service
{
    public class UserService : IUserInterface
    {
        private readonly DemoProjectContext _dbContext;
        private readonly ILogger<UserService> _logger;
        private readonly IEncrypt encryptService;
        public UserService(DemoProjectContext dbContext, ILogger<UserService> logger, IEncrypt encrypt)
        {
            encryptService = encrypt;
            _dbContext = dbContext;
            _logger = logger;
        }
        public List<UserDisplay> GetUser()
        {
            List<SignUp> signUps = _dbContext.SignUps.ToList();
            List<UserType> userTypes = _dbContext.UserTypes.ToList();
            var result = (from su in signUps
                          join ut in userTypes on su.TypeOfUser equals ut.UserTypeId
                          orderby su.UserId
                          select new
                          {
                              su.UserId,
                              su.FirstName,
                              su.LastName,
                              su.Email,
                              su.PhoneNum,
                              ut.UserType1,
                              su.CreatedDate,
                              su.UpdatedDate,
                              su.Active
                          }).ToList();
            List<UserDisplay> user=new List<UserDisplay>();
            //List<UserDisplay> users = 
            // result.Dumb();
            return user;
        }
        public string SignUp(SignUp signUp)
        {  
            string encryptPassword=encryptService.EncodePasswordToBase64(signUp.Password);
            //string encryptConPassword = encryptService.EncodePasswordToBase64(signUp.ConfirmPassword);
            signUp.Password=encryptPassword;
            signUp.CreatedDate=DateTime.Now;
            signUp.UpdatedDate=null;
            signUp.Active = true;
            //if (obj.Password == obj.ConfirmPassword)
            //{
                if (_dbContext.SignUps.Count() == 0)
                {
                    _dbContext.SignUps.Add(signUp);
                    _dbContext.SaveChanges();
                    return "Sign up successfully";
                }
            SignUp signUp2 = _dbContext.SignUps.Where(x => x.Email == signUp.Email).FirstOrDefault();
            if (signUp2 == null)
            {
                _dbContext.SignUps.Add(signUp);
                _dbContext.SaveChanges();
                return "Sign up successfully";
            }
            else
            {
                return "Your Email already registered. Please Log in";
            }
            //}
            //else
            //{
            //    return "Password and Confirm password not matched";
            //}
        }
        public bool LogIn(LogIn login)
        {
            string encryptPassword = encryptService.EncodePasswordToBase64(login.Password);
            SignUp signUp = _dbContext.SignUps.Where(x => x.Email == login.Email && x.Password==encryptPassword).FirstOrDefault();
            if (signUp!=null)
            {
                        return true;   
            }
                return false;
        }
        public string ForgetPassword(ChangePassword changePassword)
        {
            string encryptPassword = encryptService.EncodePasswordToBase64(changePassword.Password);
            string encryptConPassword = encryptService.EncodePasswordToBase64(changePassword.ConfirmPassword);
            if (encryptPassword == encryptConPassword)
            {
                SignUp signUp= _dbContext.SignUps.Where(x => x.Email == changePassword.Email).FirstOrDefault();
               signUp.Password= encryptPassword;
                signUp.UpdatedDate = DateTime.Now;

                if (signUp != null)
                { 
                    _dbContext.Entry(signUp).State=EntityState.Modified;
                    _dbContext.SaveChanges();
                    return "Password updated successfully";
                }
                return "Email doesn't registered. Please Sign up";
            }
            else
            {
                return "Password and Confirm password not matched";
            }
        }
    }
}