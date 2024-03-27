using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using MainLogin.Classes;
using MainLogin.Controllers;
using Dapper;


namespace MainLogin
{

    [ApiController]
    [Route("api/[controller]")]


    public class MainLoginControler : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private string connectionString;

        private static string globalGeneratedSalt = string.Empty;
        private static string globalGeneratedKey = string.Empty;

        public MainLoginControler(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet("Test Hash")]
        public string GetLogin()
        {
            PasswordHashing passwordHashing = new PasswordHashing();

            string[] tester = new string[2];
            tester = passwordHashing.getHashedPassword();

            globalGeneratedSalt = tester[0];
            globalGeneratedKey = tester[1];

            string returnValue = ("The Salt generated is: " + globalGeneratedSalt + " ,And the complete Password is: " + globalGeneratedKey);

            return returnValue;

        }

        [HttpGet("Test Hashed Password")]
        public string testLogin()
        {
            PasswordHashing passwordHashing = new PasswordHashing();

            string testSalt = "";
            string testKey = "";

            string[] tester = new string[2];
            tester = passwordHashing.getTestedPassword(globalGeneratedSalt);


            string returnSalt = tester[0];
            string returnKey = tester[1];

            string returnValue = string.Empty;

            if (returnSalt == globalGeneratedSalt && returnKey == globalGeneratedKey)
            {
                returnValue = "The salts and Passwords matched";
            }
            else
            {
                returnValue = ("The Salt generated is: " + returnSalt + " ,And the complete Password is: " + returnKey + "But the generated salt is: " + globalGeneratedSalt + "and the password is: " + globalGeneratedKey);
            }


            return returnValue;

        }

    }



}
