using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using MainLogin.Classes;
using Dapper;

namespace MainLogin
{

    [ApiController]
    [Route("api/[controller]")]


    public class MainLoginControler : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private string connectionString;

        public MainLoginControler(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet("getHash")]
        public string getHash()
        {
            bool registerSuccess = false;

            UserRegistration userRegistration = new UserRegistration();

            userRegistration.Firstname = "CodeSide";
            userRegistration.Lastname = "SaltedHash";
            userRegistration.Username = "Code2ndTest";
            userRegistration.UserRoleRestriction = 1;//admin
            userRegistration.UserPlatformRestriction = 1;//test value

            string testHash = "Faan12345";

            //userRegistration.Password = CalculateSHA512(testHash);
            userRegistration.Password = HashPasword(testHash, out var salt);
            userRegistration.Salt = Convert.ToHexString(salt);

            string dbHashVal = "0x5CADA057CD99EA85D1A8A65C7AB220606899672B9FDD0595D9A62447079298979F567DCC1AFCB8B1AC3A1AD697BD843E5DDEA09EF38D9E49566E73A1AF9965BF";//hash to compare

            if (userRegistration.Password == dbHashVal)
            {
                return "Password match";
            }
            else { return "Password NOT Match"; }


            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = connectionString;
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("sp_RegisterNewUser", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = userRegistration.Firstname;
                        cmd.Parameters.Add("@LastName", SqlDbType.VarChar).Value = userRegistration.Lastname;
                        cmd.Parameters.Add("@HashedPassword", SqlDbType.VarChar).Value = userRegistration.Password;
                        cmd.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userRegistration.Username;
                        cmd.Parameters.Add("@Salt", SqlDbType.VarChar).Value = userRegistration.Salt;
                        cmd.Parameters.Add("@UserRoleRestriction", SqlDbType.Int).Value = userRegistration.UserRoleRestriction;
                        cmd.Parameters.Add("@UserPlatformRestriction", SqlDbType.Int).Value = userRegistration.UserPlatformRestriction;


                        int sqlreturn = Convert.ToInt32(cmd.ExecuteScalar());
                        registerSuccess = sqlreturn > 0 ? true : false;
                        con.Close();
                    }
                }
                if (registerSuccess)
                {
                    return "success dbregidter";
                }
                else
                {
                    return "unsuccessfull DB register";
                }

            }
            catch (Exception)
            {
                return "Not Done ...Catch";
            }

        }

        [HttpGet("testHash")]
        public bool testHash()
        {
            string testHash = "Faan12345";
            string dbHashVal = "0x5CADA057CD99EA85D1A8A65C7AB220606899672B9FDD0595D9A62447079298979F567DCC1AFCB8B1AC3A1AD697BD843E5DDEA09EF38D9E49566E73A1AF9965BF";//hash to compare
            byte[] byteArray = Convert.FromHexString("F9E59C38518BE2C2CF82654FAD17175F55DFE3B2205E467112B4E5EF0A09657224FD2222072E50CB512533222D628C9E287BF3CA6C4D6F64D1BA8DFD1BB9311D");
            //string endHashVal = CalculateSHA512(inputHash);
            bool endHashVal = VerifyPassword(testHash, dbHashVal, byteArray);

            return endHashVal;
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


        const int keySize = 64;
        const int iterations = 350000;
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
        string HashPasword(string password, out byte[] salt)
        {
            salt = RandomNumberGenerator.GetBytes(keySize);

            string testsalt = "F9E59C38518BE2C2CF82654FAD17175F55DFE3B2205E467112B4E5EF0A09657224FD2222072E50CB512533222D628C9E287BF3CA6C4D6F64D1BA8DFD1BB9311D";

            byte[] byteArray = new byte[testsalt.Length / 2];

            for (int i = 0; i < byteArray.Length; i++)
            {
                byteArray[i] = Convert.ToByte(testsalt.Substring(i * 2, 2), 16);
            }

            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                byteArray,
                iterations,
                hashAlgorithm,
                keySize);
            return Convert.ToHexString(hash);
        }

        bool VerifyPassword(string password, string hash, byte[] salt)
        {
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, hashAlgorithm, keySize);
            byte[] byteArray = new byte[hash.Length / 2];

            for (int i = 0; i < byteArray.Length; i++)
            {
                byteArray[i] = Convert.ToByte(hash.Substring(i * 2, 2), 16);
            }
            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
        }




        //TODO
        //Test the connection to MainLogin db
        //DB connection failure... check setup
    }


}
