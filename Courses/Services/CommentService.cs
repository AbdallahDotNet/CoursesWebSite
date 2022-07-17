using AutoMapper;
using Interfaces.Interfaces;
using Interfaces.ViewModels.CommentVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Courses.Services
{
    public class CommentService : IComment
    {
        private DataContext _context;
        private ICoreBase _repoCore;
        private ISetting _repoSetting;
        private IMapper _mapper;
        public CommentService(DataContext context,
            ICoreBase repoCore,
            ISetting repoSetting,
            IMapper mapper)
        {
            _context = context;
            _repoCore = repoCore;
            _repoSetting = repoSetting;
            _mapper = mapper;
        }

        public async Task<bool> Approving(Guid id)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
            comment.Approval = true;

            await _repoCore.SaveAll();
            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
            comment.Is_active = false;

            await _repoCore.SaveAll();
            return true;
        }

        public async Task<List<GetCommentViewModel>> GetAll()
        {
            var setting = await _repoSetting.Get();
            var comments = await _context.Comments
                .Include(x => x.Course_specialty)
                .OrderByDescending(x => x.Date)
                .ToListAsync();
            comments.ForEach(x => x.Date = x.Date.AddHours(setting.Time_difference));
            return _mapper.Map<List<GetCommentViewModel>>(comments);
        }

        public async Task<SaveCommentViewModel> GetUpdatedComment(Guid id)
        {
            var comment = await _context.Comments
                .FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<SaveCommentViewModel>(comment);
        }

        public async Task<bool> Save(SaveCommentViewModel model)
        {
            var reg = new Regex(@"^[^<>.,?;:'()!~%$\-_@#/*""]+$");
            if (!reg.IsMatch(model.Student_name) || !reg.IsMatch(model.Content))
            {
                return false;
            }

            if (model.Id == Guid.Empty)
            {
                var comment = _mapper.Map<Entitis.Models.Comments>(model);
                comment.Date = DateTime.UtcNow;
                comment.Is_active = true;
                _repoCore.Add(comment);
            }
            else
            {
                var comment = await _context.Comments
                    .FirstOrDefaultAsync(x => x.Id == model.Id);
                _mapper.Map(model , comment);
            }

            await _repoCore.SaveAll();
            return true;
        }
    }
}
