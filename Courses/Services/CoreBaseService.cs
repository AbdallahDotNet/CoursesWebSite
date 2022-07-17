using Interfaces.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Courses.Services
{
    public class CoreBaseService : ICoreBase
    {
        private DataContext _context;
        public CoreBaseService(DataContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity)
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity)
        {
            _context.Remove(entity);
        }

        public string GenerateRandomCodeAsAlphanumeric()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            return finalString;
        }

        public string GenerateRandomCodeAsNumber()
        {
            var random = new Random();
            int _min = 1000;
            int _max = 9999;
            int month = random.Next(_min, _max);

            return month.ToString();
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool SaveSingleImage(string root, IFormFile file, out string file_name)
        {
            string unique_file_name = null;

            if (file != null)
            {
                unique_file_name = Guid.NewGuid().ToString() + "_" + file.FileName;
                string full_path = Path.Combine(root, unique_file_name);

                using(var file_stream = new FileStream(full_path, FileMode.Create))
                {
                    file.CopyTo(file_stream);
                }
            }

            file_name = unique_file_name;
            return true;
        }
    }
}
