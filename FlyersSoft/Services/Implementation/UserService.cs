using FlyersSoft.Model;
using FlyersSoft.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace FlyersSoft.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly MyDBContext myDBContext;
        private readonly IConfiguration config;
        private byte[] IV =
        {
            0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
            0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16
        };
        public UserService(MyDBContext _myDBContext, IConfiguration _config)
        {
            myDBContext = _myDBContext;
            config = _config;
        }
        public async Task<JsonResult> addNewUser(User user)
        {
            if(myDBContext.Users.Where(x=> x.userName == user.userName).Any())
                return new JsonResult(new { msg = "Username already exists" });

            if(string.IsNullOrEmpty(user.passwordString))
                return new JsonResult(new { msg = "Invalid password" });

            user.password = await EncryptAsync(user.passwordString);
            user.isActive = true;
            user.createdBy = 0;//userid needs to implement;
            user.createdOn = DateTime.Now;
            user.passwordString = null;
            var insertedUser =  myDBContext.Users.Add(user).Entity;
            myDBContext.SaveChanges();
            insertedUser.password = null;
            return new JsonResult(new { msg = "success", resp = insertedUser });
        }
        
        public async Task<bool> isValidUser(string userName, string password)
        {
            
            if (!myDBContext.Users.Where(x => x.userName == userName).Any())
                return false;

            User user = myDBContext.Users.Where(x => x.userName == userName).FirstOrDefault();
            if (user?.password == null)
                return false;

            string decryptedPassword = await DecryptAsync(user.password);
            if(decryptedPassword != password)
                return false;
            return true;
        }

        private byte[] DeriveKeyFromPassword(string password)
        {
            var emptySalt = Array.Empty<byte>();
            var iterations = 1000;
            var desiredKeyLength = 16; 
            var hashMethod = HashAlgorithmName.SHA384;
            return Rfc2898DeriveBytes.Pbkdf2(Encoding.Unicode.GetBytes(password),
                                             emptySalt,
                                             iterations,
                                             hashMethod,
                                             desiredKeyLength);
        }

        public async Task<byte[]> EncryptAsync(string clearText)
        {
            using Aes aes = Aes.Create();
            aes.Key = DeriveKeyFromPassword(config.GetValue<string>("passphrase"));
            aes.IV = IV;
            using MemoryStream output = new();
            using CryptoStream cryptoStream = new(output, aes.CreateEncryptor(), CryptoStreamMode.Write);
            await cryptoStream.WriteAsync(Encoding.Unicode.GetBytes(clearText));
            await cryptoStream.FlushFinalBlockAsync();
            return output.ToArray();
        }

        public async Task<string> DecryptAsync(byte[] encrypted)
        {
            using Aes aes = Aes.Create();
            aes.Key = DeriveKeyFromPassword(config.GetValue<string>("passphrase"));
            aes.IV = IV;
            using MemoryStream input = new(encrypted);
            using CryptoStream cryptoStream = new(input, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using MemoryStream output = new();
            await cryptoStream.CopyToAsync(output);
            return Encoding.Unicode.GetString(output.ToArray());
        }

    }
}
