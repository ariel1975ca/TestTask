using Microsoft.AspNetCore.Mvc;

using TestTaskWebApi.Models;
using TestTaskWebApi.Services;

namespace TestTaskWebApi.Controllers
{
    /// <summary>
    ///   Handle the API interactions for the tests
    /// </summary>
    [ApiController]
    [Route("testtask")]
    public class TestTaskController : ControllerBase
    {
        #region - Properties -

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<TestTaskController> _logger;

        /// <summary>
        /// The test task service
        /// </summary>
        private readonly ITestTaskService _testTaskService;

        #endregion - Properties -

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="TestTaskController" /> class.
        /// </summary>
        /// <param name="testTaskService">The Test Task service.</param>
        /// <param name="logger">The logger.</param>
        public TestTaskController(ITestTaskService testTaskService, ILogger<TestTaskController> logger)
        {
            _testTaskService = testTaskService;
            _logger = logger;
        }

        #endregion - Constructors -

        #region - Public methods -

        // GET: testtask/availabletests
        /// <summary>
        /// Gets all the available tests.
        /// </summary>
        /// <returns>
        /// An Array with all available tests
        /// </returns>
        /// <response code="200">Available tests retrieved.</response>
        /// <response code="500">An unexpected error occurs.</response>
        [HttpGet("availabletests", Name = "GetAvailableTests")]
        [ProducesResponseType(typeof(IEnumerable<ApiTest>), 200)]
        [ProducesResponseType(typeof(ApiError), 500)]
        public async Task<IActionResult> GetAvailableTests()
        {
            IEnumerable<ApiTest> apiModels;
            try
            {
                apiModels = await _testTaskService.GetTests();
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "There was an error during the TestTaskController.GetAvailableTests request!");

                return StatusCode(StatusCodes.Status500InternalServerError, ApiError.InternalServerError(new ApiErrorMessage(0, ex.Message)));
            }

            return new OkObjectResult(apiModels ?? new List<ApiTest>());
        }

        // GET: testtask/testbycompilation/{id}
        /// <summary>
        /// Gets the specified test details.
        /// </summary>
        /// <returns>
        /// The requested test details
        /// </returns>
        /// <response code="200">Test retrieved.</response>
        /// <response code="500">An unexpected error occurs.</response>
        [HttpGet("testbycompilation", Name = "GetTestByCompilationId")]
        [ProducesResponseType(typeof(IEnumerable<ApiTest>), 200)]
        [ProducesResponseType(typeof(ApiError), 500)]
        public async Task<IActionResult> GetTestByCompilationId(Guid id)
        {
            ApiTest apiModel;
            try
            {
                apiModel = await _testTaskService.GetTestByCompilationId(id);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "There was an error during the TestTaskController.GetTestByCompilationId request!");

                return StatusCode(StatusCodes.Status500InternalServerError, ApiError.InternalServerError(new ApiErrorMessage(0, ex.Message)));
            }

