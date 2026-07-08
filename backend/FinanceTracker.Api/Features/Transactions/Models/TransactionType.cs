using System.Text.Json.Serialization;

namespace FinanceTracker.Api.Features.Transactions
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TransactionType
    {
        revenue,
        expense
    }
}