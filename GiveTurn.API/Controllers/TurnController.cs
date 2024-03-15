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

        public TurnController(ITurnRepository repository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _repository = repository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DateTime>> ShowLastTime()
        {
            try
            {
                DateTime TurnTime = await _repository.GiveTurnDateTime();
                if(TurnTime !=  DateTime.MinValue) 
                {
                    return Ok(TurnTime);
                }
                else
                {
                    return BadRequest();
                }
            }

            catch
            {
                return NotFound();
            }
        }

        [HttpGet("{Userid:int}", Name = "GetUsersTurn")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TurnDto>> AddNewTurn([FromBody] TurnDto newturn, int Userid)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var TurnMap = _mapper.Map<Turn>(newturn);
                    TurnMap.User = await _userRepository.GetUserById(Userid);
                    TurnMap.UserTurnDate = await _repository.GiveTurnDateTime();
                    await _repository.AddTurns(TurnMap);
                    return Ok(newturn);
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        [HttpDelete(Name = "DeleteAllTurns")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TurnDto>> DeleteAllTurns()
        {
            var DeleteTurns = await _repository.DeleteAllTurns();
            if (DeleteTurns == true)
            {
                return Ok("Success");
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
