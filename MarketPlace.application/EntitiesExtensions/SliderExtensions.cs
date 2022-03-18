using MarketPlace.application.Utils;
using MarketPlace.dataLayer.Entities.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.application.EntitiesExtensions
{
  public static  class SliderExtensions
    {
        public static string GetSliderImage(this Slider slider)
        {
            return PathExtension.SliderOrigin+slider.ImageName;
        }
    }
}
