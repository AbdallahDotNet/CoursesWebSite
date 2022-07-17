using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitis.Models
{
    public class Notifications
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public bool Seen { get; set; }
        public bool Is_active { get; set; }
    }
}
