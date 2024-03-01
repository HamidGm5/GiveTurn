using AutoMapper;
using GiveTurn.API.Entities;
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
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        CancellationTokenSource cts;

        public TurnController(ITurnRepository repository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _repository = repository;
            _userRepository = userRepository;
            _mapper = mapper;
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

        [HttpGet("{Userid:int}", Name = "GetUsersTurn")]
        public async Task<ActionResult<ICollection<TurnDto>>> GetUserTurns(int Userid)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Turns = await _repository.GetUserTurns(Userid);
                    if (Turns != null)
                    {
                        return Ok(Turns);
                    }
                    else
                    {
                        return NotFound();
                    }
                }

                else
                {
                    return BadRequest(ModelState);
                }
            }

            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{Userid:int}/{id:int}", Name = "GetUserTurn")]
        public async Task<ActionResult<TurnDto>> GetUserTurn(int Userid, int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Turn = await _repository.GetUserTurn(Userid, id);
                    if (Turn != null)
                    {
                        return Ok(Turn);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }

            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("{Userid:int}", Name = "AddNewTurn")]
        public async Task<ActionResult<TurnDto>> AddNewTurn([FromBody] TurnDto newturn, int Userid)
        {
            try
            {
                cts = new CancellationTokenSource();
                if (ModelState.IsValid)
                {
                    var TurnMap = _mapper.Map<Turn>(newturn);
                    TurnMap.User = await _userRepository.GetUserById(Userid);

                    if (newturn.UserTurnDate != null)
                    {
                        var AddTurn = await _repository.AddTurns(TurnMap);
                        return Ok(newturn);
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }

            catch
            {
                return BadRequest();
            }
        }

        [HttpPatch("{id:int}", Name = "UpdateTurn")]
        public async Task<ActionResult<TurnDto>> UpdateTurn([FromBody] TurnDto UpdateTurn, int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var UpdateTurnMap = _mapper.Map<Turn>(UpdateTurn);
                    UpdateTurnMap.UserTurnDate = await _repository.GiveTurnDateTime();
                    var UpdatedTurn = await _repository.Update(id, UpdateTurnMap);

                    if (UpdateTurn != null)
                    {
                        return Ok(UpdatedTurn);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }

            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id:int}", Name = "DeleteTurn")]
        public async Task<ActionResult<TurnDto>> DeleteTurn(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool DeletedTurn = await _repository.Delete(id);
                    if (DeletedTurn == true)
                    {
                        return Ok("Success");
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }

            catch
            {
                return BadRequest();
            }
        }
    }
}
