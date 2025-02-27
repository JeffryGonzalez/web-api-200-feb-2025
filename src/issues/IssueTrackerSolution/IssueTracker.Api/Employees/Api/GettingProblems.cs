using IssueTracker.Api.Employees.Services;
using Marten;
using Microsoft.AspNetCore.Http.HttpResults;

namespace IssueTracker.Api.Employees.Api;

public static class GettingProblems
{
    public static async Task<Ok<IReadOnlyList<EmployeeProblemReadModel>>> GetAllProblemsAsync(
        IProvideTheEmployeeId employeeIdProvider,
        IDocumentSession session,
        Guid softwareId,
        CancellationToken token
    )
    {
        var employeeId = await employeeIdProvider.GetEmployeeIdAsync(token);
        var problems = await session.Query<EmployeeProblemReadModel>().Where(x => x.EmployeeId == employeeId && x.SoftwareId == softwareId).ToListAsync(token);
        return TypedResults.Ok(problems);
    }
    
    public static async Task<Results<Ok<EmployeeProblemReadModel>, NotFound>> GetProblemAsync(
        IProvideTheEmployeeId employeeIdProvider,
        IDocumentSession session,
        Guid softwareId,
        Guid problemId,
        CancellationToken token
    )
    {
        var employeeId = await employeeIdProvider.GetEmployeeIdAsync(token);
        var problem = await session.Query<EmployeeProblemReadModel>().SingleOrDefaultAsync(x => x.EmployeeId == employeeId && x.SoftwareId == softwareId && x.Id == problemId, token);
        if (problem == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(problem);
    }
}