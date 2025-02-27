using System.Security.Claims;
using Marten;

namespace IssueTracker.Api.Middleware;

public class AuthenticatedUserMakesARequestMiddleware(IDocumentSession session) : IEndpointFilter
{


    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {

        var sub = context?.HttpContext.User.FindFirstValue("sub");
        if (sub is not null)
        {
            // this is going through EVERY EVENT in the system (inline) and coming up with this result.
            var employee = await session.Query<AuthenticatedUser>().Where(u => u.Sub == sub).SingleOrDefaultAsync();

            if (employee is null)
            {
                var idForNewEmployee = Guid.NewGuid();
                session.Events.StartStream(idForNewEmployee, new AuthenticatedUserCreated(idForNewEmployee, sub));
            }
            else
            {
                session.Events.Append(employee.Id, new AuthenticatedUserHitApi(employee.Id));
            }

        }
        await session.SaveChangesAsync();

        return await next(context);
    }
}

public record AuthenticatedUser
{
    public Guid Id { get; set; }
    public string Sub { get; set; } = string.Empty;

    public DateTimeOffset LastApiUsage { get; set; }

    public DateTimeOffset EmployeeCreated { get; set; }

    public static AuthenticatedUser Create(AuthenticatedUserCreated @event)
    {
        return new AuthenticatedUser
        {
            Id = @event.Id,
            Sub = @event.Sub,
            EmployeeCreated = DateTimeOffset.Now,
            LastApiUsage = DateTimeOffset.Now,
        };
    }

    public static AuthenticatedUser Apply(AuthenticatedUserHitApi _, AuthenticatedUser user)
    {
        return user with { LastApiUsage = DateTimeOffset.UtcNow };
    } 
}

public record AuthenticatedUserCreated(Guid Id, string Sub);

public  record AuthenticatedUserHitApi(Guid Id);