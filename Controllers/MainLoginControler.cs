/*using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;

namespace MainLogin.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]

    public class MainLoginControler : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private string connectionString;

        public MainLoginControler(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }


        static string CalculateSHA512(string input)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashedBytes = sha512.ComputeHash(inputBytes);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    builder.Append(hashedBytes[i].ToString("x2")); // "x2" format specifier for hexadecimal
                }
                return builder.ToString();
            }
        }



        [HttpGet(Name = "GetHashVal")]
        public string getHash()
        {
            string testHash = "Faan12345";
            string EndHash = string.Empty;

            EndHash = CalculateSHA512(testHash);


            return EndHash;

        }

        *//*[HttpGet(Name = "TestHashValue")]*//*
        public bool testHash()
        {
            string inputHash = "Faan12345";
            string dbHashVal = "63028207695b4f6deae55810193d29690563854f349837dc91dfeb43066b0b653c1917dc3e0bff154287730ff27c001bb98bb8f56848c368deb080c909e8b0ec";
            string endHashVal = string.Empty;

            endHashVal = CalculateSHA512(inputHash);

            if (endHashVal == dbHashVal)
            {
                return true;
            }

            return false;
        }


        *//*public MainLoginControler(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }*//*
        //TODO
        //Create a class to accomodate Login and Register

        //Test the connection to MainLogin db

    }
}
*/