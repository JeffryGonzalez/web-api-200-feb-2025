

using IssueTracker.Api.Employee.Domain;
using Microsoft.AspNetCore.Http.HttpResults;

namespace IssueTracker.Api.Employee.Api;

public static class SubmittingAProblem
{
    public static async  Task<Ok> SubmitAsync(
        ProblemSubmitModel request,
      
        Guid softwareId,
        CancellationToken token
        )
    {
       
        var problem = new SubmitProblem(softwareId, request.Description);
        //var emp = new Employee.Domain.Employee(null, null);
        //var problemSubmitted = emp.Process(problem);
     
        return TypedResults.Ok();
    }
}


public record ProblemSubmitModel(string Description);