using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums;

public enum QuestionStepType
{
	[Display(Name ="ثبت سوال")]
	Add=1,
	[Display(Name = "پیش پردازش سوال")]
	Proccess =2,
	[Display(Name = "در حال تحقیق")]
	Research =3,
	[Display(Name = "ارجاع به جلسه مشورتی")]
	Session =4,
	[Display(Name = "در حال ویراستاری")]
	Editor =5,
	[Display(Name = "آرشیو شده")]
	Archive=6,
	[Display(Name ="سوال تکراری")]
	Duplicate=7
}
