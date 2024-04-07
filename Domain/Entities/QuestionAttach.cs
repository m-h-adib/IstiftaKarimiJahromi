using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class QuestionAttach:BaseEntity<long>
{
    public string? File { get; set; }

    #region navigations
    public long? QuestionId { get; set; }
    public Question? Question { get; set; }
    #endregion
}
