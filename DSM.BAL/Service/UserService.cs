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
        public List<SignUp> GetUser()
        {
            var users = _dbContext.SignUps.ToList();
            return users;
        }
        public string SignUp(SignUp signUp)
        {  
            string encryptPassword=encryptService.EncodePasswordToBase64(signUp.Password);
            //string encryptConPassword = encryptService.EncodePasswordToBase64(signUp.ConfirmPassword);

            var obj = new SignUp
            {
                FirstName = signUp.FirstName,
                LastName = signUp.LastName,
                Email = signUp.Email,
                PhoneNum = signUp.PhoneNum,
                Password = encryptPassword,
                //ConfirmPassword = encryptConPassword,
                    TypeOfUser = signUp.TypeOfUser,
                    CreatedDate=DateTime.Now,
                    UpdatedDate=null,
                    Active=true,

                };
            //if (obj.Password == obj.ConfirmPassword)
            //{
                if (_dbContext.SignUps.Count() == 0)
                {
                    _dbContext.SignUps.Add(obj);
                    _dbContext.SaveChanges();
                    return "Sign up successfully";
                }
                foreach (var item in _dbContext.SignUps)
                {
                    if (item.Email != signUp.Email)
                    {
                        _dbContext.SignUps.Add(obj);
                        _dbContext.SaveChanges();
                        return "Sign up successfully";
                    }
                }
            //}
            //else
            //{
            //    return "Password and Confirm password not matched";
            //}
            return "Your Email already registered. Please Log in";
        }
        public bool LogIn(LogIn login)
        {
            var obj = new LogIn
            {
                Email = login.Email,
                Password = login.Password,
            };
            foreach (var item in _dbContext.SignUps)
            {
                string decryptedPassword = encryptService.DecodeFrom64(item.Password);
                if (item.Email == obj.Email && decryptedPassword==obj.Password)
                {
                    return true;   
                }
            }
            return false;
        }
        public string ForgetPassword(ChangePassword changePassword)
        {
            string encryptPassword = encryptService.EncodePasswordToBase64(changePassword.Password);
            string encryptConPassword = encryptService.EncodePasswordToBase64(changePassword.ConfirmPassword);

            var obj = new ChangePassword
            {
                Email = changePassword.Email,
                Password = encryptPassword,
                ConfirmPassword = encryptConPassword,
            };
            if (obj.Password == obj.ConfirmPassword)
            {
                SignUp signUp= _dbContext.SignUps.Where(x => x.Email == obj.Email).FirstOrDefault();
                var obj2 = new SignUp()
                {
                    FirstName = signUp.FirstName,
                    LastName = signUp.LastName,
                    Email = signUp.Email,
                    PhoneNum = signUp.PhoneNum,
                    Password = obj.Password,
                    TypeOfUser = signUp.TypeOfUser,
                    CreatedDate = signUp.CreatedDate,
                    UpdatedDate = DateTime.Now,
                    Active = true,

                };
                if (obj2 != null)
                { 
                    _dbContext.SignUps.Update(obj2);
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