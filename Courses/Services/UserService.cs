using AutoMapper;
using Entitis.Models;
using Interfaces.Interfaces;
using Interfaces.ViewModels.UserVM;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Courses.Services
{
    public class UserService : IUser
    {
        private UserManager<Users> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private ICoreBase _repoCore;
        private IConfiguration _configuration;
        private IMapper _mapper;
        private DataContext _context;
        public UserService(UserManager<Users> userManager,
            RoleManager<IdentityRole> roleManager,
            ICoreBase repoCore,
            IConfiguration configuration,
            IMapper mapper,
            DataContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _repoCore = repoCore;
            _configuration = configuration;
            _mapper = mapper;
            _context = context;
        }

        public async Task<bool> CheckRolesExist()
        {
            var Is_roles_exist = await _context.Roles.AnyAsync();
            return Is_roles_exist;
        }

        public async Task<bool> CheckUserExist(string name)
        {
            var Is_user_exist = await _context.Users.AnyAsync(x => x.Name == name);
            return Is_user_exist;
        }

        public async Task<bool> Delete(string id)
        {
            if (_context.Users.Count() == 1)
            {
                return false;
            }

            var user = await _userManager.FindByIdAsync(id);
            _repoCore.Delete(user);

            await _repoCore.SaveAll();
            return true;
        }

        public JwtSecurityToken GenerateToken(IList<string> Roles, Users user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var auth_key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddYears(1),
                claims: claims,
                signingCredentials: new SigningCredentials(auth_key, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        public async Task<List<GetUserViewModel>> GetAll()
        {
            var users = await _context.Users.ToListAsync();
            return _mapper.Map<List<GetUserViewModel>>(users);
        }

        public async Task<SaveUserViewModel> GetUpdatedUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return _mapper.Map<SaveUserViewModel>(user);
        }

        public async Task<LoginReturnViewModel> Login(LoginViewModel model)
        {
            var user = new Users();

            var reg = new Regex(@"^[^<>.,?;:'()!~%\-_@#/*""\s]+$");
            if (!reg.IsMatch(model.Name))
            {
                return null;
            }

            if (!await CheckUserExist(model.Name))
            {
                return null;
            }

            user = await _context.Users
                .FirstOrDefaultAsync(x => x.Name == model.Name);

            if (!await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return null;
            }

            var roles = await _userManager.GetRolesAsync(user);
            return new LoginReturnViewModel
            {
                Token = GenerateToken(roles, user)
            };
        }

        public async Task<bool> SaveUser(SaveUserViewModel model)
        {
            if (!await CheckRolesExist())
            {
                var role = new IdentityRole { Name = "Admin", NormalizedName = "ADMIN"};
                await _roleManager.CreateAsync(role);
            }

            if (string.IsNullOrEmpty(model.Id))
            {
                var user = _mapper.Map<Entitis.Models.Users>(model);
                user.Id = Guid.NewGuid().ToString();
                user.UserName = _repoCore.GenerateRandomCodeAsNumber() + model.Name;

                var res = await _userManager.CreateAsync(user, model.Password);
                if (!res.Succeeded)
                {
                    return false;
                }
                await _userManager.AddToRoleAsync(user, "Admin");
            }
            else
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                _mapper.Map(model, user);

                if (!string.IsNullOrEmpty(model.New_password))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    await _userManager.ResetPasswordAsync(user, token, model.New_password);
                }

                await _repoCore.SaveAll();
            }

            return true;
        }
    }
}
