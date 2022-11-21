using DSM.BAL.Abstraction;
using DSM.BAL.Service;
using DSM.Entities.Models;
using DSM.Model.DBModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSM.APITests
{
    public class UserService2 : IUserInterface
    {
        private readonly List<SignUp> signUp;
        private readonly List<UserDisplay> userDisplay;

        public UserService2()
        {
            signUp = new List<SignUp>()
            {
                new SignUp() { UserId=1, FirstName="Gokul", LastName="B V", Email="bvgokulgok@gmail.com", PhoneNum=8825547037, Password="1234", TypeOfUser=1, CreatedDate=DateTime.Now, UpdatedDate=null, Active=true},
                new SignUp() { UserId=2, FirstName="Diptesh", LastName="Patra", Email="diptesh@gmail.com", PhoneNum=8825547037, Password="1234", TypeOfUser=1, CreatedDate=DateTime.Now, UpdatedDate=null, Active=true},
                new SignUp() { UserId=3, FirstName="Kazeem", LastName="Uddin", Email="kazeem@gmail.com", PhoneNum=8825547037, Password="1234", TypeOfUser=1, CreatedDate=DateTime.Now, UpdatedDate=null, Active=true},
            };
            userDisplay = new List<UserDisplay>()
            {
                new UserDisplay() { UserId=1, FirstName="Gokul", LastName="B V", Email="bvgokulgok@gmail.com", PhoneNum=8825547037,  TypeOfUser="Team Manager", CreatedDate=DateTime.Now, UpdatedDate=null, Active=true},
                new UserDisplay() { UserId=2, FirstName="Diptesh", LastName="Patra", Email="diptesh@gmail.com", PhoneNum=8825547037,  TypeOfUser="Team Manager", CreatedDate=DateTime.Now, UpdatedDate=null, Active=true},
                new UserDisplay() { UserId=3, FirstName="Kazeem", LastName="Uddin", Email="kazeem@gmail.com", PhoneNum=8825547037,  TypeOfUser="Team Manager", CreatedDate=DateTime.Now, UpdatedDate=null, Active=true},
            };
        }

        public List<UserDisplay> GetUser()
        {
            return userDisplay;
        }

        public string SignUp(SignUp newsignUp)
        {
            SignUp signUp2 = signUp.Where(x => x.Email == newsignUp.Email).FirstOrDefault();
            if (signUp2 == null)
            {
                newsignUp.UserId = 4;
                signUp.Add(newsignUp);
                return "Sign up successfully";
            }
            else
            {
                return "Your Email already registered. Please Log in";
            }
        }

        public bool LogIn(LogIn login)
        {
            SignUp signUp2 = signUp.Where(x => x.Email == login.Email && x.Password == login.Password).FirstOrDefault();
            if (signUp2 != null)
            {
                return true;
            }
            return false;
        }

        public string ForgetPassword(ChangePassword changePassword)
        {
            if (changePassword.Password == changePassword.ConfirmPassword)
            {
                int index = signUp.FindIndex(item => item.Email == changePassword.Email);
                if (index != null)
                {
                    SignUp signUp2 = signUp.Where(x => x.Email == changePassword.Email).FirstOrDefault();
                    signUp2.Password = changePassword.Password;
                    signUp2.UpdatedDate = DateTime.Now;
                    signUp[index] = signUp2;
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
