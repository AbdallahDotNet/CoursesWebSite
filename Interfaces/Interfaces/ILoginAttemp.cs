using Interfaces.ViewModels.LoginAtemmpVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Interfaces
{
    public interface ILoginAttemp
    {
        Task<List<GetLoginAttempViewModel>> GetAll();
        Task<List<GetLoginAttempViewModel>> Search(string key);
        Task<bool> SaveLoginAttemps(string ip_address);
        Task<bool> CheckIpBlocked(string ip_address);
        Task<bool> RemoveBlock(Guid id);
    }
}
