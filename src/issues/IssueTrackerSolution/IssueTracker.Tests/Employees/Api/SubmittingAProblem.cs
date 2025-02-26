﻿

using System.Security.Claims;
using Alba;
using Alba.Security;
using IssueTracker.Api.Employees.Api;
using IssueTracker.Api.Employees.Domain;
using IssueTracker.Tests.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;


namespace IssueTracker.Tests.Employees.Api;
[Trait("Category", "UnitIntegration")]
[Collection("EmployeeApiCollection")]
public class SubmittingAProblem(EmployeeHostedIntegrationTest fixture)
{
    [Fact]
    public async Task Trial()
    {

        // the employee identity (that has to be in the authorization header as a JWT)
        // the software id (that is in the URL - route parameter, softwareId
        // the description of the problem.

        var problem = new ProblemSubmitModel("Thing is broke real bad");
        var response = await fixture.Host.Scenario(api =>
        {
            api
                .WithClaim(new Claim("sub", "bob@company.com"));
                api.Post
            .Json(problem)
            .ToUrl($"/employee/software/{SeededSoftware.DockerDesktop}/problems");
        });

       // we should probably verify something here, right?


    }

    [Fact]
    public async Task SoftwareNotIntheCatalogReturnsFourOhFour()
    {
        var problem = new ProblemSubmitModel("Thing is broke real bad");
        
        var response = await fixture.Host.Scenario(api =>
        {
        
            api.Post
                .Json(problem)
                .ToUrl($"/employee/software/{SeededSoftware.NotPresentInCatalog}/problems");
            api.StatusCodeShouldBe(404);
            
        });
    }

    [Fact]
    public async Task EmployeeIsCreatedForNewSub()
    {
        var sub = "carlo@gmail.com";
        var session = fixture.Store!.LightweightSession();
        var repository = new EmployeeRepository(session);
        var nonUser = await repository.GetBySubAsync(sub);
        Assert.Null(nonUser);
        var problem = new ProblemSubmitModel("Thing is broke real bad");

        await fixture.Host.Scenario(api =>
        {
            
            api.WithClaim(new Claim("sub", sub));
            api.Post
                .Json(problem)
                .ToUrl($"/employee/software/{SeededSoftware.DockerDesktop}/problems");
            api.StatusCodeShouldBeSuccess();
        });

        var savedUser = await repository.GetBySubAsync(sub);

        Assert.NotNull(savedUser);

        // We have to do something here.
    }
    

    

}



public class EmployeeHostedIntegrationTest : HostedUnitIntegrationTestFixture
{
    protected override AuthenticationStub GetAuthenticationStub()
    {
        return base.GetAuthenticationStub();
        // return new AuthenticationStub().WithName("bob@company.com");
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
       // for things that don't exist yet.
    }

    protected override void ConfigureTestServices(IServiceCollection services)
    {
        // for things that do exist, but you want to replace in your test with another thing.

        base.ConfigureTestServices(services);
    }
}

[CollectionDefinition("EmployeeApiCollection")]
public class EmployeeApiFixture: ICollectionFixture<EmployeeHostedIntegrationTest> { }