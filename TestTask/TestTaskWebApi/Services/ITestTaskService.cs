using TestTaskWebApi.Models;

namespace TestTaskWebApi.Services
{
    /// <summary>
    /// Define the TestTaskService public interface
    /// </summary>
    public interface ITestTaskService
    {
        /// <summary>
        /// Gets all the available tests.
        /// </summary>
        /// <returns>A list of api models</returns>
        Task<IEnumerable<ApiTest>> GetTests();

        /// <summary>
        /// Get the tests details defined by the compilation
        /// </summary>
        /// <param name="compilationId">The compilation identifier</param>
        /// <returns>The tests details</returns>
        Task<ApiTest> GetTestByCompilationId(Guid compilationId);

        /// <summary>
        /// Get the test's question
        /// </summary>
        /// <param name="testId">The test identifier</param>
        /// <param name="number">The question number</param>
        /// <returns>The question details</returns>
        Task<ApiQuestion> GetQuestion(Guid testId, int number);

        /// <summary>
        /// Get the compiled tests results
        /// </summary>
        /// <param name="compilationId">The compilation identifier</param>
        /// <returns>The test results</returns>
        Task<ApiTestCompilationResults> GetTestCompilationResults(Guid compilationId);

        /// <summary>
        /// Create a new Test Compilation for the specified test and person.
        /// </summary>
        /// <returns>the updated Test Compilation object</returns>
        Task<ApiTestCompilation> CreateTestCompilation(ApiTestCompilation newTestCompilation);

        /// <summary>
        /// Save the user's answer selection for the specified question.
        /// </summary>
        Task<bool> SaveSelectedQuestionAnswer(ApiSelectedQuestionAnswer selAnswer);
    }
}
