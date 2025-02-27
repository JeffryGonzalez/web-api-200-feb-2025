using IssueTracker.Api.Employees.Api;

namespace IssueTracker.Api.Employees.Services;

public class EveryoneIsAVip : ICheckForVipEmployees
{
    public Task<bool> IsEmployeeAVipAsync(Guid Id, CancellationToken token)
    {
        return Task.FromResult(true);
    }
}
