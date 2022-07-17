using Entitis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModels.CommentVM
{
    public class GetCommentViewModel
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Student_name { get; set; }
        public string Content { get; set; }
        public bool Approval { get; set; }
        public bool Is_active { get; set; }

        public Course_specialty Course_specialty { get; set; }
    }
}
