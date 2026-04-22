using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Session:BaseEntity<long>
{
    public string? Title { get; set; }
    public DateTime? Date { get; set; }
    public string? Time { get; set; }
    public string? Description { get; set; }

    #region navigations
    public long? ResearchId { get; set; }
    public Research? Research { get; set; }
    public ICollection<SessionUser>? SessionUsers { get; set; }
    public long? UserId { get; set; }
    public User? User { get; set; }
    #endregion
}
