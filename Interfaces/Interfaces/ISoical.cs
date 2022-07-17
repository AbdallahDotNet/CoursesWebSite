using Interfaces.ViewModels.SoicalVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Interfaces
{
    public interface ISoical
    {
        Task<GetSoicalViewModel> Get();
        Task<SaveSoicalViewModel> GetUpdateSoical();
        Task<bool> Save(SaveSoicalViewModel model);
    }
}
