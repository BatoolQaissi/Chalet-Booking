using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApp.Models
{
    public class HouseRules
    {
        [ForeignKey("Accommodation")]
        public Guid Id { get; set; }

        public virtual Accommodation Accommodation { get; set; }

        [Display(Name = "الوقت الأدنى للوصول")]
        [Required(ErrorMessage = "يجب عليك تحديد الوقت الأدنى للوصول")]
        [DataType(DataType.Time)]
        public TimeSpan ArrivalHour { get; set; }

        [Display(Name = "الوقت الأقصى للمغادرة")]
        [Required(ErrorMessage = "يجب عليك تحديد الوقت الأقصى للمغادرة")]
        [DataType(DataType.Time)]
        public TimeSpan DepartureHour { get; set; }

        [Display(Name = "يسمح بالحيوانات الأليفة")]
        public bool PetAllowed { get; set; }

        [Display(Name = "يسمح بالحفلات")]
        public bool PartyAllowed { get; set; }

        [Display(Name = "السكن يسمح بالتدخين")]
        public bool SmokeAllowed { get; set; }

        public override string ToString()
        {
            return "الحيوانات: " + (PetAllowed ? "نعم" : "لا") + " -- الحفلات: " + (PartyAllowed ? "نعم" : "لا") + " -- التدخين: " + (SmokeAllowed ? "نعم" : "لا")
                + " || وقت الوصول: " + ArrivalHour.ToString("hh\\hmm") + " -- وقت المغادرة: " + DepartureHour.ToString("hh\\hmm");
        }
    }
}
