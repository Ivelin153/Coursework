using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/fileUpload/{userId}")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        readonly string userHome = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        public FileController(IDatingRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet("{id}", Name = "GetFile")]
        public async Task<IActionResult> GetFile(int id)
        {
            var fileFromRepo = await _repo.GetFile(id);

            var file = _mapper.Map<FileForCreationDto>(fileFromRepo);

            return Ok(file);
        }

        [HttpPost]
        public async Task<IActionResult> AddFileForUser(int userId, [FromForm] FileForCreationDto fileForCreationDto)
        {
            var pathToSave = $@"{userHome}\Desktop\PTS\userId_{userId.ToString()}\";

            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _repo.GetUser(userId);

            var file = fileForCreationDto.File;

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);

                    if (!Directory.Exists(pathToSave))
                        Directory.CreateDirectory(pathToSave);

                    var isFileExisting = Directory.GetFiles(pathToSave, fileName).Any(x => x.ToString() == fullPath);
                    if (!isFileExisting)
                    {
                        using (var str = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(str);
                        }
                    }
                    else
                        return BadRequest("File already uploaded");

                }
            }
            var fileToAdd = _mapper.Map<Model.File>(fileForCreationDto);
            fileToAdd.FileName = file.FileName;

            userFromRepo.Files.Add(fileToAdd);

            if (await _repo.SaveAll())
            {
                return CreatedAtRoute("GetFile", new { userId = userId, id = fileToAdd.Id }, fileToAdd);
            }

            return BadRequest("Could not add the file!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFile(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();


            var user = await _repo.GetUser(userId);

            var pathToDelete = $@"{userHome}\Desktop\PTS\userId_{userId.ToString()}\";

            if (!user.Files.Any(p => p.Id == id))
                return Unauthorized();

            var fileFromRepo = await _repo.GetFile(id);

            if (fileFromRepo != null)
            {
                _repo.Delete(fileFromRepo);
                System.IO.File.Delete(pathToDelete + $"{fileFromRepo.FileName}");
            }

            if (await _repo.SaveAll())
                return Ok();

            return BadRequest("Failed to delete the file");
        }

    }
}
