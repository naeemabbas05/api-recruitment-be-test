using ApiApplication.Database;
using ApiApplication.Database.Entities;
using ApiApplication.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShowtimeController : Controller
    {
        private readonly IShowtimesRepository _showtimesRepository;
        public ShowtimeController(IShowtimesRepository showtimesRepository)
        {
            _showtimesRepository = showtimesRepository;
        }

        [Authorize(Policy = "Read")]
        [HttpGet]
        public IActionResult Get([FromHeader(Name = "ApiKey")][Required] string token, string movieName,string date)
        {
            var response = new Response<IEnumerable<ShowtimeEntity>>();

            if (string.IsNullOrEmpty(movieName) && string.IsNullOrEmpty(date))
            {
                 response = _showtimesRepository.GetCollection();
      
            }
            else {
                if (string.IsNullOrEmpty(movieName))
                {
                    response = _showtimesRepository.GetCollection(a =>(a.StartDate <= DateTime.Parse(date) && a.EndDate >= DateTime.Parse(date)));

                }
                else if (string.IsNullOrEmpty(date))
                {
                   response = _showtimesRepository.GetCollection(a => a.Movie.Title.Contains(movieName));

                }
                else {
                    response = _showtimesRepository.GetCollection(a => a.Movie.Title.Contains(movieName) || (a.StartDate <= DateTime.Parse(date) && a.EndDate >= DateTime.Parse(date)));

                }
               
            }
            return Ok(response);

        }
        [Authorize(Policy = "Write")]
        [HttpPost]
        public async Task<IActionResult> Post([FromHeader(Name = "ApiKey")][Required] string token, ShowtimeEntity showtime)
        {
            var response = await _showtimesRepository.Add(showtime);
            return Ok(response);
        }
        [Authorize(Policy = "Write")]
        [HttpPut]
        public async Task<IActionResult> Put([FromHeader(Name = "ApiKey")][Required] string token, ShowtimeEntity showtime)
        {
            var response = await _showtimesRepository.Update(showtime);
            return Ok(response);
        }
        [Authorize(Policy = "Write")]
        [HttpPatch]
        public async Task<IActionResult> Patch([FromHeader(Name = "ApiKey")][Required] string token)
        {
            throw new NotSupportedException();
        }
        [Authorize(Policy = "Write")]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromHeader(Name = "ApiKey")][Required] string token, int Id)
        {
            var response = await _showtimesRepository.Delete(Id);
            return Ok(response);
        }


    }
}
