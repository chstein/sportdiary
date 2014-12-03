using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sporty.Common
{
    public class WeatherCondition
    {
        public string ImageFilepath { get; set; }
        public string SelectValue { get; set; }
        public string Text { get; set; }
        public bool IsSelectedWeatherCondition { get; set; }
    }
}
