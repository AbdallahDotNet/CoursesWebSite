using Interfaces.Helper;
using Interfaces.ViewModels.EventVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Interfaces
{
    public interface IEvent
    {
        Task<List<GetEventViewModel>> GetAll();
        Task<SaveEventViewModel> GetUpdatedEvent(Guid id);
        Task<string> Save(SaveEventViewModel model, string root, string culture_name);
        Task<bool> Delete(Guid id, string root);
        Task<bool> CheckExist(Guid id);
    }
}
