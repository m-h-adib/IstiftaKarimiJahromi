using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class PreProccess:BaseEntity<long>
{    
    public string? Description { get; set; }


    #region ctors
    public long? QuestionCycleId { get; set; }
    public QuestionCycle? QuestionCycle { get; set; }
    #endregion
}
