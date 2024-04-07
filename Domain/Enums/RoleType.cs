using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums;

public enum RoleType
{
	[Display(Name ="سوپر ادمین")]
	SuperAdmin=1,
	[Display(Name ="ادمین")]
	Admin=2,
	[Display(Name ="محقق")]
	Researcher=3,
	[Display(Name = "ویراستار")]
	Editor=4
}
