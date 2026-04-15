// تقييم الأداء والتدريب
using HKDR.DomainEntities.Entities.HR;
using System.ComponentModel.DataAnnotations;

public class Performance
{
    [Key]
    public int Id { get; set; }

    public int EmployeeId { get; set; }
    public Employee Employee { get; set; }

    public DateTime EvaluationDate { get; set; }
    public string KPI { get; set; }
    public int Score { get; set; } // 0-100
    public string Comments { get; set; }
    public string TrainingPlan { get; set; }
}