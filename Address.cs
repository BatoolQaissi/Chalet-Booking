using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace BookingApp.Models
{
    public class Address
    {
		[ForeignKey("Accommodation")]
		public Guid Id { get; set; }

		[JsonIgnore] 
		public virtual Accommodation Accommodation { get; set; }

		[Required(ErrorMessage = "يجب عليك إدخال رقم الشارع واسم الشارع الخاص بمسكنك")]
		[Display(Name = "رقم الشارع واسم الشارع")]
		public String StreetAndNumber { get; set; }

		[Display(Name = "تكملة العنوان")]
		public String Complement { get; set; }

		[Required(ErrorMessage = "يجب عليك إدخال الرمز البريدي الخاص بمسكنك")]
		[Display(Name = "الرمز البريدي")]
		public String PostalCode { get; set; }

		[Required(ErrorMessage = "يجب عليك إدخال المدينة الخاصة بمسكنك")]
		[Display(Name = "المدينة")]
		public String City { get; set; }

		[Required(ErrorMessage = "يجب عليك إدخال البلد الخاص بمسكنك")]
		[Display(Name = "البلد")]
		public String Country { get; set; }

		public override String ToString()
        {
			return StreetAndNumber + ", " + Complement + "\n" + PostalCode + " " + City + ", " + Country;
        }

		public String ShortAddress()
        {
			return City + ", " + Country;
		}
	}
}
