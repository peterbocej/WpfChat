using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using WpfChat.Domain.Model;
using WpfChat.WebApi.Repositories;

namespace WpfChat.WebApi.Controllers;

[Route("[controller]")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly IMessagesRepository _messagesRepository;
    public ChatController(IMessagesRepository messagesRepository)
    {
        _messagesRepository = messagesRepository;
    }

    [HttpPost("connect")]
    public async Task<IActionResult> Connect([FromBody]string username)
    {
        try
        {
            var message = new Message
            {
                From = "[SYSTEM]",
                Body = $"{username} connected."
            };
            var id = await _messagesRepository.AddAsync(message);
            await _messagesRepository.SaveChangesAsync();
            message.MessageId = id;
            return CreatedAtAction(nameof(GetMessage), new { id }, message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("disconnect")]
    public async Task<IActionResult> Disconnect([FromBody]string username)
    {
        try
        {
            var message = new Message
            {
                From = "[SYSTEM]",
                Body = $"{username} disconnected."
            };
            var id = await _messagesRepository.AddAsync(message);
            await _messagesRepository.SaveChangesAsync();
            message.MessageId = id;
            return CreatedAtAction(nameof(GetMessage), new { id }, message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetSavedMessages()
    {
        try
        {
            return Ok(await _messagesRepository.GetMessagesAsync());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetMessage(int id)
    {
        try
        {
            var msg = Ok(await _messagesRepository.GetMessageAsync(id));
            if (msg == null)
                return NotFound(id);
            return Ok(msg);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] Message message)
    {
        try
        {
            var id = await _messagesRepository.AddAsync(message);
            await _messagesRepository.SaveChangesAsync();
            message.MessageId = id;
            return CreatedAtAction(nameof(GetMessage), new { id }, message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("check/{lastId:int}")]
    public async Task<IActionResult> GetLastMessages(int lastId)
    {
        try
        {
            return Ok(await _messagesRepository.GetLastMessagesAsync(lastId));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
