using AutoMapper;
using Entitis.Models;
using Interfaces.Interfaces;
using Interfaces.ViewModels.LoginAtemmpVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Courses.Services
{
    public class LoginAttempService : ILoginAttemp
    {
        private DataContext _context;
        private ICoreBase _repoCore;
        private ISetting _repoSetting;
        private IMapper _mapper;
        public LoginAttempService(DataContext context,
            ICoreBase repoCore,
            ISetting repoSetting,
            IMapper mapper)
        {
            _context = context;
            _repoCore = repoCore;
            _repoSetting = repoSetting;
            _mapper = mapper;
        }

        public async Task<List<GetLoginAttempViewModel>> GetAll()
        {
            var setting = await _repoSetting.Get();
            var login_attemps = await _context.login_Attempts.ToListAsync();
            login_attemps.ForEach(x => x.Date = x.Date.AddHours(setting.Time_difference));
            login_attemps.Select(x => x.Date.ToString("d/M/yyyy H:m:s"));
            return _mapper.Map<List<GetLoginAttempViewModel>>(login_attemps);
        }

        public async Task<bool> CheckIpBlocked(string ip_address)
        {
            if (await _context.login_Attempts.AnyAsync(x => x.Ip_address == ip_address))
            {
                var login_attemp = await _context.login_Attempts.FirstOrDefaultAsync(x => x.Ip_address == ip_address);
                if (login_attemp.Blocked == true)
                    return true;
            }
            return false;
        }


        public async Task<bool> SaveLoginAttemps(string ip_address)
        {
            var login_attemps = new Login_attempts();
            if (!await _context.login_Attempts.AnyAsync(x => x.Ip_address == ip_address))
            {
                login_attemps = new Login_attempts
                {
                    Id = Guid.NewGuid(),
                    Ip_address = ip_address,
                    Date = DateTime.UtcNow
                };
                login_attemps.Attemps += 1;

                _repoCore.Add(login_attemps);
            }
            else
            {
                login_attemps = await _context.login_Attempts
                    .FirstOrDefaultAsync(x => x.Ip_address == ip_address);
                if (login_attemps.Attemps < 2)
                    login_attemps.Attemps += 1;

                login_attemps.Blocked = true;
            }

            await _repoCore.SaveAll();
            return true;
        }

        public async Task<bool> RemoveBlock(Guid id)
        {
            var login_attemp = await _context.login_Attempts
                .FirstOrDefaultAsync(x => x.Id == id);
            _repoCore.Delete(login_attemp);

            await _repoCore.SaveAll();
            return true;
        }

        public async Task<List<GetLoginAttempViewModel>> Search(string key)
        {
            var setting = await _repoSetting.Get();
            var login_attemps = await _context.login_Attempts
                .Where(x => x.Ip_address.Contains(key))
                .ToListAsync();
            login_attemps.Select(x => x.Date.AddHours(setting.Time_difference).ToString("d/M/yyyy H:m:s"));
            return _mapper.Map<List<GetLoginAttempViewModel>>(login_attemps);
        }
    }
}
