using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieManagement.Domain.Dto;
using MovieManagement.Domain.Entities;
using MovieManagement.Domain.Repository;

namespace MovieManagement.API.Controllers
{
    [Route("api/v1/movies")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private ILogger _logger;

        public MoviesController(IUnitOfWork unitOfWork, ILogger<MoviesController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <summary>
        /// GET call for v1 Movies API
        /// endpoint: api/v1/movies
        /// Returns all movies
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Movie>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Get()
        {
            try
            {
                var movies = _unitOfWork.Movie.GetAll();

                _logger.LogDebug("Got list of all movies");
                return Ok(movies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception when calling GetAllMovies");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// GET call for v1 Movies API
        /// endpoint: api/v1/movies/{name}
        /// Returns movies with name matching what is given in the query string
        /// </summary>
        /// <param name="name">movie name</param>
        [HttpGet("name")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Movie>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetMoviesWithFullName([FromQuery] string name)
        {
            try
            {
                var movies = _unitOfWork.Movie.Find(m=> m.Name == name);

                if (movies.Count() == 0)
                {
                    _logger.LogDebug("Could not find any movies with name: {name}", name);
                    return NotFound();
                }

                _logger.LogDebug("Got list of all movies with name: {name}", name);
                return Ok(movies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception when calling get for movie with name: {name}", name);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// GET call for v1 Movies API
        /// endpoint: api/v1/movies/{id}
        /// Returns movies with name matching what is given in the query string
        /// </summary>
        /// <param name="name">movie name</param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Movie))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetMovieWithId([FromRoute] int id)
        {
            try
            {
                var movie = _unitOfWork.Movie.GetById(id);

                if (movie == null)
                {
                    _logger.LogDebug("Could not find any movies with id: {id}", id);
                    return NotFound();
                }

                _logger.LogDebug("Got list of all movies with id: {id}", id);
                return Ok(movie);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception when calling get for movie with id: {id}", id);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// GET call for v1 Movies API
        /// endpoint: api/v1/movies/actor/{id}
        /// Returns movies with name matching what is given in the query string
        /// </summary>
        /// <param name="name">movie name</param>
        [HttpGet("actor/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Movie))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetMovieWithActorId([FromRoute] int id)
        {
            try
            {
                var movie = _unitOfWork.Movie.GetByActorId(id);

                if (movie == null)
                {
                    _logger.LogDebug("Could not find any movies that had actor with id: {id}", id);
                    return NotFound();
                }

                _logger.LogDebug("Got list of all movies that had actor with id: {id}", id);
                return Ok(movie);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception when calling get for movie with actor of id: {id}", id);
                return StatusCode(500);
            }
        }
    }
}
