using Interfaces.ViewModels.FaqVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Interfaces
{
    public interface IFaq
    {
        Task<List<GetFaqViewModel>> GetAll();
        Task<SaveFaqViewModel> GetUpdatedFaq(Guid id);
        Task<bool> Save(SaveFaqViewModel model);
        Task<bool> Delete(Guid id);
    }
}
