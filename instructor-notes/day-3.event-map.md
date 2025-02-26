# Our Identified Events

| Event Name | 
|-------------|
| EmployeeCreated |
| EmployeeRequestLogged |
| ProblemSubmitted |

## Use Case

### Employees

> Employees need to see a list of software to be able to SubmitAProblem

> When the Employee SubmitsAProblem, the Employee should get a "receipt" showing the details of that problem as recorded when they sucessfully submitted it. (EmployeeProblemReadModel?)

> An Employee wants to see a list of all the problems they've submitted, and their current status. - This is "reference data"

### VIP Api

> VipIssueApi needs a reference to the problem submitted by the Employee.

### Techs

> Need a list of all Problems in the "Submitted" state (e.g., not assigned to a tech, etc.)

> Needs a list of submitted Problems they have adopted.

And on and on.



