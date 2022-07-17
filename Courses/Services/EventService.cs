using AutoMapper;
using Interfaces.Helper;
using Interfaces.Interfaces;
using Interfaces.ViewModels.EventVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Courses.Services
{
    public class EventService : IEvent
    {
        private DataContext _context;
        private ICoreBase _repoCore;
        private ISetting _repoSetting;
        private IMapper _mapper;
        public EventService(DataContext context,
            ICoreBase repoCore,
            ISetting repoSetting,
            IMapper mapper)
        {
            _context = context;
            _repoCore = repoCore;
            _repoSetting = repoSetting;
            _mapper = mapper;
        }

        public async Task<bool> CheckExist(Guid id)
        {
            var is_event_exist = await _context.Events.AnyAsync(x => x.Id == id);
            return is_event_exist;
        }

        public async Task<bool> Delete(Guid id, string root)
        {
            var _event = await _context.Events.FirstOrDefaultAsync(x => x.Id == id);
            _event.Is_active = false;

            await _repoCore.SaveAll();
            return true;
        }

        public async Task<List<GetEventViewModel>> GetAll()
        {
            var setting = await _repoSetting.Get();
            var _events = await _context.Events
                .OrderByDescending(x => x.Date)
                .ToListAsync();
            _events.ForEach(x => x.Date = x.Date.AddHours(setting.Time_difference));
            return _mapper.Map<List<GetEventViewModel>>(_events);
        }

        public async Task<SaveEventViewModel> GetUpdatedEvent(Guid id)
        {
            var _event = await _context.Events.FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<SaveEventViewModel>(_event);
        }

        public async Task<string> Save(SaveEventViewModel model, string root, string culture_name)
        {
            if (model.Id == Guid.Empty)
            {
                if (model.File == null)
                    if (culture_name == "en")
                        return "Please upload image!";
                    else
                        return "برجاء رفع صوره!";

                string file_name = null;
                var upload = _repoCore.SaveSingleImage(root, model.File, out file_name);

                if (!upload)
                    if (culture_name == "en")
                        return "Error while uploading!";
                    else
                        return "خطأ اثناء الرفع!";

                if (model.Content.Length < 140)
                    if (culture_name == "en")
                        return "Content must not be less than 140 characters";
                    else
                        return "المحتوى يجب ان لا يقل عن 140 حرف";

                var _event = _mapper.Map<Entitis.Models.Events>(model);
                _event.Image = file_name;
                _event.Date = DateTime.UtcNow;
                _event.Is_active = true;

                _repoCore.Add(_event);
            }
            else
            {
                string file_name = null;

                if (model.File != null)
                {
                    var full_path = root + "/" + model.Image;
                    if (System.IO.File.Exists(full_path))
                    {
                        System.IO.File.Delete(full_path);
                    }

                    var upload = _repoCore.SaveSingleImage(root, model.File, out file_name);

                    if (!upload)
                        if (culture_name == "en")
                            return "Error while uploading!";
                        else
                            return "خطأ اثناء الرفع!";
                }

                var _event = await _context.Events.FirstOrDefaultAsync(x => x.Id == model.Id);
                _mapper.Map(model, _event);

                _event.Image = (model.File == null) ? model.Image : file_name;
            }

            await _repoCore.SaveAll();
            return string.Empty;
        }
    }
}
