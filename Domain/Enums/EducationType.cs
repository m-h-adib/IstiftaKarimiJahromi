using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums;

public enum EducationType
{
	[Display(Name ="بی سواد")]
	No=1,
	[Display(Name = "ابتدایی")]
	Ebtedayi = 2,
	[Display(Name = "سیکل")]
	Sikl = 3,
	[Display(Name = "دیپلم")]
	Diplom = 4,
	[Display(Name = "فوق دیپلم")]
	FogheDiplom = 5,
	[Display(Name = "کارشناسی")]
	Lisans = 6,
	[Display(Name = "کارشناسی ارشد")]
	FogheLisans = 7,
	[Display(Name = "دکتری")]
	Doctora = 8
}
