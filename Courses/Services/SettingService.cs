using AutoMapper;
using Interfaces.Interfaces;
using Interfaces.ViewModels.NotifySettingVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Courses.Services
{
    public class SettingService : ISetting
    {
        private DataContext _context;
        private ICoreBase _repoCore;
        private IMapper _mapper;
        private IHashString _hash;
        public SettingService(DataContext context,
            ICoreBase repoCore,
            IMapper mapper,
            IHashString hash)
        {
            _context = context;
            _repoCore = repoCore;
            _mapper = mapper;
            _hash = hash;
        }

        public async Task<GetSettingViewModel> Get()
        {
            var setting = await _context.Settings.FirstOrDefaultAsync();
            return _mapper.Map<GetSettingViewModel>(setting);
        }

        public async Task<SaveSettingViewModel> GetUpdatedSetting()
        {
            var setting = await _context.Settings.FirstOrDefaultAsync();
            return _mapper.Map<SaveSettingViewModel>(setting);
        }

        public async Task<string> Save(SaveSettingViewModel model)
        {
            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (!regex.IsMatch(model.Email))
                return "Invalid Email Adderss!";

            if (model.Id == Guid.Empty)
            {
                if (string.IsNullOrEmpty(model.Password))
                    return "Password Required!";

                var setting = _mapper.Map<Entitis.Models.Setting>(model);
                setting.Password = _hash.Encrypt(model.Password);

                _repoCore.Add(setting);
            }
            else
            {
                var setting = await _context.Settings.FirstOrDefaultAsync();
                _mapper.Map(model, setting);

                setting.Password = (!string.IsNullOrEmpty(model.New_password))? _hash.Encrypt(model.New_password) : model.Password;
            }

            await _repoCore.SaveAll();
            return string.Empty;
        }
    }
}
