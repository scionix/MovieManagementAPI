using Microsoft.AspNetCore.Mvc;
using MovieManagement.Domain.Entities;
using MovieManagement.Domain.Repository;

namespace MovieManagement.API.Controllers
{
    [Route("api/v1/genres")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private ILogger _logger;

        public GenresController(IUnitOfWork unitOfWork, ILogger<GenresController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <summary>
        /// GET call for v1 Movies API
        /// endpoint: api/v1/genres
        /// Returns all genres
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Get()
        {
            try
            {
                var genres = _unitOfWork.Genre.GetAll();
                _logger.LogDebug("Got list of all genres");
                return Ok(genres);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception when calling GetAllGenres");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// GET call for v1 Movies API
        /// endpoint: api/v1/genres/movies
        /// Returns genres with their related movies
        /// </summary>
        [HttpGet("movies")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetGenresWithMovies()
        {
            try
            {
                var genres = _unitOfWork.Genre.GetGenresWithMovies();
                _logger.LogDebug("Got list of all genres with their attached movies");
                return Ok(genres);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception when calling GetAllGenresWithMovies");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// GET call for v1 Movies API
        /// endpoint: api/v1/genres/{id}
        /// Returns genre with ID matching what is given in the route
        /// </summary>
        /// <param name="id">actor id</param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetGenreWithId([FromRoute] int id)
        {
            try
            {
                var genreWithId = _unitOfWork.Genre.GetById(id);

                if (genreWithId == null)
                {
                    _logger.LogDebug("Could not find any genre with ID: {id}", id);
                    return NotFound();
                }

                _logger.LogDebug("Got genre with id: {id}", id);
                return Ok(genreWithId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception when trying to find genre with ID: {id}", id);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// GET call for v1 Movies API
        /// endpoint: api/v1/genres/{id}
        /// Returns genre with name matching what is given in the route
        /// </summary>
        /// <param name="id">actor id</param>
        [HttpGet("name")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetGenreWithName([FromQuery] string name)
        {
            try
            {
                var genreWithId = _unitOfWork.Genre.GetGenreByName(name);

                if (genreWithId.Count() == 0)
                {
                    _logger.LogDebug("Could not find any genre with name: {name}", name);
                    return NotFound();
                }

                _logger.LogDebug("Got genre with name: {name}", name);
                return Ok(genreWithId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception when trying to find genre with name: {name}", name);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// POST call for v1 Movies API
        /// endpoint: api/v1/genres
        /// Returns posted genre on successful POST request
        /// </summary>
        /// <param name="act">actor entity to post</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult PostGenre([FromBody] Genre gen)
        {
            try
            {
                _unitOfWork.Genre.Add(gen);
                _unitOfWork.Save();
                _logger.LogDebug("Posted new genre with name: {name}", gen.Name);
                return new ObjectResult(gen) { StatusCode = StatusCodes.Status201Created };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception when trying to post genre: {name}", gen.Name);
                return StatusCode(500);
            }
        }
    }
}
