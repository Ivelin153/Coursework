using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AprioriApp.API.Data;
using AprioriApp.API.DTOs;
using AprioriApp.API.Helpers;
using AprioriApp.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;

namespace AprioriApp.API.Controllers
{
    [Authorize]
    [Route("api/apriori/{userId}")]
    [ApiController]
    public class AprioriController : ControllerBase
    {
        private const int _IndexOfComponent = 2;
        private const int _IndexOfEventName = 3;
        private const int _IndexOfStudentId = 1;
        private const int _IndexOfCourseId = 3;
        private readonly IMapper _mapper;
        private readonly IAprioriRepository _repo;
        private string _dataFile = string.Empty;
        private readonly string _userHome = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        public AprioriController(IAprioriRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAprioriResult(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var user = await _repo.GetUser(userId);

            if (!user.Files.Any(f => f.Id == id))
                return Unauthorized();

            var fileFromRepo = await _repo.GetFile(id);

            var fileToExtractDataFrom = $@"{_userHome}\Desktop\PTS\userId_{userId.ToString()}\{fileFromRepo.FileName}";

            if (System.IO.File.Exists(fileToExtractDataFrom))
                ExtractDataFromFile(userId, fileToExtractDataFrom, fileFromRepo.FileName);

            var aprioriResult = await ProcessData();

            if (aprioriResult != null)
                return Ok(aprioriResult);

            return BadRequest();
        }


        private async Task<AprioriResult> ProcessData()
        {
            var apriori = new Apriori(_dataFile);
            int k = 1;
            int Support = 2;
            List<ItemSet> ItemSets = new List<ItemSet>();
            var aprioriResult = new AprioriResult();
            var allLines = await System.IO.File.ReadAllLinesAsync(_dataFile);
            aprioriResult.AllLines = allLines.ToList();

            bool next;
            do
            {
                next = false;
                var L = apriori.GetItemSet(k, Support, IsFirstItemList: k == 1);
                if (L.Count > 0)
                {
                    List<AssociationRule> rules = new List<AssociationRule>();
                    if (k != 1)
                        rules = apriori.GetRules(L);

                    var dictinory = new Dictionary<string, int>();
                    foreach (var item in L)
                    {
                        var key = string.Join(", ", item.Key);
                        var value = item.Value;
                        dictinory.Add(key, value);
                    }
                    aprioriResult.ItemSet.Add(dictinory);
                    
                    aprioriResult.AssociationRules.Add(rules);
                    next = true;
                    k++;
                    ItemSets.Add(L);
                }
            } while (next);

            return aprioriResult;
        }

        private void ExtractDataFromFile(int userId, string file, string fileName)
        {
            var sb = new StringBuilder();

            if (fileName.EndsWith(".csv"))
            {
                using (TextFieldParser parser = new TextFieldParser(file))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    while (!parser.EndOfData)
                    {
                        //Process row
                        string[] fields = parser.ReadFields();
                        if (fields[_IndexOfComponent].ToLower().Contains("system") && fields[_IndexOfEventName].ToLower().Contains("course viewed"))
                        {
                            var description = fields[4].Split('\'');
                            var studentId = description[_IndexOfStudentId];
                            var cousreId = description[_IndexOfCourseId];

                            sb.AppendLine($"{studentId} {cousreId}");
                        }
                    }

                    sb.ToString().TrimEnd();
                }
                _dataFile = $@"{_userHome}\Desktop\PTS\userId_{userId.ToString()}\{fileName}.txt";

                using (TextWriter writer = new StreamWriter(_dataFile, false))
                {
                    writer.Write(sb.ToString().TrimEnd());
                }
            }
            else
            {
                _dataFile = file;
            }
        }
    }
}
