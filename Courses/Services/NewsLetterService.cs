using AutoMapper;
using Interfaces.Interfaces;
using Interfaces.ViewModels.NewsLetterVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Courses.Services
{
    public class NewsLetterService : INewsLetter
    {
        private DataContext _context;
        private ICoreBase _repoCore;
        private IMapper _mapper;
        public NewsLetterService(DataContext context,
            ICoreBase repoCore,
            IMapper mapper)
        {
            _context = context;
            _repoCore = repoCore;
            _mapper = mapper;
        }

        public async Task<bool> CheckExist(string email)
        {
            var is_email_exist = await _context.NewsLetters.AnyAsync(x => x.Email == email);
            return is_email_exist;
        }

        public async Task<bool> Delete(Guid id)
        {
            var news_letter = await _context.NewsLetters.FirstOrDefaultAsync(x => x.Id == id);
            news_letter.Is_active = false;

            await _repoCore.SaveAll();
            return true;
        }

        public async Task<List<GetNewsLetterViewModel>> GetAll()
        {
            var news_letters = await _context.NewsLetters.ToListAsync();
            return _mapper.Map<List<GetNewsLetterViewModel>>(news_letters);
        }

        public async Task<bool> Save(SaveNewsLetterViewModel model)
        {
            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

            if (!regex.IsMatch(model.Email))
                return false;

            var news_letter = _mapper.Map<Entitis.Models.NewsLetter>(model);
            news_letter.Is_active = true;
            _repoCore.Add(news_letter);

            await _repoCore.SaveAll();
            return true;
        }
    }
}
