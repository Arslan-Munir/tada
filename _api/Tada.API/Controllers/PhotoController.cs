using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Tada.API.Data.Interfaces;
using Tada.API.Dtos;
using Tada.API.Helpers;
using Tada.API.Models;

namespace Tada.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[Controller]")]
    public class PhotoController : ControllerBase
    {
        private readonly IMapper _mapper;
        private Cloudinary _cloudinary;
        private readonly IUserRepository _repo;
        private readonly IPhotoRepository _photoRepository;
        private readonly CloudinarySettings _cloudinaryConfigs;

        public PhotoController(IMapper mapper, IUserRepository repo, IPhotoRepository photoRepository, IOptions<CloudinarySettings> cloudinaryConfigs)
        {
            _mapper = mapper;
            _repo = repo;
            _photoRepository = photoRepository;
            _cloudinaryConfigs = cloudinaryConfigs.Value;

            Account acc = new Account(
                _cloudinaryConfigs.CloudName,
                _cloudinaryConfigs.ApiKey,
                _cloudinaryConfigs.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }
        
        [HttpGet("{id}", Name="GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromRepo = await _photoRepository.Get(id);
            var photo = _mapper.Map<PhotoToReturnDto>(photoFromRepo);
            return Ok(photo);
        }

        [HttpPost("user/{userId}")]
        public async Task<IActionResult> AddPhotoForUser(int userId, [FromForm] PhotoToSaveDto dto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _repo.Get(userId);
            var file = dto.File;
            var uploadResult = new ImageUploadResult();

            if(file == null || file.Length <= 0)
                return BadRequest();
            
            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.Name, stream),
                    Transformation = new Transformation()
                        .Height(500).Width(500).Crop("fill").Gravity("face")
                };

                uploadResult = _cloudinary.Upload(uploadParams);
            }

            dto.Url = uploadResult.Uri.ToString();
            dto.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(dto);
            if(!userFromRepo.Photos.Any(p=>p.IsMain))
                photo.IsMain = true;

            userFromRepo.Photos.Add(photo);
            if(await _repo.SaveAll())
            {
                var photoToReturn = _mapper.Map<PhotoToReturnDto>(photo);
                return CreatedAtRoute("GetPhoto", new {id = photo.Id}, photoToReturn);
            }

            return BadRequest("Could not add the photo.");
        }

        [HttpPut("{id}/user/{userId}/setMain")]
        public async Task<IActionResult> SetMainPhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var user = await _repo.Get(userId);

            if(!user.Photos.Any(p=>p.Id == id))
                return Unauthorized();

            if(user.Photos.FirstOrDefault(x=>x.Id==id).IsMain)
                return BadRequest("This is already the main photo.");

            var currentMainPhoto = user.Photos.FirstOrDefault(x => x.IsMain);
            currentMainPhoto.IsMain = false;

            var newMainPhoto = user.Photos.FirstOrDefault(x => x.Id == id);
            newMainPhoto.IsMain = true;
            
            if(await _photoRepository.SaveAll())
                return NoContent();

            return BadRequest("Could not set photo to main");
        }

        [HttpDelete("{id}/user/{userId}")]
        public async Task<IActionResult> Delete(int id, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var user = await _repo.Get(userId);

            if (!user.Photos.Any(p => p.Id == id))
                return Unauthorized();

            if (user.Photos.FirstOrDefault(x => x.Id == id).IsMain)
                return BadRequest("You can't delete main photo.");

            var photoToDelete = user.Photos.FirstOrDefault(x => x.Id == id);


            if(photoToDelete.PublicId == null)
                _photoRepository.Delete(photoToDelete);

            else
            {
                var result = _cloudinary.Destroy(new DeletionParams(photoToDelete.PublicId));

                if (result.StatusCode.Equals(HttpStatusCode.OK))
                    _photoRepository.Delete(photoToDelete);
            }
            
            if(await _photoRepository.SaveAll())
                return Ok();

            return BadRequest("Failed to delete photo");
        }
    }
}