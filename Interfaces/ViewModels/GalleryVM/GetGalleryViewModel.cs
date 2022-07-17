using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModels.GalleryVM
{
    public class GetGalleryViewModel
    {
        public Guid Id { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public bool Is_active { get; set; }

    }
}
