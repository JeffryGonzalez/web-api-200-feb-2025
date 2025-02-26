

using System.Security.Claims;
using IssueTracker.Api.Employees.Domain;
using IssueTracker.Api.Middleware;
using Marten;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;

namespace IssueTracker.Api.Employees.Api;

public static class SubmittingAProblem
{
    public static async  Task<Ok<EmployeeProblem>> SubmitAsync(
        ProblemSubmitModel request,
        Guid softwareId,
      //  IProvideTheEmployeeId employeeIdProvider,
        IDocumentSession session,
    CancellationToken token,
    HttpContext context
    )
    {
        var sub = context.User.FindFirstValue("sub");
        var employee = await session.Query<AuthenticatedUser>().Where(u => u.Sub == sub).SingleOrDefaultAsync();
        

        var problemId = Guid.NewGuid();
        var employeeProblem = new EmployeeSubmittedAProblem(problemId, employee.Id, softwareId, request.Description);
        var problemSubmited = new ProblemSubmitted(problemId, softwareId, employee.Id, request.Description, DateTimeOffset.UtcNow);

        session.Events.StartStream(problemId, problemSubmited); // write it to the table of problems.
        session.Events.Append(employee.Id, employeeProblem);
        await session.SaveChangesAsync();

        var response = await session.Events.AggregateStreamAsync<EmployeeProblem>(problemId);
        return TypedResults.Ok(response);
    }
}


public record ProblemSubmitModel(string Description);

public record EmployeeSubmittedAProblem(Guid ProblemId, Guid EmployeeId, Guid SoftwareId, string Description);

public interface IProcessCommandsForTheCurrentEmployee
{
 
    Task<ProblemSubmitted> ProcessProblemAsync(SubmitProblem problem);
}

public interface IProvideTheEmployeeId
{
    public Task<Guid> GetEmployeeIdAsync(CancellationToken token = default);
}


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
}