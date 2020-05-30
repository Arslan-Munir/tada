using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tada.API.Data.Interfaces;
using Tada.API.Dtos;
using Tada.API.Helpers;
using Tada.API.Models;

namespace Tada.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[Controller]")]
    [ServiceFilter(typeof(LogUserActivity))]
    public class MessagesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMessageRepository _repo;
        private readonly IUserRepository _userRepository;

        public MessagesController(IMapper mapper, IMessageRepository repo, IUserRepository userRepository)
        {
            _mapper = mapper;
            _repo = repo;
            _userRepository = userRepository;
        }

        [HttpGet("{id}/user/{userId}", Name="GetMessage")]
        public async Task<IActionResult> Get(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messageFromRepo = await _repo.Get(id);

            if(messageFromRepo == null)
                return NotFound();

            return Ok(messageFromRepo);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetMessagesForUser(int userId, [FromQuery] MessageParams messageParams)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            messageParams.UserId = userId;
            var messagesFromRepo = await _repo.GetMessagesForUser(messageParams);
            var messagesToReturn = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesFromRepo);
            Response.AddPagination(messagesFromRepo.CurrentPage, messagesFromRepo.ItemsPerPage, messagesFromRepo.TotalItems, messagesFromRepo.TotalPages);
            return Ok(messagesToReturn);
        }

        [HttpGet("{senderId}/thread/{receiverId}")]
        public async Task<IActionResult> GetMessageThread(int senderId, int receiverId)
        {
            if (senderId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messageFromRepo = await _repo.GetMessageThread(senderId, receiverId);
            var messageThread = _mapper.Map<IEnumerable<MessageToReturnDto>>(messageFromRepo);
            return Ok(messageThread);
        }

        [HttpPost("user/{userId}")]
        public async Task<IActionResult> Create(int userId, MessageToCreateDto dto)
        {
            var sender = await _userRepository.Get(userId);
            if (sender.Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            dto.SenderId = userId;

            var receiver = await _userRepository.Get(dto.ReceiverId);
            if(_userRepository.Get(dto.ReceiverId) == null)
                return NotFound("Could not find user.");

            var message = _mapper.Map<Message>(dto);

            _repo.Add(message);

            if(await _repo.SaveAll())
            {
                var messageToReturn = _mapper.Map<MessageToReturnDto>(message);
                return CreatedAtRoute("GetMessage", new {id = message.Id, userId = userId}, messageToReturn);
            }

            throw new Exception("Failed to sent message.");
        }

        [HttpPut("{id}/user/{userId}")]
        public async Task<IActionResult> DeleteMessage(int id, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messageFromRepo = await _repo.Get(id);
            if(messageFromRepo.SenderId == userId)
                messageFromRepo.SenderDeleted = true;
            
            if(messageFromRepo.ReceiverId == userId)
                messageFromRepo.ReceiverDeleted = true;

            if(messageFromRepo.SenderDeleted && messageFromRepo.ReceiverDeleted)
                _repo.Delete(messageFromRepo);
            
            if(await _repo.SaveAll())
                return NoContent();

            throw new Exception("Error deleting message");
        }

        [HttpPut("{id}/read/user/{userId}")]
        public async Task<IActionResult> MarkAsRead(int id, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var message = await _repo.Get(id);

            if(message.ReceiverId != userId)
                return Unauthorized();

            message.IsRead = true;
            message.ReadDate = DateTime.Now;

            await _repo.SaveAll();
            return NoContent();
        }
    }
}