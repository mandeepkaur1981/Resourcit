using Microsoft.AspNetCore.Http;
using ResourcitModels.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResourcitModels.ViewModels
{
    public class ExtendResourceInfo
    {
        public Resource resource { get; set; }
        public IFormFile ProfilePhoto { get; set; }
    }
}
