namespace IssueTracker.Api.Employees.Api;

public interface IProvideTheEmployeeId
{
    public Task<Guid> GetEmployeeIdAsync(CancellationToken token = default);
}