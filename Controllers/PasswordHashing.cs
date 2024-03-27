using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using System.Collections;

namespace MainLogin.Controllers
{


    public class PasswordHashing : ControllerBase
    {
        const int keySize = 64;
        const int iterations = 350000;
        private static byte[] globalSalt = new byte[64];

        private byte[] getSalt()
        {
            return globalSalt = RandomNumberGenerator.GetBytes(keySize);
        }

        private string getHashAlgorithm(string inputPassword, byte[] bSalt)
        {

            HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

            var hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(inputPassword), bSalt, iterations, hashAlgorithm, keySize);
            return Convert.ToHexString(hash);

            
        }

        private string getHashAlgorithmTest(string inputPassword, byte[] bSalt)
        {

            HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;



            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(inputPassword), bSalt, iterations, hashAlgorithm, keySize);
            string returnHash = Convert.ToHexString(hash);
            return returnHash;


        }

        public string[] getHashedPassword()
        {
            //test data
            string dummyPassword = "Faan";

            byte[] generatedSalt = getSalt();
            string hashedPassword = getHashAlgorithm(dummyPassword, generatedSalt);

            string[] generatedData = new string[2];

            string saltString = Convert.ToBase64String(generatedSalt);

            generatedData[0] = saltString;
            generatedData[1] = hashedPassword;

            return generatedData;

        }



        public string[] getTestedPassword(string dummySalt)
        {
            string dummyPassword = "Faan";

            byte[] byteSalt = globalSalt;
            
            string hashedPasswordTest = getHashAlgorithmTest(dummyPassword, byteSalt);

            string[] generatedData = new string[2];

            generatedData[0] = Convert.ToBase64String(byteSalt);
            generatedData[1] = hashedPasswordTest;

            return generatedData;
        }


    }

}
