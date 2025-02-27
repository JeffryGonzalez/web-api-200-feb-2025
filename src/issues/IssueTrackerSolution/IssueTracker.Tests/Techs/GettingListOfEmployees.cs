

using System.Security.Claims;
using IssueTracker.Api.Middleware;
using IssueTracker.Tests.Employees.Api;
using IssueTracker.Tests.Fixtures;

namespace IssueTracker.Tests.Techs;

[Collection("SeededApiCollection")] // fix this.
[Trait("Category", "UnitIntegration")]
public class GettingListOfEmployees(SeededWithEmployeesApiFixture fixture)
{
    [Fact]
    public async Task CanGetListOfEmployees()
    {

      
        var getResponse = await fixture.Host.Scenario(api =>
        {
            api.WithClaim(new Claim("sub", "roeder"));
            api.WithClaim(new Claim(ClaimTypes.Role, "help-desk"));
            api.Get.Url("/help-desk-staff/employees");
        });

        var getBody = getResponse.ReadAsJson<IReadOnlyList<Employee>>();

        Assert.NotNull(getBody);

        Assert.Equal(2, getBody.Count());
    }

}

public class SeededWithEmployeesApiFixture : HostedUnitIntegrationTestFixture
{

    protected override async Task BeforeAsync()
    {
        using var session = Store.LightweightSession();

        var employeeCreated1 = new EmployeeCreated(Guid.NewGuid(), "carl");
        var employeeCreated2 = new EmployeeCreated(Guid.NewGuid(), "brunhilde");
        session.Events.StartStream<Employee>(employeeCreated1.Id, employeeCreated1);
        session.Events.StartStream<Employee>(employeeCreated2.Id, employeeCreated2);
        await session.SaveChangesAsync();
    }
}
[CollectionDefinition("SeededApiCollection")]
public class SeededWithEmployeesCollection : ICollectionFixture<SeededWithEmployeesApiFixture> { }