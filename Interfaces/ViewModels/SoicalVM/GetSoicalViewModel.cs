﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModels.SoicalVM
{
    public class GetSoicalViewModel
    {
        public Guid Id { get; set; }
        public string Linkedin { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
    }
}
