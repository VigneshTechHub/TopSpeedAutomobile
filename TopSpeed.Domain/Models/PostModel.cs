using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSpeed.Domain.ApplicationEnums;
using TopSpeed.Domain.Common;

namespace TopSpeed.Domain.Models
{
    public class PostModel:BaseModel
    {

            [Display(Name = "Brand")]
            public Guid BrandId { get; set; }

            [ValidateNever]
            [ForeignKey("BrandId")]
            public BrandModel Brand { get; set; }

            [Display(Name = "Vehicle Type")]
            public Guid VehicleTypeId { get; set; }

            [ValidateNever]
            [ForeignKey("VehicleTypeId")]
            public VehicleTypeModel VehicleType { get; set; }

            public string Name { get; set; }

            [Display(Name = "Select Engine / Fuel Type ")]
            public EngineAndFuelType EngineAndFuelType { get; set; }

            [Display(Name = "Select Transmisson Mode")]
            public Transmission Transmission { get; set; }

            public int Engine { get; set; }

            public int TopSpeed { get; set; }

            public int Milage { get; set; }

            public int Range { get; set; }

            [Display(Name = "Seating Capacity")]
            public string SeatingCapacity { get; set; }

            [Display(Name = "Base Price")]
            public double PriceFrom { get; set; }

            [Display(Name = "Top End Price")]
            public double PriceTo { get; set; }

            [Range(1, 5, ErrorMessage = "Rating should be 1 to 5 only")]
            public int Rating { get; set; }

            [Display(Name = "Update Vehicle Image")]
            public string VehicleImage { get; set; }
        }
}

