using AutoMapper;
using Interfaces.Interfaces;
using Interfaces.ViewModels.ApplicationIntroVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Courses.Services
{
    public class ApplicationIntroService : IApplicationIntro
    {
        private DataContext _context;
        private ICoreBase _repoCore;
        private IMapper _mapper;
        public ApplicationIntroService(DataContext context,
            ICoreBase repoCore,
            IMapper mapper)
        {
            _context = context;
            _repoCore = repoCore;
            _mapper = mapper;
        }

        public async Task<GetApplicationIntroViewModel> Get()
        {
            var app_intro = await _context.Application_intros.FirstOrDefaultAsync();
            return _mapper.Map<GetApplicationIntroViewModel>(app_intro);
        }

        public async Task<SaveApplicationIntroViewModel> GetUpdatedApplicationIntro()
        {
            var app_intro = await _context.Application_intros.FirstOrDefaultAsync();
            return _mapper.Map<SaveApplicationIntroViewModel>(app_intro);
        }

        public async Task<string> Save(SaveApplicationIntroViewModel model, string root)
        {
            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (!regex.IsMatch(model.Email))
                return "Email invalid!";

            if (model.Id == Guid.Empty)
            {
                if (model.File == null)
                    return "Please upload image!";

                string file_name = null;
                var upload = _repoCore.SaveSingleImage(root, model.File, out file_name);

                if (!upload)
                    return "Error while uploading!";

                var app_intro = _mapper.Map<Entitis.Models.Application_intro>(model);
                app_intro.Logo = file_name;
                app_intro.Is_active = true;

                _repoCore.Add(app_intro);
            }
            else
            {
                string file_name = null;

                if (model.File != null)
                {
                    var full_path = root + "/" + model.Logo;
                    if (System.IO.File.Exists(full_path))
                    {
                        System.IO.File.Delete(full_path);
                    }

                    var upload = _repoCore.SaveSingleImage(root, model.File, out file_name);

                    if (!upload)
                        return "Error while uploading!";
                }

                var app_intro = await _context.Application_intros.FirstOrDefaultAsync();
                _mapper.Map(model, app_intro);

                app_intro.Logo = (model.File == null) ? model.Logo : file_name;
            }

            await _repoCore.SaveAll();
            return string.Empty;
        }
    }
}
