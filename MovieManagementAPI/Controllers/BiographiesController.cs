using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MovieManagement.DataAccess.Implementation;
using MovieManagement.Domain.Repository;

namespace MovieManagement.API.Controllers
{
    [Route("api/v1/biographies")]
    [ApiController]
    public class BiographiesController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private ILogger _logger;

        public BiographiesController(IUnitOfWork unitOfWork, ILogger<BiographiesController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <summary>
        /// GET call for v1 Movies API
        /// endpoint: api/v1/biographies
        /// Returns all biographies
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Get()
        {
            try
            {
                var biographies = _unitOfWork.Biography.GetAll();
                _logger.LogDebug("Got list of all biographies");
                return Ok(biographies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception when calling GetAllBiographies");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// GET call for v1 Movies API
        /// endpoint: api/v1/biographies/{id}
        /// Returns actors with ID matching what is given in the route
        /// </summary>
        /// <param name="id">actor id</param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetBiographyWithId([FromRoute] int id)
        {
            try
            {
                var bioWithId = _unitOfWork.Biography.GetById(id);

                if (bioWithId == null)
                {
                    _logger.LogDebug("Could not find any biography with ID: {id}", id);
                    return NotFound();
                }

                _logger.LogDebug("Got biography with id: {id}", id);
                return Ok(bioWithId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception when trying to get biography with ID: {id}", id);
                return StatusCode(500);
            }
        }
    }
}
