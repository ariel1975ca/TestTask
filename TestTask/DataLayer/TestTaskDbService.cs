using DataLayer.Models;

using Microsoft.Azure.Cosmos;

namespace DataLayer
{
    /// <summary>
    /// Handle all the database interactions
    /// </summary>
    public class TestTaskDbService: ITestTaskDbService
    {
        #region - Constants and enumerations -

        const string TestDocumentType = "test";
        const string CompilationDocumentType = "compilation";

        #endregion - Constants and enumerations -

        #region - Properties -

        /// <summary>
        /// The Test Task Cosmos Db client
        /// </summary>
        private readonly CosmosClient _dbClient;

        /// <summary>
        /// The Test task container
        /// </summary>
        private readonly Container _testsContainer;

        #endregion - Properties -

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="TestTaskDbService"/> class.
        /// </summary>
        /// <param name="cosmosDbClient">The Test Task Cosmos Db client.</param>
        /// <param name="logger">The logger.</param>
        public TestTaskDbService(CosmosClient cosmosDbClient, string databaseName, string containerName)
        {
            _dbClient = cosmosDbClient;
            _testsContainer = cosmosDbClient.GetContainer(databaseName, containerName);
        }

        #endregion - Constructors -

        #region - Public methods -

        /// <summary>
        /// Get all available tests
        /// </summary>
        /// <returns>The id and name of the availables tests</returns>
        public async Task<IEnumerable<Test>> GetTests()
        {
            return await GetMultipleAsync<Test>($"SELECT c.id, c.name FROM c WHERE c.type = '{TestDocumentType}' ORDER BY c.name");
        }

        /// <summary>
        /// Get the test details
        /// </summary>
        /// <param name="testId">The test to look</param>
        /// <returns>the test details</returns>
        public async Task<Test> GetTestDetails(Guid testId)
        {
            try
            {
                var response = await _testsContainer.ReadItemAsync<Test>(testId.ToString(), new PartitionKey(TestDocumentType));
                return response.Resource;
            }
            catch (CosmosException)
            {
                //we can handle item not found and other exceptions

                return null;
            }
        }

        /// <summary>
        /// Get the test id used in the specified compilation
        /// </summary>
        /// <param name="compilationId">The compilation</param>
        /// <returns>The test id or null if no compilation found</returns>
        public async Task<Guid?> GetTestIdByCompilation(Guid compilationId)
        {
            try
            {
                var response = await _testsContainer.ReadItemAsync<TestCompilation>(compilationId.ToString(), new PartitionKey(CompilationDocumentType));
                return response.Resource.TestId;
            }
            catch (CosmosException) 
            {
                //we can handle item not found and other exceptions

                return null;
            }
        }

        /// <summary>
        /// Get the specified question with all possible answers
        /// </summary>
        /// <param name="testId">The test this question belogs to</param>
        /// <param name="number">The question identifier</param>
        /// <returns>The question with all possible answers</returns>
        public async Task<Question> GetTestQuestionWithAnswers(Guid testId, int number)
        {
            var questions = await GetMultipleAsync<Question>($"SELECT q.number, q.name, q.answers FROM c as t JOIN q IN t.questions Where t.type = '{TestDocumentType}' and t.id = '{testId}' and q.number = {number}");

            return questions.SingleOrDefault();
        }

        /// <summary>
        /// Get the specified question's answer
        /// </summary>
        /// <param name="testId">The test this question belogs to</param>
        /// <param name="number">The question identifier</param>
        /// <param name="number">The answer identifier</param>
        /// <returns>The question's answer</returns>
        public async Task<Answer> GetTestQuestionAnswer(Guid testId, int questionNumber, int answerNumber)
        {
            var questions = await GetMultipleAsync<Answer>($"SELECT a.number, a.name, a.isCorrect FROM c as t JOIN q IN t.questions JOIN a in q.answers Where t.type = '{TestDocumentType}' and t.id = '{testId}' and q.number = {questionNumber} and a.number = {answerNumber}");

            return questions.SingleOrDefault();
        }

        /// <summary>
        /// Get the specified Test Compilation
        /// </summary>
        /// <param name="compilationId">The compilation</param>
        /// <returns>The test compilation details</returns>
        public async Task<TestCompilation> GetTestCompilation(Guid compilationId)
        {
            try
            {
                var response = await _testsContainer.ReadItemAsync<TestCompilation>(compilationId.ToString(), new PartitionKey(CompilationDocumentType));
                return response.Resource;
            }
            catch (CosmosException)
            {
                //we can handle item not found and other exceptions

                return null;
            }
        }

        /// <summary>
        /// Create a new person Test compilation
        /// </summary>
        /// <param name="testId">The test to be compiled</param>
        /// <param name="personName">The name of the person compiling the test</param>
        /// <returns>The identification of the newly created compilation</returns>
        public async Task<Guid> AddTestCompilation(Guid testId, string personName)
        {
            var id = Guid.NewGuid();
            var compilation = new TestCompilation
            {
                Id = id,
                TestId = testId,
                PersonName = personName,
                Type = CompilationDocumentType
            };

            var response = await _testsContainer.CreateItemAsync(compilation, new PartitionKey(compilation.Type));

            return id;
        }

        /// <summary>
        /// Add a new selected question answer to the specified compilation
        /// </summary>
        /// <param name="compilationId">The compilation identifier</param>
        /// <param name="questionNumber">The question identifier</param>
        /// <param name="answerNumber">The answer identifier</param>
        /// <returns>true: is operation was successful; false: otherwise</returns>
        public async Task<bool> SaveSelectedQuestionAnswer(Guid compilationId, int questionNumber, int answerNumber)
        {
            var compilation = await GetTestCompilation(compilationId);

            if (compilation == null) return false;

            var answer = await GetTestQuestionAnswer(compilation.TestId, questionNumber, answerNumber);

            if (answer == null) return false;

            var answers = new List<CompilationQuestionAnswer>(compilation.QuestionAnswers ?? Array.Empty<CompilationQuestionAnswer>());

            answers.Add(new CompilationQuestionAnswer { QuestionNumber = questionNumber, AnswerNumber = answerNumber, IsCorrect = answer.IsCorrect });

            compilation.QuestionAnswers = answers.ToArray();

            var response = await _testsContainer.ReplaceItemAsync(compilation, compilation.Id.ToString(), new PartitionKey(CompilationDocumentType));

            return response.Resource != null;
        }

        #endregion - Public methods -

        #region - Private methods -

        /// <summary>
        /// handle a multi-document result query
        /// </summary>
        /// <typeparam name="T">The deserialized type of the expected document</typeparam>
        /// <param name="queryString">The sql like query string</param>
        /// <returns>The list of all matching documents as T</returns>
        private async Task<IEnumerable<T>> GetMultipleAsync<T>(string queryString)
        {
            var query = _testsContainer.GetItemQueryIterator<T>(new QueryDefinition(queryString));
            var results = new List<T>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }

        #endregion - Private methods -
    }
}