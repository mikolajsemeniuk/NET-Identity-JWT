using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using app.Interfaces;
using app.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace app.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly SymmetricSecurityKey _key;
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;

        public UserRepository(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        public async Task<string> GetJWTTokenAsync(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.NameId, "1"),
                new Claim(JwtRegisteredClaimNames.UniqueName, "username")

            };
            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);
            return tokenHandler.WriteToken(token);
        }

        public async Task<string> SignInAsync(string email, string password)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(
                user => user.Email == email);

            if (user == null)
                throw new Exception("Invalid Username");

            var wasUserSignedIn = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (!wasUserSignedIn.Succeeded)
                throw new Exception("Invalid password");

            return await GetJWTTokenAsync(user);
        }

        public async Task<string> SignUpAsync(string username, string email, string password)
        {
            if (await _userManager.Users.AnyAsync(user => user.UserName == username))
                throw new Exception("Username is taken");

            if (await _userManager.Users.AnyAsync(user => user.Email == email))
                throw new Exception("Email is taken");

            var user = new User 
            { 
                Email = email,
                UserName = username 
            };

            var wasUserSaved = await _userManager.CreateAsync(user, password);
            if (!wasUserSaved.Succeeded)
                throw new Exception("something gone wrong try later");
            
            var wereRolesGranted = await _userManager.AddToRolesAsync(user, new[] { "member" });
            if (!wereRolesGranted.Succeeded)
                throw new Exception("problem granting role");

            return await GetJWTTokenAsync(user);
        }
    }
}