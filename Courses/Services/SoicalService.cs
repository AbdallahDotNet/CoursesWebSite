using AutoMapper;
using Interfaces.Interfaces;
using Interfaces.ViewModels.SoicalVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Courses.Services
{
    public class SoicalService : ISoical
    {
        private DataContext _context;
        private ICoreBase _repoCore;
        private IMapper _mapper;
        public SoicalService(DataContext context,
            ICoreBase repoCore,
            IMapper mapper)
        {
            _context = context;
            _repoCore = repoCore;
            _mapper = mapper;
        }

        public async Task<GetSoicalViewModel> Get()
        {
            var soical = await _context.Soicals.FirstOrDefaultAsync();
            return _mapper.Map<GetSoicalViewModel>(soical);
        }

        public async Task<SaveSoicalViewModel> GetUpdateSoical()
        {
            var soical = await _context.Soicals.FirstOrDefaultAsync();
            return _mapper.Map<SaveSoicalViewModel>(soical);
        }

        public async Task<bool> Save(SaveSoicalViewModel model)
        {
            if (model.Id == Guid.Empty)
            {
                var soical = _mapper.Map<Entitis.Models.Soical>(model);
                _repoCore.Add(soical);
            }
            else
            {
                var soical = await _context.Soicals.FirstOrDefaultAsync();
                _mapper.Map(model, soical);
            }

            await _repoCore.SaveAll();
            return true;
        }
    }
}
