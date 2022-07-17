using AutoMapper;
using Interfaces.Helper;
using Interfaces.Interfaces;
using Interfaces.ViewModels.BlogVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Courses.Services
{
    public class BlogService : IBlog
    {
        private DataContext _context;
        private ICoreBase _repoCore;
        private ISetting _repoSetting;
        private IMapper _mapper;
        public BlogService(DataContext context,
            ICoreBase repoCore,
            ISetting repoSetting,
            IMapper mapper)
        {
            _context = context;
            _repoCore = repoCore;
            _repoSetting = repoSetting;
            _mapper = mapper;
        }

        public async Task<bool> CheckExist(Guid id)
        {
            var is_blog_exist = await _context.Blogs.AnyAsync(x => x.Id == id);
            return is_blog_exist;
        }

        public async Task<bool> Delete(Guid id, string root)
        {
            var blog = await _context.Blogs.FirstOrDefaultAsync(x => x.Id == id);
            blog.Is_active = false;

            await _repoCore.SaveAll();
            return true;
        }

        public async Task<List<GetBlogViewModel>> GetAll()
        {
            var setting = await _repoSetting.Get();
            var blogs = await _context.Blogs
                .OrderByDescending(x => x.Posted_date)
                .ToListAsync();
            blogs.ForEach(x => x.Posted_date = x.Posted_date.AddHours(setting.Time_difference));
            return _mapper.Map<List<GetBlogViewModel>>(blogs);
        }

        public async Task<PagedList<GetBlogViewModel>> GetAllPagination(int page_number)
        {
            var setting = await _repoSetting.Get();
            var blogs = _context.Blogs
                .OrderByDescending(x => x.Posted_date)
                .Where(x => x.Is_active == true)
                .Select(x => new GetBlogViewModel { 
                    Id = x.Id,
                    Title = x.Title,
                    Image = x.Image,
                    Content = x.Content,
                    Posted_date = x.Posted_date.AddHours((setting == null)? 0: setting.Time_difference),
                    Publisher = x.Publisher
                }).AsQueryable();

            return await PagedList<GetBlogViewModel>.CreateAsync(blogs, page_number, 12);
        }

        public async Task<SaveBlogViewModel> GetUpdatedBlog(Guid id)
        {
            var blog = await _context.Blogs.FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<SaveBlogViewModel>(blog);
        }

        public async Task<string> Save(SaveBlogViewModel model, string publisher, string root, string culture_name)
        {
            if (model.Id == Guid.Empty)
            {
                if (model.File == null)
                    if (culture_name == "en")
                        return "Please upload image!";
                    else
                        return "برجاء رفع صوره!";

                string file_name = null;
                var upload = _repoCore.SaveSingleImage(root, model.File, out file_name);

                if (!upload)
                    if (culture_name == "en")
                        return "Error while uploading!";
                    else
                        return "خطأ اثناء الرفع!";

                if (model.Title.Length < 32)
                    if (culture_name == "en")
                        return "Title must not be less than 32 characters";
                    else
                        return "العنوان لا يجب ان يقل عن 32 حرف";

                if (model.Content.Length < 140)
                    if (culture_name == "en")
                        return "Content must not be less than 140 characters";
                    else
                        return "المحتوى لا يجب ان يقل عن 140 حرف";

                var blog = _mapper.Map<Entitis.Models.Blogs>(model);
                blog.Image = file_name;
                blog.Posted_date = DateTime.Now;
                blog.Publisher = publisher;
                blog.Is_active = true;

                _repoCore.Add(blog);
            }
            else
            {
                string file_name = null;

                if (model.File != null)
                {
                    var full_path = root + "/" + model.Image;
                    if (System.IO.File.Exists(full_path))
                    {
                        System.IO.File.Delete(full_path);
                    }

                    var upload = _repoCore.SaveSingleImage(root, model.File, out file_name);

                    if (!upload)
                        if (culture_name == "en")
                            return "Error while uploading!";
                        else
                            return "خطأ اثناء الرفع!";
                }

                if (model.Title.Length < 32)
                    if (culture_name == "en")
                        return "Title must not be less than 32 characters";
                    else
                        return "العنوان لا يجب ان يقل عن 32 حرف";

                if (model.Content.Length < 140)
                    if (culture_name == "en")
                        return "Content must not be less than 140 characters";
                    else
                        return "المحتوى لا يجب ان يقل عن 140 حرف";

                var blog = await _context.Blogs.FirstOrDefaultAsync(x => x.Id == model.Id);
                var posted_date = blog.Posted_date;
                _mapper.Map(model, blog);

                blog.Image = (model.File == null) ? model.Image : file_name;
                blog.Posted_date = posted_date;
            }

            await _repoCore.SaveAll();
            return string.Empty;
        }

        public async Task<PagedList<GetBlogViewModel>> Search(string key, int page_number)
        {
            var setting = await _repoSetting.Get();
            var blogs = _context.Blogs
                .Where(x => x.Title.Contains(key))
                .Select(x => new GetBlogViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Image = x.Image,
                    Content = x.Content,
                    Posted_date = x.Posted_date.AddHours(setting.Time_difference),
                    Publisher = x.Publisher
                }).AsQueryable();

            return await PagedList<GetBlogViewModel>.CreateAsync(blogs, page_number, 12);
        }
    }
}
