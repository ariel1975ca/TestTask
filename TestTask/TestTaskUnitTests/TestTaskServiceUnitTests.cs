using DataLayer;
using DataLayer.Models;

using Microsoft.Extensions.Logging;

using Moq;

using TestTaskWebApi.Models;
using TestTaskWebApi.Services;

namespace TestTaskUnitTests
{
    [TestClass]
    public class TestTaskServiceUnitTests
    {
        #region - Properties -

        private Mock<ITestTaskDbService> _mockTestTaskDbService;
        private Mock<ILogger<TestTaskService>> _mockLogger;

        private TestTaskService service;

        #endregion - Properties -

        #region - Test Initialize -

        [TestInitialize]
        public void TestInitializationMethod()
        {
            _mockTestTaskDbService = new Mock<ITestTaskDbService>();
            _mockLogger = new Mock<ILogger<TestTaskService>>();

            service = new TestTaskService(_mockTestTaskDbService.Object, _mockLogger.Object);
        }

        #endregion - Test Initialize -

        #region - Test Methods -

        [TestMethod]
        [Description("GetTests - Ok")]
        public async Task GetTests_Ok()
        {
            var tests = new[] {
                new Test { Id = Guid.NewGuid(), Name = "test 1", Type = "test"  },
                new Test { Id = Guid.NewGuid(), Name = "test 1", Type = "test"  }
            };

            _mockTestTaskDbService.Setup(c => c.GetTests()).Returns(Task.FromResult(tests.Cast<Test>()));

            var result = await service.GetTests();

            Assert.IsNotNull(result);
            _mockTestTaskDbService.Verify(c => c.GetTests(), Times.Once);
        }

        [TestMethod]
        [Description("GetTestByCompilationId - Ok")]
        public async Task GetTestByCompilationId_Ok()
        {
            Guid compilationId = Guid.NewGuid();
            Guid? testId = Guid.NewGuid();

            var test = new Test { Id = testId.Value, Name = "test 1", Type = "test" };

            _mockTestTaskDbService.Setup(c => c.GetTestIdByCompilation(It.IsAny<Guid>())).Returns(Task.FromResult(testId));
            _mockTestTaskDbService.Setup(c => c.GetTestDetails(It.IsAny<Guid>())).Returns(Task.FromResult(test));

            var result = await service.GetTestByCompilationId(compilationId);

            Assert.IsNotNull(result);
            _mockTestTaskDbService.Verify(c => c.GetTestIdByCompilation(It.IsAny<Guid>()), Times.Once);
            _mockTestTaskDbService.Verify(c => c.GetTestDetails(It.IsAny<Guid>()), Times.Once);
            Assert.AreEqual(test.Id, result.Id);
            Assert.AreEqual(test.Name, result.Name);
        }

        [TestMethod]
        [Description("GetTestByCompilationId - Should Throw")]
        public async Task GetTestByCompilationId_Error()
        {
            Guid compilationId = Guid.NewGuid();
            Guid? testId = Guid.NewGuid();

            _mockTestTaskDbService.Setup(c => c.GetTestIdByCompilation(It.IsAny<Guid>())).Returns(Task.FromResult((Guid?)null));

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () => await service.GetTestByCompilationId(compilationId));

            _mockTestTaskDbService.Verify(c => c.GetTestIdByCompilation(It.IsAny<Guid>()), Times.Once);

            _mockTestTaskDbService.Setup(c => c.GetTestIdByCompilation(It.IsAny<Guid>())).Returns(Task.FromResult(testId));
            _mockTestTaskDbService.Setup(c => c.GetTestDetails(It.IsAny<Guid>())).Returns(Task.FromResult((Test)null));

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () => await service.GetTestByCompilationId(compilationId));

