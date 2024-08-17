using AutoMapper;
using GiveTurn.API.Entities;
using GiveTurn.API.Repository.Interfaces;
using GiveTurn.Model.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace GiveTurn.API.Controllers
{
    [ApiController]
    [Route("/api/[Controller]")]
    public class DeletedTurnsController : Controller
    {
        private readonly IDeleteTurnsRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public DeletedTurnsController(IDeleteTurnsRepository repository,
                                        IMapper mapper,
                                        IUserRepository userRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        [HttpGet("{userid:int}", Name = "UserDeletedTurn")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<bool>> UserDeletedTurn(int userid)
        {
            try
            {
                var Deleted = await _repository.UserDeletedTurn(userid);
                if (Deleted)
                    return Ok("have");
                return BadRequest();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("{userid:int}", Name = "AddDeletedTurn")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> AddDeletedTurn([FromBody] DeleteTurnsDto turnsDto, int userid)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var TurnMap = _mapper.Map<DeleteTurns>(turnsDto);
                    TurnMap.user = await _userRepository.GetUserById(userid);
                    if (TurnMap.user == null)
                    {
                        return NotFound();
                    }
                    var Delete = await _repository.AddDeleteTurn(TurnMap);

                    if (!Delete)
                        return BadRequest();
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete(Name = "RemoveAllTodayTurns")]
        [ProducesResponseType(200)]
        public async Task<ActionResult> RemoveAllTodayTurns()
        {
            try
            {
                await _repository.DeleteTurns();
                return Ok("successfully");
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
