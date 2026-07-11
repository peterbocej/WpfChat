using Microsoft.AspNetCore.Mvc;

using WpfChat.Domain.Model;
using WpfChat.WebApi.Repositories;

namespace WpfChat.WebApi.Controllers;

[Route("[controller]")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly IMessagesRepository _messagesRepository;
    private readonly ILogger<ChatController> _logger;
    public ChatController(IMessagesRepository messagesRepository, ILogger<ChatController> logger, IHttpClientFactory httpClientFactory)
    {
        _messagesRepository = messagesRepository;
        _logger = logger;
    }

    [HttpPost("connect")]
    public async Task<IActionResult> Connect([FromBody] string username)
    {
        try
        {
            _logger.LogInformation($"Connect: {username}");

            if (string.IsNullOrEmpty(username))
                return this.BadRequest("Invalid user name.");

            var message = new Message
            {
                From = "[SYSTEM]",
                Body = $"{username} connected."
            };
            var msgRet = await _messagesRepository.AddAsync(message)
                ?? throw new ApplicationException("Error adding message.");
            await _messagesRepository.SaveChangesAsync();
            return Ok(msgRet);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("disconnect")]
    public async Task<IActionResult> Disconnect([FromBody] string username)
    {
        try
        {
            _logger.LogInformation($"Disonnect: {username}");
            var message = new Message
            {
                From = "[SYSTEM]",
                Body = $"{username} disconnected."
            };
            var msgRet = await _messagesRepository.AddAsync(message)
                ?? throw new ApplicationException("Error adding message.");
            await _messagesRepository.SaveChangesAsync();
            return Ok(msgRet);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("list")]
    public async Task<IActionResult> List()
    {
        try
        {
            _logger.LogInformation(nameof(List));
            return Ok(await _messagesRepository.GetMessagesAsync());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetMessage(int id)
    {
        try
        {
            _logger.LogInformation($"GetMessage: {id}");
            var msg = Ok(await _messagesRepository.GetMessageAsync(id));
            if (msg == null)
                return NotFound(id);
            return Ok(msg);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("send")]
    public async Task<IActionResult> Send([FromBody] Message message)
    {
        try
        {
            _logger.LogInformation($"Send: {message}");
            var msgRet = await _messagesRepository.AddAsync(message)
                ?? throw new ApplicationException("Error adding message.");
            await _messagesRepository.SaveChangesAsync();
            return Created();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("check/{lastId:int}")]
    public async Task<IActionResult> Check(int lastId)
    {
        try
        {
            _logger.LogInformation($"Get last: {lastId}");
            return Ok(await _messagesRepository.GetLastMessagesAsync(lastId));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest(ex.Message);
        }
    }
}