            _mockTestTaskDbService.Verify(c => c.GetTestDetails(It.IsAny<Guid>()), Times.Once);
        }

        [TestMethod]
        [Description("GetQuestion - Ok")]
        public async Task GetQuestion_Ok()
        {
            Guid testId = Guid.NewGuid();
            int number = 1;

            var question = new Question { Number = number, Name = "question 1" };

            _mockTestTaskDbService.Setup(c => c.GetTestQuestionWithAnswers(It.IsAny<Guid>(), It.IsAny<int>())).Returns(Task.FromResult(question));

            var result = await service.GetQuestion(testId, number);

            Assert.IsNotNull(result);
            _mockTestTaskDbService.Verify(c => c.GetTestQuestionWithAnswers(It.IsAny<Guid>(), It.IsAny<int>()), Times.Once);
            Assert.AreEqual(question.Number, result.Number);
            Assert.AreEqual(question.Name, result.Name);
        }

        [TestMethod]
        [Description("GetQuestion - Should Throw")]
        public async Task GetQuestion_Error()
        {
            Guid testId = Guid.NewGuid();
            int number = 1;

            _mockTestTaskDbService.Setup(c => c.GetTestQuestionWithAnswers(It.IsAny<Guid>(), It.IsAny<int>())).Returns(Task.FromResult((Question)null));

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () => await service.GetQuestion(testId, number));

            _mockTestTaskDbService.Verify(c => c.GetTestQuestionWithAnswers(It.IsAny<Guid>(), It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        [Description("GetTestCompilationResults - Ok")]
        public async Task GetTestCompilationResults_Ok()
        {
            Guid compilationId = Guid.NewGuid();

            var compilation = new TestCompilation
            {
                Id = compilationId,
                TestId = Guid.NewGuid(),
                PersonName = "person 1",
                QuestionAnswers = new CompilationQuestionAnswer[]
                {
                    new CompilationQuestionAnswer{ QuestionNumber = 1, AnswerNumber = 1, IsCorrect = true },
                    new CompilationQuestionAnswer{ QuestionNumber = 2, AnswerNumber = 1, IsCorrect = false }
                }
            };

            _mockTestTaskDbService.Setup(c => c.GetTestCompilation(It.IsAny<Guid>())).Returns(Task.FromResult(compilation));

            var result = await service.GetTestCompilationResults(compilationId);

            Assert.IsNotNull(result);
            _mockTestTaskDbService.Verify(c => c.GetTestCompilation(It.IsAny<Guid>()), Times.Once);
            Assert.AreEqual(compilation.Id, result.Id);
            Assert.AreEqual(compilation.TestId, result.TestId);
            Assert.AreEqual(compilation.PersonName, result.PersonName);
            Assert.AreEqual(2, result.NumberQuestions);
            Assert.AreEqual(1, result.CorrectAnswers);
        }

        [TestMethod]
        [Description("GetTestCompilationResults - Should Throw")]
        public async Task GetTestCompilationResults_Error()
        {
            Guid compilationId = Guid.NewGuid();

            _mockTestTaskDbService.Setup(c => c.GetTestCompilation(It.IsAny<Guid>())).Returns(Task.FromResult((TestCompilation)null));

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () => await service.GetTestCompilationResults(compilationId));

            _mockTestTaskDbService.Verify(c => c.GetTestCompilation(It.IsAny<Guid>()), Times.Once);
        }

        [TestMethod]
        [Description("CreateTestCompilation - Ok")]
        public async Task CreateTestCompilation_Ok()
        {
            Guid compilationId = Guid.NewGuid();

            var compilation = new ApiTestCompilation
            {
                TestId = Guid.NewGuid(),
                PersonName = "person 1"
            };

            _mockTestTaskDbService.Setup(c => c.AddTestCompilation(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.FromResult(compilationId));

            var result = await service.CreateTestCompilation(compilation);

            Assert.IsNotNull(result);
            _mockTestTaskDbService.Verify(c => c.AddTestCompilation(It.IsAny<Guid>(), It.IsAny<string>()), Times.Once);
            Assert.AreEqual(compilationId, result.Id);
        }

        [TestMethod]
        [Description("SaveSelectedQuestionAnswer - Ok")]
        public async Task SaveSelectedQuestionAnswer_Ok()
        {
            Guid compilationId = Guid.NewGuid();

            var compilation = new ApiSelectedQuestionAnswer
            {
                TestCompilationId = compilationId,
                QuestionNumber = 1,
                QuestionAnswerNumber = 1
            };

            _mockTestTaskDbService.Setup(c => c.SaveSelectedQuestionAnswer(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(true));

            var result = await service.SaveSelectedQuestionAnswer(compilation);

            Assert.IsNotNull(result);
            _mockTestTaskDbService.Verify(c => c.SaveSelectedQuestionAnswer(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            Assert.IsTrue(result);
        }

        [TestMethod]
        [Description("SaveSelectedQuestionAnswer - Should Throw")]
        public async Task SaveSelectedQuestionAnswer_Error()
        {
            Guid compilationId = Guid.NewGuid();

            var compilation = new ApiSelectedQuestionAnswer
            {
                TestCompilationId = compilationId,
                QuestionNumber = 1,
                QuestionAnswerNumber = 1
            };

            _mockTestTaskDbService.Setup(c => c.SaveSelectedQuestionAnswer(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(false));

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () => await service.SaveSelectedQuestionAnswer(compilation));

            _mockTestTaskDbService.Verify(c => c.SaveSelectedQuestionAnswer(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        #endregion - Test Methods -
    }
}