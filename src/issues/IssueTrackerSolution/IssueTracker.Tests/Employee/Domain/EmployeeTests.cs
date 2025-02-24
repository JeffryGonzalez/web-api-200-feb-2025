
using System.Threading.Tasks;
using IssueTracker.Api.Employee.Domain;
using IssueTracker.Tests.Fixtures;
using Marten;
using Microsoft.Extensions.DependencyInjection;

namespace IssueTracker.Tests.Employee.Domain;

[Trait("Category", "UnitIntegration")] // this is metadata so we can run just some of these at a time.
[Collection("UnitIntegration")] // everything here should use the same database as everything else in this "collection"
public class EmployeeTests(UnitIntegrationTestFixture fixture)
{
    [Fact]

    public async Task CanCreateAnEmployee()
    {
        IDocumentSession session = fixture.Store.LightweightSession(); 


        var repository = new EmployeeRepository(session); // a thing that handles persistence.
        var sub = "bob@company";
        var emp = repository.Create(sub);

        // I want to save it to the database (we do this through the aggregate)
        await emp.SaveAsync();
        // and make sure it got saved.

        var emp2 = await repository.GetByIdAsync(emp.Id);

        Assert.NotNull(emp2);
        Assert.Equal(emp.Id, emp2.Id);

        var emp3 = await repository.GetBySubAsync(sub);
        Assert.NotNull(emp3);
        // Assert.Equal(/// No sub ?? We should probably check that)



    }

    [Fact]
    public void EmployeeCanSubmitProblems()
    {

    }
}
