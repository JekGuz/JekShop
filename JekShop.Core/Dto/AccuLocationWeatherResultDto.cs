using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JekShop.Core.Dto
{
    public class AccuLocationWeatherResultDto
    {
        public string? CityName { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Text { get; set; }
        public double? TemoMectricValueUnit {get; set; }
    }
}
