using AutoMapper;
using GiveTurn.API.Context;
using GiveTurn.API.Entities;
using GiveTurn.API.Repository.Interfaces;
using GiveTurn.Model.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace GiveTurn.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet(Name = "GetAllUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<ICollection<UserDto>>> GetAllUser()
        {
            try
            {
                var Users = await _repository.GetAllUsers();
                if (User == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(Users);
                }
            }

            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{Userid:int}", Name = "GetUserById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<UserDto>> GetuserById(int Userid)
        {
            try
            {
                var User = await _repository.GetUserById(Userid);

                if (User == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(User);
                }
            }

            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{username}/{password}", Name = "Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<UserDto>> Login(string username, string password)
        {
            try
            {
                var User = await _repository.Login(username, password);
                if (User == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(User);
                }
            }

            catch
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost(Name = "SignUp")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<UserDto>> SignUp([FromBody] UserDto user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!await _repository.UserExist(user.Username))
                    {
                        var UserMap = _mapper.Map<User>(user);
                        if (UserMap == null)
                        {
                            return BadRequest();
                        }
                        else
                        {
                            var UserAdd = await _repository.SignUp(UserMap);
                            return Ok(UserAdd);
                        }
                    }
                    else
                    {
                        return BadRequest();
                    }
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

        [HttpPut("{id:int}", Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<UserDto>> UpdateUser(int id, [FromBody] UserDto user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var UserMap = _mapper.Map<User>(user);
                    UserMap.Id = id;
                    var Update = await _repository.Update(id, UserMap);

                    if (Update == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(Update);
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

        [HttpPatch("{id:int}/{status}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<bool>> UpdateHaveTurn(int id, bool status)
        {
            try
            {
                var Update = await _repository.UpdateHaveTurn(id, status);
                if (Update)
                    return Ok("successfully");
                else
                    return NotFound();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id:int}", Name = "DeleteUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<UserDto>> DeleteUser(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool IsDelete = await _repository.Delete(id);
                    if (IsDelete == false)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok("Success");
                    }
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
    }
}
