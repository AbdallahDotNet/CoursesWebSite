using Interfaces.ViewModels.ApplicationIntroVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Interfaces
{
    public interface IApplicationIntro
    {
        Task<GetApplicationIntroViewModel> Get();
        Task<SaveApplicationIntroViewModel> GetUpdatedApplicationIntro();
        Task<string> Save(SaveApplicationIntroViewModel model, string root);
    }
}
