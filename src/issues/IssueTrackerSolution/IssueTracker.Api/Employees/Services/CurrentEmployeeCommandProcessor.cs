using System.Security.Claims;
using IssueTracker.Api.Employees.Api;
using IssueTracker.Api.Employees.Domain;
using IssueTracker.Api.Middleware;


namespace IssueTracker.Api.Employees.Services;

public class CurrentEmployeeCommandProcessor(IHttpContextAccessor context,
    EmployeeRepository repository) : IProcessCommandsForTheCurrentEmployee
{
    public async Task<ProblemSubmitted> ProcessProblemAsync(SubmitProblem problem)
    {

        //var result = employee.Process(problem);

        // return result;
        return null;
    }
}
