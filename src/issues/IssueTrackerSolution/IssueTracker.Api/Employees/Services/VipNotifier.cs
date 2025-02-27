using IssueTracker.Api.Employees.Api;
using IssueTrackerShared;

namespace IssueTracker.Api.Employees.Services;

public class VipNotifier(HttpClient client) : INotifyTheVipApiOfAProblem
{

    public async Task<bool> NotifyOfProblem(VipIssueCreateModel issue)
    {
        var response = await client.PostAsJsonAsync("/vip/notifications", issue);

        response.EnsureSuccessStatusCode();

        return true;
    }
}
