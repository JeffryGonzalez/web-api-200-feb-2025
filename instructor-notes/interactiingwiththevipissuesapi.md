# Interactions


When a VIP submits a problem with a piece of software the IssueTracker.Api should call the VIP API with the right request model and link.


## They are going to give us a list of employees that are VIPs really soon.

- we may need (hint) an endpoint for HR to call to say "the person with this sub should be considered a VIP"


## After we call that API the problem's status should be "Submitted For VIP Treatment"
- employees should still be able to cancel in this state.


On the VIP API -

- They are going to notify a bunch of techs or whatever, and one of them is going to "adopt" this.

- They need to tell US that it has been assigned to a VIP tech. 

When that happens, the status should be changed to "Assigned to VIP Tech" and the issue should no longer be cancellable by the employee that submitted it.

