using Interfaces.ViewModels.NotifySettingVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Interfaces
{
    public interface ISetting
    {
        Task<GetSettingViewModel> Get();
        Task<SaveSettingViewModel> GetUpdatedSetting();
        Task<string> Save(SaveSettingViewModel model);
    }
}
