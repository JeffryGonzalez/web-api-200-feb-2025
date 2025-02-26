using Marten;
using Microsoft.AspNetCore.Http.HttpResults;

namespace IssueTracker.Api.Employees.Api;

public static class GettingProblems
{
    public static async Task<Ok<IReadOnlyList<EmployeeProblem>>> GetAllProblemsAsync(
        IProvideTheEmployeeId employeeIdProvider,
        IDocumentSession session,
        Guid softwareId,
        CancellationToken token
    )
    {
        var employeeId = await employeeIdProvider.GetEmployeeIdAsync(token);
        var problems = await session.Query<EmployeeProblem>().Where(x => x.EmployeeId == employeeId && x.SoftwareId == softwareId).ToListAsync(token);
        return TypedResults.Ok(problems);
    }
    
    public static async Task<Results<Ok<EmployeeProblem>, NotFound>> GetProblemAsync(
        IProvideTheEmployeeId employeeIdProvider,
        IDocumentSession session,
        Guid softwareId,
        Guid problemId,
        CancellationToken token
    )
    {
        var employeeId = await employeeIdProvider.GetEmployeeIdAsync(token);
        var problem = await session.Query<EmployeeProblem>().SingleOrDefaultAsync(x => x.EmployeeId == employeeId && x.SoftwareId == softwareId && x.Id == problemId, token);
        if (problem == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(problem);
    }
}