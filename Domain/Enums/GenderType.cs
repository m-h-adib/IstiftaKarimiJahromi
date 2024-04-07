using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums;

public enum GenderType
{
	[Display(Name ="آقا")]
	Man=1,
	[Display(Name = "خانم")]
	Woman =2
}
