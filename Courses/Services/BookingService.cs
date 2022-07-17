using AutoMapper;
using Interfaces.Interfaces;
using Interfaces.ViewModels.BookingVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Courses.Services
{
    public class BookingService : IBooking
    {
        private DataContext _context;
        private ICoreBase _repoCore;
        private IMapper _mapper;
        public BookingService(DataContext context,
            ICoreBase repoCore,
            IMapper mapper)
        {
            _context = context;
            _repoCore = repoCore;
            _mapper = mapper;
        }

        public async Task<bool> CheckExist(string name, Guid course_id)
        {
            var is_booking_exist = await _context.Bookings
                .AnyAsync(x => x.Student_name == name && x.Course_id == course_id);
            return is_booking_exist;
        }

        public async Task<bool> Delete(Guid id)
        {
            var booking = await _context.Bookings.FirstOrDefaultAsync(x => x.Id == id);
            booking.Is_active = false;

            await _repoCore.SaveAll();
            return true;
        }

        public async Task<List<GetBookingViewModel>> GetAll()
        {
            var bookings = await _context.Bookings
                .Include(x => x.Courses)
                .ToListAsync();
            return _mapper.Map<List<GetBookingViewModel>>(bookings);
        }

        public async Task<bool> Save(SaveBookingViewModel model)
        {
            var reg = new Regex(@"^[^<>.,?;:'()!~%$\-_@#/*""]+$");
            if (!reg.IsMatch(model.Student_name) || !reg.IsMatch(model.Phone))
            {
                return false;
            }

            if (model.Id == Guid.Empty)
            {
                var booking = _mapper.Map<Entitis.Models.Booking>(model);
                booking.Student_name = model.Student_name.ToLower();
                booking.Is_active = true;
                _repoCore.Add(booking);
            }
            else
            {
                var booking = await _context.Bookings
                    .FirstOrDefaultAsync(x => x.Id == model.Id);
                _mapper.Map(model, booking);
            }

            await _repoCore.SaveAll();
            return true;
        }

        public List<GetBookingViewModel> Search(string key)
        {
            var bookings = _context.Bookings
                .Include(x => x.Courses)
                .ToList();

            if (!string.IsNullOrEmpty(key))
            {
                bookings = bookings
                .Where(x => x.Student_name.Contains(key)
                    || x.Phone.Contains(key)).ToList();
            }

            return _mapper.Map<List<GetBookingViewModel>>(bookings);
        }
    }
}
