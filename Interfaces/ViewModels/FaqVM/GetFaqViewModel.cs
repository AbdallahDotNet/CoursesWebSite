using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModels.FaqVM
{
    public class GetFaqViewModel
    {
        public Guid Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public bool Is_active { get; set; }

    }
}
