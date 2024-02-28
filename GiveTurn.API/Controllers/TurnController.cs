using GiveTurn.API.Repository;
using GiveTurn.API.Repository.Interfaces;
using GiveTurn.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace GiveTurn.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TurnController : ControllerBase
    {
        private readonly ITurnRepository _repository;

        public TurnController(ITurnRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<TurnDto>>> GetAllTurns()
        {
            try
            {
                var Turns = await _repository.GetAllTurns();
             
                if (Turns != null)
                {
                    return Ok(Turns);
                }
                else
                {
                    return NotFound();
                }
            }

            catch
            {
                return BadRequest();
            }
        }

    }
}
