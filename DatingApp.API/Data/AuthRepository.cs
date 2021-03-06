using System;
using System.Threading.Tasks;
using DatingApp.API.models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            this._context = context;

        }
        public async Task<User> Login(string username, string password)
        {

            // get a user object based on the username
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);

            if(user == null)
            {
                return null;
            }

            if(!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
             using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                // this will create a new computed hash but with the key. The computed hash should be the same as
                // the passwordHash stroed in the user object.
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for(int i = 0; i < computedHash.Length; i++)
                {
                    if(computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                     
                     
                }

                return true;
            }
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;

            // bytes are value types. we want to pass the vairable as a reference type. Prefix out keyword to the variables in
            // the function call

             CreatePasswordHash(password, out passwordHash, out passwordSalt);

             user.PasswordHash = passwordHash;
             user.PasswordSalt = passwordSalt;

             await _context.Users.AddAsync(user); // add new data to datacontext
             await _context.SaveChangesAsync(); // save changes back to db

             return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
            
        }

        public async Task<bool> UserExists(string username)
        {
            if(await _context.Users.AnyAsync(x => x.UserName == username))
            {
                return true;
            }
            
            return false;
        }
    }
}