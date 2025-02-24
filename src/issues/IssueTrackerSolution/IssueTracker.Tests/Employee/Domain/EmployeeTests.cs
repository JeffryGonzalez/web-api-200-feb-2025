
using IssueTracker.Api.Employee.Domain;
using IssueTracker.Tests.Fixtures;
using Marten;

namespace IssueTracker.Tests.Employee.Domain;

[Trait("Category", "UnitIntegration")] // this is metadata so we can run just some of these at a time.
[Collection("UnitIntegration")] // everything here should use the same database as everything else in this "collection"
public class EmployeeTests(UnitIntegrationTestFixture fixture)
{
    [Fact]

    public void CanCreateAnEmployee()
    {
        IDocumentSession session = fixture.Store.LightweightSession(); 


        var repository = new EmployeeRepository(session); // a thing that handles persistence.
        var sub = "bob@company";
        var emp = repository.Create(sub);

        




    }

    [Fact]
    public void EmployeeCanSubmitProblems()
    {

    }
}
