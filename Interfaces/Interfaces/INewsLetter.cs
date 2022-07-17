using Interfaces.ViewModels.NewsLetterVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Interfaces
{
    public interface INewsLetter
    {
        Task<List<GetNewsLetterViewModel>> GetAll();
        Task<bool> Save(SaveNewsLetterViewModel model);
        Task<bool> CheckExist(string email);
        Task<bool> Delete(Guid id);
    }
}
