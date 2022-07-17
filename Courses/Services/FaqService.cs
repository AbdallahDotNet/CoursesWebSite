using AutoMapper;
using Entitis.Models;
using Interfaces.Interfaces;
using Interfaces.ViewModels.FaqVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Courses.Services
{
    public class FaqService : IFaq
    {
        private DataContext _context;
        private ICoreBase _repoCore;
        private IMapper _mapper;
        public FaqService(DataContext context,
            ICoreBase repoCore,
            IMapper mapper)
        {
            _context = context;
            _repoCore = repoCore;
            _mapper = mapper;
        }

        public async Task<bool> Delete(Guid id)
        {
            var faq = await _context.Faqs.FirstOrDefaultAsync(x => x.Id == id);
            faq.Is_active = false;

            await _repoCore.SaveAll();
            return true;
        }

        public async Task<List<GetFaqViewModel>> GetAll()
        {
            var faqs = await _context.Faqs.ToListAsync();
            return _mapper.Map<List<GetFaqViewModel>>(faqs);
        }

        public async Task<SaveFaqViewModel> GetUpdatedFaq(Guid id)
        {
            var faq = await _context.Faqs.FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<SaveFaqViewModel>(faq);
        }

        public async Task<bool> Save(SaveFaqViewModel model)
        {
            if (model.Id == Guid.Empty)
            {
                var faq = _mapper.Map<Faqs>(model);
                faq.Is_active = true;
                _repoCore.Add(faq);
            }
            else
            {
                var faq = await _context.Faqs.FirstOrDefaultAsync(x => x.Id == model.Id);
                _mapper.Map(model, faq);
            }

            await _repoCore.SaveAll();
            return true;
        }
    }
}
