using DataLayer.Models;

namespace DataLayer
{
    /// <summary>
    /// Define the TestTaskDbService public interface
    /// </summary>
    public interface ITestTaskDbService
    {
        /// <summary>
        /// Get all available tests
        /// </summary>
        /// <returns>The id and name of the availables tests</returns>
        Task<IEnumerable<Test>> GetTests();

        /// <summary>
        /// Get the test details
        /// </summary>
        /// <param name="testId">The test to look</param>
        /// <returns>the test details</returns>
        Task<Test> GetTestDetails(Guid testId);

        /// <summary>
        /// Get the test id used in the specified compilation
        /// </summary>
        /// <param name="compilationId">The compilation</param>
        /// <returns>The test id or null if no compilation found</returns>
        Task<Guid?> GetTestIdByCompilation(Guid compilationId);

        /// <summary>
        /// Get the specified question with all possible answers
        /// </summary>
        /// <param name="testId">The test this question belogs to</param>
        /// <param name="number">The question identifier</param>
        /// <returns>The question with all possible answers</returns>
        Task<Question> GetTestQuestionWithAnswers(Guid testId, int number);

        /// <summary>
        /// Get the specified question's answer
        /// </summary>
        /// <param name="testId">The test this question belogs to</param>
        /// <param name="number">The question identifier</param>
        /// <param name="number">The answer identifier</param>
        /// <returns>The question's answer</returns>
        Task<Answer> GetTestQuestionAnswer(Guid testId, int questionNumber, int answerNumber);

        /// <summary>
        /// Get the specified Test Compilation
        /// </summary>
        /// <param name="compilationId">The compilation</param>
        /// <returns>The test compilation details</returns>
        Task<TestCompilation> GetTestCompilation(Guid compilationId);

        /// <summary>
        /// Create a new person Test compilation
        /// </summary>
        /// <param name="testId">The test to be compiled</param>
        /// <param name="personName">The name of the person compiling the test</param>
        /// <returns>The identification of the newly created compilation</returns>
        Task<Guid> AddTestCompilation(Guid testId, string personName);

        /// <summary>
        /// Add a new selected question answer to the specified compilation
        /// </summary>
        /// <param name="compilationId">The compilation identifier</param>
        /// <param name="questionNumber">The question identifier</param>
        /// <param name="answerNumber">The answer identifier</param>
        /// <returns>true: is operation was successful; false: otherwise</returns>
        Task<bool> SaveSelectedQuestionAnswer(Guid compilationId, int questionNumber, int answerNumber);
    }
}
