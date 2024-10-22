using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums;

public enum UserQueryTypeEnum
{
    [Display(Name ="سوالات ارجاع شده")]
    ReferenceQuestions=1,
    [Display(Name = "سوالات انجام شده توسط کاربر")]
    DoneQuestions =2,
    [Display(Name = "تمام سوالات")]
    AllQuestions =3
}
