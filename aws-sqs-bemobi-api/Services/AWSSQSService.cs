using Amazon.SQS.Model;
using aws_sqs_bemobi_api.Helpers;
using aws_sqs_bemobi_api.Models;
using aws_sqs_bemobi_api.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aws_sqs_bemobi_api.Services
{
    public interface IAWSSQSService
    {
        Task<List<AllMessage>> GetAllMessagesAsync();
        Task InsertAsync(File file);
        Task UpdateAsync(File file);
        Task<File> FindOneAsync(string filename);
    }
    public class AWSSQSService : IAWSSQSService
    {
        private readonly IAWSSQSHelper _AWSSQSHelper;
        private readonly IFileRepository _FileRepository;

        // A identificação no AWSSQS é necessária para utilização real desse construtor
        //public AWSSQSService(IAWSSQSHelper AWSSQSHelper, IFileRepository FileRepository)
        //{
        //    _AWSSQSHelper = AWSSQSHelper;
        //    _FileRepository = FileRepository;
        //}

        // Construtor utilizado para a modelagem do "Moq" no AWSSQSController
        public AWSSQSService(IFileRepository FileRepository) => _FileRepository = FileRepository;
        // Método que seria utilizado para recuperar os dados do AWSSQS
        public async Task<List<AllMessage>> GetAllMessagesAsync()
        {
            List<AllMessage> allMessages = new List<AllMessage>();
            try
            {
                List<Message> messages = await _AWSSQSHelper.ReceiveMessageAsync();
                allMessages = messages.Select(c => new AllMessage { MessageId = c.MessageId, ReceiptHandle = c.ReceiptHandle, UserDetail = JsonConvert.DeserializeObject<UserDetail>(c.Body) }).ToList();
                return allMessages;
            }
            catch (Exception ex) { throw ex; }
        }
        public async Task InsertAsync(File file) => await _FileRepository.InsertAsync(file);
        public async Task UpdateAsync(File file) => await _FileRepository.UpdateAsync(file);
        public async Task<File> FindOneAsync(string filename) => await _FileRepository.FindOneAsync(filename);
    }
}
