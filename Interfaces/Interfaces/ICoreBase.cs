using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Interfaces
{
    public interface ICoreBase
    {
        void Add<T>(T entity);
        void Delete<T>(T entity);
        Task<bool> SaveAll();
        string GenerateRandomCodeAsNumber();
        string GenerateRandomCodeAsAlphanumeric();
        bool SaveSingleImage(string root, IFormFile file, out string file_name);
    }
}
