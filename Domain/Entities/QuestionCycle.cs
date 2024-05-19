using Domain.Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class QuestionCycle:BaseEntity<long>
{
    public QuestionStepType? FromStep { get; set; }
    public QuestionStepType? ToStep { get; set; }
    public bool? IsAccept { get; set; }
    public string? Description { get; set; }


    #region navigations
    /// <summary>
    /// یوزر انجام دهنده فعالیت قبل
    /// </summary>
    public long? FromUserId { get; set; }
	[NotMapped]
	public User? FromUser { get; set; }
    /// <summary>
    /// یوزری که باید فعالیت جدید را انجام دهد
    /// </summary>
	public long? ToUserId { get; set; }
	[NotMapped]
	public User? ToUser { get; set; }
    public long QuestionId { get; set; }
    public Question? Question { get; set; }
	public ICollection<PreProccess>? PreProccesses { get; set; }
	public ICollection<Research>? Researches { get; set; }
    public ICollection<Editor>? Editors { get; set; }
    /// <summary>
    /// یوزر ارجاع دهنده
    /// </summary>
    public long? UserId { get; set; }
    public User? User { get; set; }
    #endregion
}
