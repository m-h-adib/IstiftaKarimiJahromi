using Domain.Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Question:BaseEntity<long>
{
    public string? Title { get; set; }
    public DateTime? QuestionDate { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public GenderType? Gender { get; set; }
    public int? Age { get; set; }
    public string? City { get; set; }
    public EducationType? Education { get; set; }
    public string? Job { get; set; }
    public string? Mobile { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? QuestionText { get; set; }
    public Guid? QuestionCode { get; set; }
    public QuestionStepType? Step { get; set; }



    #region navigations
    public long? CountryId { get; set; }
    public Country? Country { get; set; }
    public long? ProvinceId { get; set; }
    public Province? Province { get; set; }
    public long? JobCategoryId { get; set; }
    public JobCategory? JobCategory { get; set; }
    public long? MethodOfReceiveQuestionId { get; set; }
    public MethodOfReceiveQuestion? MethodOfReceiveQuestion { get; set; }
    public long? QuestionCategoryId { get; set; }
    public QuestionCategory? QuestionCategory { get; set; }
    public long? UserId { get; set; }
    public User? User { get; set; }
    public ICollection<QuestionAttach>? QuestionAttaches { get; set; }
    #endregion
}
