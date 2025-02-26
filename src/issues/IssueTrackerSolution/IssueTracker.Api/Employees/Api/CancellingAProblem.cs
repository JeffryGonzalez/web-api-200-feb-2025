using Marten;
using Microsoft.AspNetCore.Http.HttpResults;

namespace IssueTracker.Api.Employees.Api;

public static class CancellingAProblem
{
    public static async Task<Results<NoContent, Conflict<string>>> CancelAProblemAsync(
        Guid problemId,
        IDocumentSession session,
        CancellationToken token
        )
    {
        // TODO: Should we verify the identity and the softwareId and all that?
        var problem = await session.LoadAsync<EmployeeProblem>(problemId, token);
        if (problem is null)
        {
            return TypedResults.NoContent();
        }
        if( problem.Status != "Submitted")
        {
            return TypedResults.Conflict("Problem cannot be cancelled now.");
        }

        session.Events.Append(problemId, new EmployeeCancelledAProblem());
        await session.SaveChangesAsync(token);
        return TypedResults.NoContent();

    }
}

public record EmployeeCancelledAProblem();