

using System.Text.Json.Serialization;

namespace IssueTrackerShared;

[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Default)]
[JsonSerializable(typeof(VipIssueResponseModel))]

internal partial class JsonSourceGenerationRules : JsonSerializerContext
{

}
public record VipIssueCreateModel(string Problem, string Description);

public record VipIssueResponseModel(Guid Id, string Problem, string Description, string Status);