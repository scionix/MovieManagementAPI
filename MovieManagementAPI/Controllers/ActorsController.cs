using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MovieManagement.Domain.Dto;
using MovieManagement.Domain.Entities;
using MovieManagement.Domain.Repository;
using System.Collections.Generic;

namespace MovieManagement.API.Controllers
{
    [Route("api/v1/actors")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private ILogger _logger;
        private readonly IMapper _mapper;

        public ActorsController(IUnitOfWork unitOfWork, ILogger<ActorsController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// GET call for v1 Movies API
        /// endpoint: api/v1/actors
        /// Returns all actors in the form of the ActorDto
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ActorDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Get()
        {
            try
            {
                var actors = _mapper.Map<IEnumerable<ActorDto>>(_unitOfWork.Actor.GetAll());

                _logger.LogDebug("Got list of all actors");
                return Ok(actors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception when calling GetAllActors");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// GET call for v1 Movies API
        /// endpoint: api/v1/actors/movies
        /// Returns actors with their related movies
        /// </summary>
        [HttpGet("movies")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ActorDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetActorsWithMovies()
        {
            try 
            {
                var actors = _mapper.Map<List<ActorDto>>(_unitOfWork.Actor.GetActorsWithMovies());
                _logger.LogDebug("Got list of all actors with their attached movies");
                return Ok(actors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception when calling GetAllActorsWithMovies");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// GET call for v1 Movies API
        /// endpoint: api/v1/actors/movies
        /// Returns actors with their related biography
        /// </summary>
        [HttpGet("biography")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ActorDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetActorsWithBio()
        {
            try
            {
                var actors = _mapper.Map<List<ActorDto>>(_unitOfWork.Actor.GetActorsWithBiography());
                _logger.LogDebug("Got list of all actors with their attached biography");
                return Ok(actors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception when calling GetAllActorsWithBio");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// GET call for v1 Movies API
        /// endpoint: api/v1/actors/{name}
        /// Returns actors with full name matching what is given in the query string
        /// </summary>
        /// <param name="firstName">actor first name</param>
        /// <param name="lastName">actor last name</param>
        [HttpGet("name")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ActorDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetActorsWithFullName([FromQuery] string firstName, [FromQuery] string lastName)
        {
            try 
            {
                var actors = _mapper.Map<List<ActorDto>>(_unitOfWork.Actor.GetActorsByFullName(firstName, lastName));

                if (actors.IsNullOrEmpty())
                {
                    _logger.LogDebug("Could not find any actors with name: {firstname} {lastname}", firstName, lastName);
                    return NotFound();
                }

                _logger.LogDebug("Got list of all actors with name: {firstname} {lastname}", firstName, lastName);
                return Ok(actors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception when get for actor with name: {firstname} {lastname}", firstName, lastName);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// GET call for v1 Movies API
        /// endpoint: api/v1/actors/{id}
        /// Returns actors with ID matching what is given in the route
        /// </summary>
        /// <param name="id">actor id</param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Actor))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetActorWithId([FromRoute] int id)
        {
            try
            {
                var actorWithId = _mapper.Map<ActorDto>(_unitOfWork.Actor.GetById(id));

                if (actorWithId == null)
                {
                    _logger.LogDebug("Could not find any actors with ID: {id}", id);
                    return NotFound();
                }

                _logger.LogDebug("Got actor with id: {id}", id);
                return Ok(actorWithId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception when trying to get actor with ID: {id}", id);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// GET call for v1 Movies API
        /// endpoint: api/v1/actors/{id}
        /// Updates actor with ID matching what is given in the route
        /// </summary>
        /// <param name="id">actor id</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Actor))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateActorWithId([FromRoute] int id, [FromBody] Actor act)
        {
            try
            {
                var actorWithId = _unitOfWork.Actor.GetById(id);

                if (actorWithId == null)
                {
                    _logger.LogDebug("Could not find any actors with ID: {id}", id);
                    return NotFound();
                }

                actorWithId = act;
                _unitOfWork.Actor.Update(actorWithId);
                _unitOfWork.Save();

                _logger.LogDebug("Updated actor with id: {id}", id);
                return new ObjectResult(actorWithId) { StatusCode = StatusCodes.Status201Created };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception when trying to get actor with ID: {id}", id);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// POST call for v1 Movies API
        /// endpoint: api/v1/actors
        /// Returns posted actor on successful POST request
        /// </summary>
        /// <param name="act">actor entity to post</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult PostActor([FromBody] Actor act)
        {
            try
            {
                _unitOfWork.Actor.Add(act);
                _unitOfWork.Save();
                _logger.LogDebug("Posted new actor with name: {firstName} {lastName}", act.FirstName, act.LastName);
                return new ObjectResult(act) { StatusCode = StatusCodes.Status201Created };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception when trying to post actor: {firstName}, {lastName}", act.FirstName, act.LastName);
                return StatusCode(500);
            }
        }
    }
}
