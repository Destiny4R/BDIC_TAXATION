using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BDIC_TAXATION_MODELS.ViewModels
{
    public class VerifyPaymentResponse
    {
        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("data")]
        public TransactionData Data { get; set; }

        [JsonPropertyName("execTime")]
        public double ExecTime { get; set; }

        [JsonPropertyName("error")]
        public List<object> Error { get; set; }
    }
    public class TransactionData
    {
        [JsonPropertyName("businessCode")]
        public string BusinessCode { get; set; }

        [JsonPropertyName("transRef")]
        public string TransRef { get; set; }

        [JsonPropertyName("businessRef")]
        public string BusinessRef { get; set; }

        [JsonPropertyName("debitedAmount")]
        public double DebitedAmount { get; set; }

        [JsonPropertyName("transAmount")]
        public double TransAmount { get; set; }

        [JsonPropertyName("transFeeAmount")]
        public double TransFeeAmount { get; set; }

        [JsonPropertyName("settlementAmount")]
        public double SettlementAmount { get; set; }

        [JsonPropertyName("customerId")]
        public string CustomerId { get; set; }

        [JsonPropertyName("transactionDate")]
        public string TransactionDate { get; set; }

        [JsonPropertyName("channelId")]
        public int ChannelId { get; set; }

        [JsonPropertyName("currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("metadata")]
        public List<MetadataItem> Metadata { get; set; }
    }

    public class MetadataItem
    {
        [JsonPropertyName("insightTag")]
        public string InsightTag { get; set; }

        [JsonPropertyName("insightTagValue")]
        public string InsightTagValue { get; set; }

        [JsonPropertyName("insightTagDisplay")]
        public string InsightTagDisplay { get; set; }
    }
}
