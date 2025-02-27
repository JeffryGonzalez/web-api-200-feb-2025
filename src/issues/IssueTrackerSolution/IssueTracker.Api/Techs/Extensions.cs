using System.Reflection.Metadata.Ecma335;
using IssueTracker.Api.Middleware;
using Marten;
using Marten.Events;

namespace IssueTracker.Api.Techs;

public static class Extensions
{
    public static IEndpointRouteBuilder MapTechs(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/help-desk-staff").RequireAuthorization(pol =>
        {
            pol.RequireRole("help-desk");
        });

        group.MapGet("/employees", async (IDocumentSession session) =>
        {
            var response = await session.Query<Employee>().ToListAsync();
            return TypedResults.Ok(response);
        } );
        return group;
    }
}


