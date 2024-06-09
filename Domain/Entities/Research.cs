using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Research:BaseEntity<long>
{
    public string? AnswerText { get; set; }
    public string? AnswerFile { get; set; }
    public string? ReasonText { get; set; }
    public string? ReasonFile { get; set; }
    public bool? IsAccept { get; set; }
    public string? Description { get; set; }

    #region navigations
    public long QuestionCycleId { get; set; }
    public QuestionCycle? QuestionCycle { get; set; }
    public ICollection<Session>? Sessions { get; set; }
    #endregion
}
