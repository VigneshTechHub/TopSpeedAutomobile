using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSpeed.Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using SelectListItem = Microsoft.AspNetCore.Mvc.Rendering.SelectListItem;

namespace TopSpeed.Domain.ViewModel
{
    public class HomePostVM
    {

        
        public List<PostModel>Posts { get; set; }

        public string? searchBox{ get; set; } = string.Empty;

        public Guid? BrandId { get; set; }

        public Guid? VehicleTypeId{ get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> BrandList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> VehicleTypeList { get; set; }

       


    }
}
