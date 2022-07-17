using AutoMapper;
using Interfaces.Interfaces;
using Interfaces.ViewModels.NotificationVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Courses.Services
{
    public class NotificationService : INotification
    {
        private DataContext _context;
        private ICoreBase _repoCore;
        private ISetting _repoSetting;
        private IMapper _mapper;
        public NotificationService(DataContext context,
            ICoreBase repoCore,
            ISetting repoSetting,
            IMapper mapper)
        {
            _context = context;
            _repoCore = repoCore;
            _repoSetting = repoSetting;
            _mapper = mapper;
        }

        public async Task<List<GetNotificationViewModel>> GetAll()
        {
            var setting = await _repoSetting.Get();
            var notifications = await _context.Notifications
                .OrderByDescending(x => x.Date)
                .Take(5)
                .ToListAsync();
            notifications.ForEach(x => x.Date = x.Date.AddHours(setting.Time_difference));
            notifications.Select(x => x.Date.AddHours(setting.Time_difference).ToString("d/M/yyyy H:m:s"));
            return _mapper.Map<List<GetNotificationViewModel>>(notifications);
        }

        public int GetNotificationUnSeenCount()
        {
            var notification_unSeen_count = _context.Notifications
                .Where(x => x.Seen == false).Count();
            return notification_unSeen_count;
        }

        public async Task<bool> Save(SaveNotificationViewModel model)
        {
            var notification = _mapper.Map<Entitis.Models.Notifications>(model);
            notification.Date = DateTime.Now;
            _repoCore.Add(notification);

            await _repoCore.SaveAll();
            return true;
        }

        public async Task<bool> UpdateSeen()
        {
            var notifications = await _context.Notifications.ToListAsync();

            foreach (var notify in notifications)
            {
                notify.Seen = true;
            }
            await _repoCore.SaveAll();
            return true;
        }
    }
}
