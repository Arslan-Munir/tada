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
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _repo;
        private readonly ILikeRepository _likeRepository;

        public UsersController(IMapper mapper, IUserRepository userRepository, ILikeRepository likeRepository)
        {
            _mapper = mapper;
            _repo = userRepository;
            _likeRepository = likeRepository;
        }

        [HttpGet("{id}", Name="GetUser")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _repo.Get(id);
            var userToReturn = _mapper.Map<UserForDetailedDto>(user);
            return Ok(userToReturn);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] UserParams userParams)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var currentUser = await _repo.Get(currentUserId);
            userParams.UserId = currentUserId;
            if(string.IsNullOrWhiteSpace(userParams.Gender))
            {
                userParams.Gender = currentUser.Gender == "male" ? "female" : "male";
            }

            var users = await _repo.GetAll(userParams);
            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);
            Response.AddPagination(users.CurrentPage, users.ItemsPerPage, users.TotalItems, users.TotalPages);
            return Ok(usersToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserToUpdateDto userToUpdateDto)
        {
            if(id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _repo.Get(id);
            _mapper.Map(userToUpdateDto, userFromRepo);
            if(await _repo.SaveAll())
                return NoContent();

            throw new Exception($"Updating user {id} failed on save");
        }

        [HttpPost("{likerId}/like/{likeeId}")]
        public async Task<IActionResult> LikeUser(int likerId, int likeeId)
        {
            if (likerId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var doesLike = await _likeRepository.DoesLike(likerId, likeeId);
            if(doesLike != null)
                return BadRequest("Already liked!");

            if(await _repo.Get(likeeId) == null)
                return NotFound();

            var like = new Like
            {
                LikerId = likerId,
                LikeeId = likeeId
            };

            _likeRepository.Add(like);
            if(await _likeRepository.SaveAll())
                return Ok();

            return BadRequest("Failed to like!");
        }
    }
}