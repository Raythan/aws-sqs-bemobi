namespace aws_sqs_bemobi_api.Models
{
    public class AllMessage
    {
        public string MessageId { get; set; }
        public string ReceiptHandle { get; set; }
        public UserDetail UserDetail { get; set; }
        public AllMessage() => UserDetail = new UserDetail();
    }
}
