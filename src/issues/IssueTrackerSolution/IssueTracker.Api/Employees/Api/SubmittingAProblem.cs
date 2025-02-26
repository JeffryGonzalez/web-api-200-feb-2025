using IssueTracker.Api.Employees.Domain;
using Marten;
using Microsoft.AspNetCore.Http.HttpResults;

namespace IssueTracker.Api.Employees.Api;

public static class SubmittingAProblem
{
    public static async  Task<Ok<EmployeeProblem>> SubmitAsync(
        ProblemSubmitModel request,
        Guid softwareId,
        IProvideTheEmployeeId employeeIdProvider,
        IDocumentSession session,
    CancellationToken token,
    HttpContext context
    )
    {
        var employeeId = await employeeIdProvider.GetEmployeeIdAsync(token);
        var problemId = Guid.NewGuid();
        var employeeProblem = new EmployeeSubmittedAProblem(problemId, employeeId, softwareId, request.Description);
        
        session.Events.StartStream(problemId, employeeProblem);
        await session.SaveChangesAsync(token);

        var response = await session.LoadAsync<EmployeeProblem>(problemId, token);
        return TypedResults.Ok(response);
    }
}


public record ProblemSubmitModel(string Description);

public record EmployeeSubmittedAProblem(Guid ProblemId, Guid EmployeeId, Guid SoftwareId, string Description);

public interface IProvideTheEmployeeId
{
    public Task<Guid> GetEmployeeIdAsync(CancellationToken token = default);
}