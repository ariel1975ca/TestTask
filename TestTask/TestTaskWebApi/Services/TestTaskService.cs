using DataLayer;
using DataLayer.Models;

using TestTaskWebApi.Models;

namespace TestTaskWebApi.Services
{
    /// <summary>
    /// handles all the Task Tests CRUD operations
    /// </summary>
    public class TestTaskService: ITestTaskService
    {
        #region - Properties -

        /// <summary>
        /// The database service
        /// </summary>
        private readonly ITestTaskDbService _dbService;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<TestTaskService> _logger;

        #endregion - Properties -

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="TestTaskService"/> class.
        /// </summary>
        /// <param name="dbService">The database service.</param>
        /// <param name="logger">The logger.</param>
        public TestTaskService(ITestTaskDbService dbService, ILogger<TestTaskService> logger)
        {
            _dbService = dbService ?? throw new ArgumentNullException(nameof(dbService)); ;
            _logger = logger;
        }

        #endregion - Constructors -

        #region - Public methods -

        /// <summary>
        /// Gets all the available tests.
        /// </summary>
        /// <returns>A list of api models</returns>
        public async Task<IEnumerable<ApiTest>> GetTests()
        {
            var tests = await _dbService.GetTests();
            return tests.Select(x => GetApiTest(x));
        }

        /// <summary>
        /// Get the tests details defined by the compilation
        /// </summary>
        /// <param name="compilationId">The compilation identifier</param>
        /// <returns>The tests details</returns>
        public async Task<ApiTest> GetTestByCompilationId(Guid compilationId) 
        {
            var testId = await _dbService.GetTestIdByCompilation(compilationId);

            if (!testId.HasValue)
            {
                throw new InvalidOperationException("No Compilation was found with the specified id!");
            }

            var test = await _dbService.GetTestDetails(testId.Value);

            if (test == null) 
            {
                throw new InvalidOperationException("No Test details found for the specified compilation!");
            }

            return GetApiTest(test);
        }

        /// <summary>
        /// Get the test's question
        /// </summary>
        /// <param name="testId">The test identifier</param>
        /// <param name="number">The question number</param>
        /// <returns>The question details</returns>
        public async Task<ApiQuestion> GetQuestion(Guid testId, int number)
        {
            var question = await _dbService.GetTestQuestionWithAnswers(testId, number);

            if (question == null)
            {
                throw new InvalidOperationException("No question found for the specified test!");
            }

            return GetApiQuestion(question);
        }

        /// <summary>
        /// Get the compiled tests results
        /// </summary>
        /// <param name="compilationId">The compilation identifier</param>
        /// <returns>The test results</returns>
        public async Task<ApiTestCompilationResults> GetTestCompilationResults(Guid compilationId)
        {
            var compilation = await _dbService.GetTestCompilation(compilationId);

            if (compilation == null)
            {
                throw new InvalidOperationException("No test compilation found!");
            }

            return GetApiTestCompilationResults(compilation);
        }

        /// <summary>
        /// Create a new Test Compilation for the specified test and person.
        /// </summary>
        /// <returns>the updated Test Compilation object</returns>
        public async Task<ApiTestCompilation> CreateTestCompilation(ApiTestCompilation newTestCompilation)
        {
            newTestCompilation.Id = await _dbService.AddTestCompilation(newTestCompilation.TestId, newTestCompilation.PersonName);

            return newTestCompilation;
        }

        /// <summary>
        /// Save the user's answer selection for the specified question.
        /// </summary>
        public async Task<bool> SaveSelectedQuestionAnswer(ApiSelectedQuestionAnswer selAnswer)
        {
            var success = await _dbService.SaveSelectedQuestionAnswer(selAnswer.TestCompilationId, selAnswer.QuestionNumber, selAnswer.QuestionAnswerNumber);

            if (!success)
            {
                throw new InvalidOperationException("There was an error saving the answer!");
            }

            return success;
        }

        #endregion - Public methods -

        #region - Private methods -

        /// <summary>
        /// Convert a Test db model to a ApiTest model response
        /// </summary>
        /// <param name="test">The db test document</param>
        /// <returns>The equivalent Api model</returns>
        private ApiTest GetApiTest(Test test)
        {
            return new ApiTest
            {
                Id = test.Id,
                Name = test.Name,
                NumberQuestions = test.Questions?.Length ?? 0
            };
        }

        /// <summary>
        /// Convert a Question db model to a ApiQuestion model response
        /// </summary>
        /// <param name="question">The db question document part</param>
        /// <returns>The equivalent Api model</returns>
        private ApiQuestion GetApiQuestion(Question question)
        {
            return new ApiQuestion
            {
                Number = question.Number,
                Name = question.Name,
                Answers = question.Answers?.Length > 0 ? question.Answers.OrderBy(x => x.Number).Select(x => GetApiQuestionAnswer(x)).ToArray() : Array.Empty<ApiQuestionAnswer>()
            };
        }

        /// <summary>
        /// Convert a Answer db model to a ApiQuestionAnswer model response
        /// </summary>
        /// <param name="answer">The db answer document part</param>
        /// <returns>The equivalent Api model</returns>
        private ApiQuestionAnswer GetApiQuestionAnswer(Answer answer)
        {
            return new ApiQuestionAnswer
            {
                Number = answer.Number,
                Name = answer.Name,
                IsCorrect = answer.IsCorrect
            };
        }

        /// <summary>
        /// Convert a Test Compilation db model to a ApiTestCompilationResults model response
        /// </summary>
        /// <param name="compilation">The db Test Compilation document part</param>
        /// <returns>The equivalent Api model</returns>
        private ApiTestCompilationResults GetApiTestCompilationResults(TestCompilation compilation)
        {
            return new ApiTestCompilationResults
            {
                Id = compilation.Id,
                TestId = compilation.TestId,
                PersonName = compilation.PersonName,
                NumberQuestions = compilation.QuestionAnswers?.Length ?? 0,
                CorrectAnswers = (compilation.QuestionAnswers?.Length > 0) ? compilation.QuestionAnswers.Count(x => x.IsCorrect) : 0
            };
        }

        #endregion - Private methods -
    }
}
