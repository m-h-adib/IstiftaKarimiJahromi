using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Editor:BaseEntity<long>
{
    public string? AnswerText { get; set; }
    public string? ReasonText { get; set; }


    #region navigations
    public long? LanguageId { get; set; }
    public Language? Language { get; set; }
    public long? QuestionCycleId { get; set; }
    public QuestionCycle? QuestionCycle { get; set; }
    #endregion
}
