using aws_sqs_bemobi_api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace aws_sqs_bemobi_api.Helpers
{
    public interface IAWSSQSHelper
    {
        Task<List<Message>> ReceiveMessageAsync();
    }
    public class AWSSQSHelper : IAWSSQSHelper
    {
        private readonly IAmazonSQS _sqs;
        private readonly ServiceConfiguration _settings;
        public AWSSQSHelper(IAmazonSQS sqs, IOptions<ServiceConfiguration> settings)
        {
            this._sqs = sqs;
            this._settings = settings.Value;
        }
        public async Task<List<Message>> ReceiveMessageAsync()
        {
            try
            {
                var request = new ReceiveMessageRequest
                {
                    QueueUrl = _settings.AWSSQS.QueueUrl,
                    MaxNumberOfMessages = 10,
                    WaitTimeSeconds = 5
                };

                var result = await _sqs.ReceiveMessageAsync(request);

                return result.Messages.Any() ? result.Messages : new List<Message>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
