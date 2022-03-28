using aws_sqs_bemobi_api.Models;
using aws_sqs_bemobi_api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace aws_sqs_bemobi_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AWSSQSController : Controller
    {
        private readonly IAWSSQSService _AWSSQSService;
        private readonly ILogger _Logger;
        private readonly string _BadRequestMessage = "Exceção sem tratamento acionada";

        // Injeção de dependência do AWSSQS apenas como modelo
        public AWSSQSController(IAWSSQSService AWSSQSService, ILogger<AWSSQSController> Logger)
        { 
            _AWSSQSService = AWSSQSService;
            _Logger = Logger;
        }

        // Passo 1 - Método recuperaria as informações da fila
        // Passo 2 - Converteria o objeto recuperado para o formato do model File
        // Passo 3 - Executaria a lógica do método InsetSqsAsync
        //[HttpGet("getAllMessages")]
        //public async Task<IActionResult> GetAllMessagesAsync()
        //{
        //    var result = await _AWSSQSService.GetAllMessagesAsync();
        //    return Ok(result);
        //}

        // Método "Moq" que simula a lógica para geração do que foi solicitado
        // Trabalho com os itens recuperados da fila e sendo inseridos no banco MySQL
        [HttpPost("insert-sqs")]
        public async Task<IActionResult> InsertSqsAsync([FromBody] File file)
        {
            try
            {
                File _file = await _AWSSQSService.FindOneAsync(file.filename);
                if (_file != null)
                {
                    if (_file.last_modified > file.last_modified)
                        _Logger.LogInformation("Mensagem mais atualizada já foi processada anteriormente.");
                    else
                        await _AWSSQSService.UpdateAsync(file);
                }
                else
                {
                    await _AWSSQSService.InsertAsync(new()
                    {
                        filename = file.filename,
                        filesize = file.filesize,
                        last_modified = file.last_modified
                    });
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, _BadRequestMessage);
                return BadRequest(_BadRequestMessage);
            }
        }
    }
}
