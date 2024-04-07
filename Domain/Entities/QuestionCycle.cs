using Domain.Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
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
    public long? FromUserId { get; set; }
    public User? FromUser { get; set; }
    public long? ToUserId { get; set; }
    public User? ToUser { get; set; }
    public ICollection<Proccess>? Proccesses { get; set; }
    public ICollection<Research>? Researches { get; set; }
    public ICollection<Editor>? Editors { get; set; }
    public long? UserId { get; set; }
    public User? User { get; set; }
    #endregion
}
