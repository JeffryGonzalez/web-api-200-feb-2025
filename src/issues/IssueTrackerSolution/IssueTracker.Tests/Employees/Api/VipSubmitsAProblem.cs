

using IssueTracker.Api.Employees.Api;
using System.Security.Claims;
using IssueTracker.Tests.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using IssueTrackerShared;

namespace IssueTracker.Tests.Employees.Api;

[Collection("VipSubmittingProblems")]
public class VipSubmitsAProblem(VipProblemSubmissionFixture fixture)
{
    [Fact]
    public async Task SubmittedIssuesMustBeForSoftwareInTheCatalog()
    {
        var postModel = new ProblemSubmitModel("Has issues");
        var postResponse = await fixture.Host.Scenario(api =>
        {
            api.WithClaim(new Claim("sub", "byron"));
            api.Post.Json(postModel).ToUrl($"/employee/software/{SeededSoftware.Rider}/problems");
            api.StatusCodeShouldBe(201);

        });

        var modelReturned = postResponse.ReadAsJson<EmployeeProblemReadModel>();
        Assert.NotNull(modelReturned);
        Assert.Equal("Submitted For VIP Treatment", modelReturned.Status);

    }

}


public class VipProblemSubmissionFixture: HostedUnitIntegrationTestFixture
{

    protected override void ConfigureServices(IServiceCollection services)
    {


        
        ////var stubbedVipChecker = Substitute.For<ICheckForVipEmployees>();
        ////stubbedVipChecker.IsEmployeeAVipAsync(Arg.Any<Guid>(), default).Returns(Task.FromResult(true));
        //var stubbedNotificationApi = Substitute.For<INotifyTheVipApiOfAProblem>();
        //stubbedNotificationApi.NotifyOfProblem(Arg.Any<VipIssueCreateModel>()).Returns(Task.FromResult(true));
        ////services.AddScoped<ICheckForVipEmployees>(_ => stubbedVipChecker);
        //services.AddScoped<INotifyTheVipApiOfAProblem>(_ => stubbedNotificationApi);

    }

    protected override void ConfigureTestServices(IServiceCollection services)
    {
      
    }
}

[CollectionDefinition("VipSubmittingProblems")]
public class VipSubmittingProblemsCollection : ICollectionFixture<VipProblemSubmissionFixture>
{

}