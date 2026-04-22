using Domain.Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class User:BaseEntity<long>
{
    public string? Name { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public RoleType? Role { get; set; }
    public string? EitaaId { get; set; }

    #region navigations
    public ICollection<Question>? Questions { get; set; }
    public ICollection<QuestionCycle>? QuestionCycles { get; set; }
    public ICollection<QuestionCycle>? QuestionCyclesFromUsers { get; set; }
    public ICollection<QuestionCycle>? QuestionCyclesToUsers { get; set; }
    #endregion
}
