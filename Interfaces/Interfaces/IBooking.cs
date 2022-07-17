using Interfaces.ViewModels.BookingVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Interfaces
{
    public interface IBooking
    {
        Task<List<GetBookingViewModel>> GetAll();
        List<GetBookingViewModel> Search(string key);
        Task<bool> Save(SaveBookingViewModel model);
        Task<bool> Delete(Guid id);
        Task<bool> CheckExist(string name, Guid course_id);
    }
}
