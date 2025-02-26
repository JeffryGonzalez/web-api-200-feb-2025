namespace IssueTracker.Api.Employees.Api;

public record EmployeeProblem
{
 
    public Guid Id { get; set; }

    public Guid EmployeeId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public Guid SoftwareId { get; set; }
    public DateTimeOffset Opened { get; set; }


    public static EmployeeProblem Create(EmployeeSubmittedAProblem problem)
    {
        return new EmployeeProblem
        {
            Id = problem.ProblemId,
            Description = problem.Description,
            EmployeeId = problem.EmployeeId,
            Opened = DateTimeOffset.UtcNow,
            SoftwareId = problem.SoftwareId,
            Status = "Submitted"
        };
    }
    
    public static bool ShouldDelete(EmployeeCancelledAProblem _) => true;

    
    
}