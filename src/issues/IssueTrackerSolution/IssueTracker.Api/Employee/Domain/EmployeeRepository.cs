using Marten;

namespace IssueTracker.Api.Employee.Domain;

public class EmployeeRepository(IDocumentSession session)
{
    public Employee Create(string sub)
    {
        var employeeEntity = new EmployeeEntity
        {
            Id = Guid.NewGuid(),
            Sub = sub
        };
        return new Employee(employeeEntity, this);
    }

    internal async Task SaveAsync(EmployeeEntity entity, CancellationToken token)
    {
        // we'll talk about the entity in a second.
        await session.SaveChangesAsync(token);
    }
}

// our "service" from last class.

public class Employee(EmployeeEntity entity, EmployeeRepository repository)
{
    public Guid Id { get; } = entity.Id;

    public async Task SaveAsync(CancellationToken token)
    {
        await repository.SaveAsync(entity, token);
    }
   
}
// Our "persistence model" - what we are saving, etc.
public class EmployeeEntity
{
    public Guid Id { get; set; }
    // Network Id, verified identity, etc.
    public string Sub { get; set; } = string.Empty;
    
}