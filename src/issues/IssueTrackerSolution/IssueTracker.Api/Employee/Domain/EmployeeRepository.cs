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
       // session.Insert(entity); // This will fail if the entity with that id already exists.
        session.Store(entity); // this will do an "upsert" - if it isn't there, it will create it, if it is, it will replace it.
        await session.SaveChangesAsync(token);
    }

    public async Task<Employee?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        var employeeEntity = await session.LoadAsync<EmployeeEntity>(id);
        if(employeeEntity is not null)
        {
            return new Employee(employeeEntity, this);
        }
        return null;
    }
    public async Task<Employee?> GetBySubAsync(string sub, CancellationToken token = default)
    {
        var employeeEntity = await session.Query<EmployeeEntity>()
            .Where(e => e.Sub == sub)
            .SingleOrDefaultAsync(token);
        if (employeeEntity is not null)
        {
            return new Employee(employeeEntity, this);
        }
        return null;
    }
}

// our "service" from last class.

public class Employee(EmployeeEntity entity, EmployeeRepository repository)
{
    public Guid Id { get; } = entity.Id;

    public async Task SaveAsync(CancellationToken token =default)
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