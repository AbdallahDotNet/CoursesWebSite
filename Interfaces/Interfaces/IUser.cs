using Entitis.Models;
using Interfaces.ViewModels.UserVM;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Interfaces
{
    public interface IUser
    {
        Task<List<GetUserViewModel>> GetAll();
        Task<LoginReturnViewModel> Login(LoginViewModel model);
        JwtSecurityToken GenerateToken(IList<string> roles, Users user);
        Task<SaveUserViewModel> GetUpdatedUser(string id);
        Task<bool> SaveUser(SaveUserViewModel model);
        Task<bool> Delete(string id);
        Task<bool> CheckUserExist(string name);
        Task<bool> CheckRolesExist();
    }
}
