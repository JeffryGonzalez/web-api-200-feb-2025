using IssueTracker.Api.Employees.Api;
using IssueTracker.Api.Middleware;
using IssueTracker.Tests.Fixtures;

namespace IssueTracker.Tests.Employees.Domain;

[Trait("Category", "UnitIntegration")] // this is metadata so we can run just some of these at a time.
[Collection("UnitIntegration")] // everything here should use the same database as everything else in this "collection"
public class EmployeeSubmitsAnIssue(UnitIntegrationTestFixture fixture) : IAsyncLifetime
{
    private readonly string _newSub = "carl";
    private readonly Guid _employeeId = Guid.NewGuid();
    private readonly Guid _problemId = Guid.NewGuid();
    private readonly Guid _softwareId = Guid.Parse(SeededSoftware.DockerDesktop);
    private readonly string _description = "It doesn't work!";
    private Employee _userReadModel = null!;

    public async Task InitializeAsync()
    {
        await using var session = fixture.Store.LightweightSession();

        var user = new EmployeeCreated(_employeeId, _newSub);

        session.Events.StartStream(user.Id, user);
        var problem = new EmployeeSubmittedAProblem(_problemId, _employeeId, _softwareId, _description);
        await session.SaveChangesAsync();
        
        _userReadModel = (await session.Events.AggregateStreamAsync<Employee>(user.Id))!;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

}
