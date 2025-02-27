
using IssueTracker.Api.Employees.Services;
using IssueTrackerShared;
using Marten;
using Microsoft.AspNetCore.Http.HttpResults;

namespace IssueTracker.Api.Employees.Api;

public static class SubmittingAProblem
{
    public static async Task<Created<EmployeeProblemReadModel>> SubmitAsync(
        ProblemSubmitModel request,
        Guid softwareId,
        IProvideTheEmployeeId employeeIdProvider,
        IDocumentSession session,
        ICheckForVipEmployees vipChecker,
        INotifyTheVipApiOfAProblem vipNotifier,
    CancellationToken token,
    HttpContext context
    )
    {

        var employeeId = await employeeIdProvider.GetEmployeeIdAsync(token);
        var problemId = Guid.NewGuid();
        var employeeProblem = new EmployeeSubmittedAProblem(problemId, employeeId, softwareId, request.Description);

        session.Events.StartStream(problemId, employeeProblem);
        await session.SaveChangesAsync(token);
        // if this person is a VIP then call the other API, and on success, change the status to "Submitted For VIP Treatment"
        if(await vipChecker.IsEmployeeAVipAsync(employeeId, token))
        {
            var notificationProblem = new VipIssueCreateModel($"/vipapi/problems/{problemId}", employeeProblem.Description);
            var notificationHappened = await vipNotifier.NotifyOfProblem(notificationProblem);
            if (notificationHappened)
            {
                session.Events.Append(problemId, new ProblemSubmittedtoVipIssues());
                // this is optional, but.
                await session.SaveChangesAsync();
            } // TODO: ????
        }
        var response = await session.LoadAsync<EmployeeProblemReadModel>(problemId, token);
        // var response = await session.Events.AggregateStreamAsync<EmployeeProblemReadModel>(problemId);
        return TypedResults.Created($"/employee/software/{response!.SoftwareId}/problems/{response.Id}", response);
    }
}

public interface ICheckForVipEmployees
{
    Task<bool> IsEmployeeAVipAsync(Guid Id, CancellationToken token);
}


public interface INotifyTheVipApiOfAProblem
{
    Task<bool> NotifyOfProblem(VipIssueCreateModel issue);
}
public record ProblemSubmittedtoVipIssues();

public record ProblemSubmitModel(string Description);

public record EmployeeSubmittedAProblem(Guid ProblemId, Guid EmployeeId, Guid SoftwareId, string Description);