            return new OkObjectResult(apiModel);
        }

        // GET: testtask/test/{id}/question/{number}
        /// <summary>
        /// Gets the specified test's question.
        /// </summary>
        /// <returns>
        /// The requested question data
        /// </returns>
        /// <response code="200">Question data retrieved.</response>
        /// <response code="500">An unexpected error occurs.</response>
        [HttpGet("test/{id}/question/{number}", Name = "GetQuestion")]
        [ProducesResponseType(typeof(IEnumerable<ApiQuestion>), 200)]
        [ProducesResponseType(typeof(ApiError), 500)]
        public async Task<IActionResult> GetQuestion(Guid id, int number)
        {
            ApiQuestion apiModel;
            try
            {
                apiModel = await _testTaskService.GetQuestion(id, number);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "There was an error during the TestTaskController.GetQuestion request!");

                return StatusCode(StatusCodes.Status500InternalServerError, ApiError.InternalServerError(new ApiErrorMessage(0, ex.Message)));
            }

            return new OkObjectResult(apiModel);
        }

        // GET: testtask/testresults/{id}
        /// <summary>
        /// Gets the specified test compilation results.
        /// </summary>
        /// <returns>
        /// The requested test compilation results data
        /// </returns>
        /// <response code="200">Results data retrieved.</response>
        /// <response code="500">An unexpected error occurs.</response>
        [HttpGet("testresults/{id}", Name = "GetTestCompilationResults")]
        [ProducesResponseType(typeof(IEnumerable<ApiTestCompilationResults>), 200)]
        [ProducesResponseType(typeof(ApiError), 500)]
        public async Task<IActionResult> GetTestCompilationResults(Guid id)
        {
            ApiTestCompilationResults apiModel;
            try
            {
                apiModel = await _testTaskService.GetTestCompilationResults(id);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "There was an error during the TestTaskController.GetTestCompilationResults request!");

                return StatusCode(StatusCodes.Status500InternalServerError, ApiError.InternalServerError(new ApiErrorMessage(0, ex.Message)));
            }

            return new OkObjectResult(apiModel);
        }

        // POST: testtask/newtestcompilation
        /// <summary>
        /// Create a new test compilation for the specified test and person.
        /// </summary>
        /// <param name="newTestCompilation">The test compilation to create.</param>
        /// <response code="201">Test compilation created on the server.</response>
        /// <response code="400">No test compilation specified or not valid.</response>
        /// <response code="500">There was an error creating the test compilation.</response>
        [HttpPost("newtestcompilation", Name = "CreateTestCompilation")]
        [ProducesResponseType(typeof(Guid), 201)]
        [ProducesResponseType(typeof(ApiError), 400)]
        [ProducesResponseType(typeof(ApiError), 404)]
        [ProducesResponseType(typeof(ApiError), 500)]
        public async Task<IActionResult> CreateTestCompilation([FromBody] ApiTestCompilation newTestCompilation)
        {
            // Here we can also validate the model specifications ()
            if (newTestCompilation == null)
            {
                return BadRequest();
            }

            ApiTestCompilation apiModel;
            try
            {
                apiModel = await _testTaskService.CreateTestCompilation(newTestCompilation);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "There was an error during the TestTaskController.CreateTestCompilation request!");

                return StatusCode(StatusCodes.Status500InternalServerError, ApiError.InternalServerError(new ApiErrorMessage(0, ex.Message)));
            }

            if (apiModel == null || !apiModel.Id.HasValue)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return new OkObjectResult(new { id = apiModel.Id.Value });
        }

        // POST: testtask/saveselectedanswer
        /// <summary>
        /// Save the user's answer selection for the specified question.
        /// </summary>
        /// <param name="selAnswer">The question and selected answer to save.</param>
        /// <response code="201">Answer saved on the server.</response>
        /// <response code="400">No Answer specified or not valid.</response>
        /// <response code="500">There was an error saving the Answer.</response>
        [HttpPost("saveselectedanswer", Name = "SaveSelectedQuestionAnswer")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(ApiError), 400)]
        [ProducesResponseType(typeof(ApiError), 404)]
        [ProducesResponseType(typeof(ApiError), 500)]
        public async Task<IActionResult> SaveSelectedQuestionAnswer([FromBody] ApiSelectedQuestionAnswer selAnswer)
        {
            // Here we can also validate the model specifications ()
            if (selAnswer == null)
            {
                return BadRequest();
            }

            var success = false;
            try
            {
                success = await _testTaskService.SaveSelectedQuestionAnswer(selAnswer);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "There was an error during the TestTaskController.SaveSelectedQuestionAnswer request!");

                return StatusCode(StatusCodes.Status500InternalServerError, ApiError.InternalServerError(new ApiErrorMessage(0, ex.Message)));
            }

            return new OkObjectResult(new { success });
        }

        #endregion - Public methods -
    }
}