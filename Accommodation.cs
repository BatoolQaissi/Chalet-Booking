using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BookingApp.Models
{
    public class Accommodation
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        [Display(Name = "المستخدم")]
        [JsonIgnore]
        public virtual User User { get; set; }

        [Display(Name = "العروض")]
        [JsonIgnore]
        public virtual List<Offer> Offers { get; set; }

        [Display(Name = "العنوان")]
        public virtual Address Address { get; set; }

        [Display(Name = "قواعد المنزل")]
        public virtual HouseRules HouseRules { get; set; }

        [Display(Name = "الصور")]
        public virtual List<Picture> Pictures { get; set; }

        [Display(Name = "الغرف")]
        public virtual List<Room> Rooms { get; set; }

        [Required(ErrorMessage = "يجب إدخال اسم للسكن")]
        [Display(Name = "الاسم")]
        public String Name { get; set; }

        [Required(ErrorMessage = "يجب اختيار نوع السكن")]
        [Display(Name = "النوع")]
        [RegularExpression("Appartement|Maison|Chambre dans un appartement|Chambre dans une maison", 
            ErrorMessage = "يرجى اختيار نوع سكن صالح")]
        public String Type { get; set; }

        [Required(ErrorMessage = "يجب تحديد الحد الأقصى لعدد المسافرين")]
        [Display(Name = "الحد الأقصى للمسافرين")]
        public int MaxTraveler { get; set; }

        [Required(ErrorMessage = "يجب إدخال وصف للسكن")]
        [Display(Name = "الوصف")]
        public String Description { get; set; }
    }
}
