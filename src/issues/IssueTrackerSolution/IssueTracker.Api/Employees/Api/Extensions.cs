using IssueTracker.Api.Employees.Services;
using IssueTracker.Api.Middleware;

namespace IssueTracker.Api.Employees.Api;

public static class Extensions
{
    public static IEndpointRouteBuilder MapEmployees(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("employee")
            .WithTags("Employees")
            .WithDescription("Employee Related Stuff")
            .RequireAuthorization();

        group.MapPost("/software/{softwareId:guid}/problems", SubmittingAProblem.SubmitAsync);
           
        
        // TODO: Should this have the Endpoint Filter?
        group.MapGet("/software/{softwareId:guid}/problems", GettingProblems.GetAllProblemsAsync)
            .AddEndpointFilter<ReturnNotFoundIfNoUserFilter>();
        group.MapGet("/software/{softwareId:guid}/problems/{problemId:guid}", GettingProblems.GetProblemAsync);
        
        group.MapDelete("/software/{softwareId:guid}/problems/{problemId:guid}", CancellingAProblem.CancelAProblemAsync);

        return routes;
    }
}
