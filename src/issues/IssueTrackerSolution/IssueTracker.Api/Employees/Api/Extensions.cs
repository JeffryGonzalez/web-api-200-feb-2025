using IssueTracker.Api.Employees.Services;
using IssueTracker.Api.Middleware;

namespace IssueTracker.Api.Employees.Api;

public static class Extensions
{
    public static IEndpointRouteBuilder MapEmployees(this IEndpointRouteBuilder routes)
    {
        var employeeGroup = routes.MapGroup("employee")
            .WithTags("Employees")
            .WithDescription("Employee Related Stuff")
            .RequireAuthorization(config =>
            {
                config.RequireClaim("sub");
            })
            
            .AddEndpointFilter<AuthenticatedUserToEmployeeMiddleware>();

        employeeGroup.MapPost("/software/{softwareId:guid}/problems", SubmittingAProblem.SubmitAsync);
           
        
        // TODO: Should this have the Endpoint Filter?
        employeeGroup.MapGet("/software/{softwareId:guid}/problems", GettingProblems.GetAllProblemsAsync)
            .AddEndpointFilter<ReturnNotFoundIfNoUserFilter>();
        employeeGroup.MapGet("/software/{softwareId:guid}/problems/{problemId:guid}", GettingProblems.GetProblemAsync);
        
        employeeGroup.MapDelete("/software/{softwareId:guid}/problems/{problemId:guid}", CancellingAProblem.CancelAProblemAsync);

        return routes;
    }
}
