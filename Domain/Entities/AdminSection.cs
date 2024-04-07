using Domain.Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class AdminSection:BaseEntity<long>
{
	public string? Name { get; set; }
	public string? Title { get; set; }
    public bool? ShowInMenu { get; set; }
    public string? Link { get; set; }
	public string? QueryParams { get; set; }
    public RoleType? Role { get; set; }
	public bool? IsPublic { get; set; } = false;
    public int? Order { get; set; }

    #region navigations
    public int? ParentId { get; set; }
	public AdminSection? Parent { get; set; }
	public ICollection<AdminSectionAccess>? AdminSectionAccesses { get; set; }
	#endregion
}
