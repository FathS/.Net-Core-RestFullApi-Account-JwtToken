using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TestApi.Models.Data.Entities;

namespace TestApi.Helpers
{
    public class HashingPassword
    {
        //private static void HashingPassword(string password)
        //{
        //    var rng = RandomNumberGenerator.Create();
        //    var saltBytes = new byte[16];
        //    rng.GetBytes(saltBytes);
        //    var saltText = Convert.ToBase64String(saltBytes);

        //    var sha = SHA256.Create();
        //    var saltedPassord = password + saltText;
        //    var saltedhashedPassword = Convert.ToBase64String(sha.ComputeHash(Encoding.Unicode.GetBytes(saltedPassord)));

        //    var User = new UserPassword
        //    {
        //        Salt = saltText,
        //        SaltedHashedPassword = saltedhashedPassword,
        //    };
        //}
    }
}
