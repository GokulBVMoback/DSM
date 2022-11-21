using DSM.API.Controllers;
using DSM.BAL.Abstraction;
using DSM.BAL.Service;
using DSM.Entities.Models;
using DSM.Model.DBModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

namespace DSM.APITests
{
    public class UserControllerTest
    {
        private readonly UserController _controller;
        private readonly IUserInterface _service;
        public UserControllerTest()
        {
            _service = new UserService2();
            _controller = new UserController( _service);
        }

        [Fact]
        public void Get_WhenCalled_ReturnsAllItems()
        {
            // Act
            var okResult = _controller.UserDetails();
            // Assert
            var items = Assert.IsType<List<UserDisplay>>(okResult.Value);
            Assert.Equal(3, items.Count);
        }

        [Fact]
        public void Add_ExtistingUser_ReturnsBadRequest()
        {
            // Arrange
            var ExtistingUser = new SignUp()
            {
                FirstName = "test",
                LastName = "test",
                Email = "bvgokulgok@gmail.com",
                PhoneNum = 1111111,
                Password = "test",
                TypeOfUser = 2,
            };
            _controller.ModelState.AddModelError("Email", "Extist");

            // Act
            var badResponse = _controller.SignUp(ExtistingUser);
            JsonResult jsonResult = new JsonResult("Your Email already registered. Please Log in");
            // Assert
            Assert.Matches(jsonResult.ToString(), badResponse.ToString());
        }

        [Fact]
        public void Add_New_User_Passed_ReturnsCreatedResponse()
        {
            // Arrange
            SignUp testUser = new SignUp()
            {
                FirstName="test",
                LastName="test",
                Email="test",
                PhoneNum=1111111,
                Password="test",
                TypeOfUser=2,
            };

            // Act
            var createdResponse = _controller.SignUp(testUser);
            JsonResult jsonResult = new JsonResult("Sign up successfully");

            // Assert
            Assert.Matches(jsonResult.ToString() , createdResponse.ToString());
        }

        [Fact]
        public void Login_Wrong_Password_ReturnsBadRequest()
        {
            // Arrange
            var WrongPassword = new LogIn()
            {
                Email = "bvgokulgok@gmail.com",
                Password = "4321",
            };
            _controller.ModelState.AddModelError("Password", "NotMatched");

            // Act
            var badResponse = _controller.LogIn(WrongPassword);
            JsonResult jsonResult = new JsonResult("Mail and Password not matched");

            // Assert
            Assert.Matches(jsonResult.ToString(), badResponse.ToString());
        }

        [Fact]
        public void Login_Correct_Password_ReturnsBadRequest()
        {
            // Arrange
            var WrongPassword = new LogIn()
            {
                Email = "bvgokulgok@gmail.com",
                Password = "1234",
            };

            // Act
            var goodResponse = _controller.LogIn(WrongPassword);
            JsonResult jsonResult = new JsonResult("hjLog in successfully");
            string expect = jsonResult.ToString();
            string actual=goodResponse.ToString();
            // Assert
            Assert.Matches(expect, actual);
        }
    }
}