using DSM.Entities.Models;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSM.Model;
using DSM.Model.DBModel;

namespace DSM.BAL.Abstraction
{
    public interface IUserInterface
    {
        List<SignUp> GetUser();
        string SignUp(SignUp signUp);

        bool LogIn(LogIn login);
        string ForgetPassword(ChangePassword changePassword);
    }
}
