using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Moq;

using TestTaskWebApi.Controllers;
using TestTaskWebApi.Models;
using TestTaskWebApi.Services;

namespace TestTaskUnitTests
{
    [TestClass]
    public class TestTaskControllerUnitTests
    {
        #region - Properties -

        private Mock<ITestTaskService> _mockTestTaskService;
        private Mock<ILogger<TestTaskController>> _mockLogger;

        private TestTaskController controller;

        #endregion - Properties -

        #region - Test Initialize -

        [TestInitialize]
        public void TestInitializationMethod()
        {
            _mockTestTaskService = new Mock<ITestTaskService>();
            _mockLogger = new Mock<ILogger<TestTaskController>>();

            controller = new TestTaskController(_mockTestTaskService.Object, _mockLogger.Object);
        }

        #endregion - Test Initialize -

        #region - Test Methods -

        [TestMethod]
        [Description("GetAvailableTests - Ok")]
        public async Task GetAvailableTests_Ok()
        {
            var tests = new[] {
                new ApiTest { Id = Guid.NewGuid(), Name = "test 1", NumberQuestions = 3  },
                new ApiTest { Id = Guid.NewGuid(), Name = "test 1", NumberQuestions = 5  }
            };

            _mockTestTaskService.Setup(c => c.GetTests()).Returns(Task.FromResult(tests.Cast<ApiTest>()));

            var result = await controller.GetAvailableTests();

            Assert.IsNotNull(result);
            _mockTestTaskService.Verify(c => c.GetTests(), Times.Once);
            Assert.IsTrue(result is OkObjectResult);
            Assert.IsTrue(((OkObjectResult)result).StatusCode == StatusCodes.Status200OK);
            Assert.IsTrue(((OkObjectResult)result).Value is IEnumerable<ApiTest>);
            Assert.IsTrue(((IEnumerable<ApiTest>)((OkObjectResult)result).Value).Count() == tests.Length);
        }

        [TestMethod]
        [Description("GetAvailableTests - Should Throw")]
        public async Task GetAvailableTests_Error()
        {
            Guid compilationId = Guid.NewGuid();
            Guid? testId = Guid.NewGuid();

            _mockTestTaskService.Setup(c => c.GetTests()).Throws(new Exception());

            var result = await controller.GetAvailableTests();

            _mockTestTaskService.Verify(c => c.GetTests(), Times.Once);
            Assert.IsTrue(result is ObjectResult);
            Assert.IsTrue(((ObjectResult)result).StatusCode == StatusCodes.Status500InternalServerError);
        }

        [TestMethod]
        [Description("GetTestByCompilationId - Ok")]
        public async Task GetTestByCompilationId_Ok()
        {
            Guid compilationId = Guid.NewGuid();

            var test = new ApiTest { Id = Guid.NewGuid(), Name = "test 1", NumberQuestions = 3 };

            _mockTestTaskService.Setup(c => c.GetTestByCompilationId(It.IsAny<Guid>())).Returns(Task.FromResult(test));

            var result = await controller.GetTestByCompilationId(compilationId);

            Assert.IsNotNull(result);
            _mockTestTaskService.Verify(c => c.GetTestByCompilationId(It.IsAny<Guid>()), Times.Once);
            Assert.IsTrue(result is OkObjectResult);
            Assert.IsTrue(((OkObjectResult)result).StatusCode == StatusCodes.Status200OK);
            Assert.IsTrue(((OkObjectResult)result).Value is ApiTest);
            Assert.IsTrue(((ApiTest)((OkObjectResult)result).Value).Id == test.Id);
        }

        [TestMethod]
        [Description("GetTestByCompilationId - Should Throw")]
        public async Task GetTestByCompilationId_Error()
        {
            Guid compilationId = Guid.NewGuid();

            _mockTestTaskService.Setup(c => c.GetTestByCompilationId(It.IsAny<Guid>())).Throws(new Exception());

            var result = await controller.GetTestByCompilationId(compilationId);

            _mockTestTaskService.Verify(c => c.GetTestByCompilationId(It.IsAny<Guid>()), Times.Once);
            Assert.IsTrue(result is ObjectResult);
            Assert.IsTrue(((ObjectResult)result).StatusCode == StatusCodes.Status500InternalServerError);
        }

        [TestMethod]
        [Description("GetQuestion - Ok")]
        public async Task GetQuestion_Ok()
        {
            Guid testId = Guid.NewGuid();
            int number = 1;

            var question = new ApiQuestion { Number = number, Name = "question 1" };

            _mockTestTaskService.Setup(c => c.GetQuestion(It.IsAny<Guid>(), It.IsAny<int>())).Returns(Task.FromResult(question));

            var result = await controller.GetQuestion(testId, number);

            Assert.IsNotNull(result);
            _mockTestTaskService.Verify(c => c.GetQuestion(It.IsAny<Guid>(), It.IsAny<int>()), Times.Once);
            Assert.IsTrue(result is OkObjectResult);
            Assert.IsTrue(((OkObjectResult)result).StatusCode == StatusCodes.Status200OK);
            Assert.IsTrue(((OkObjectResult)result).Value is ApiQuestion);
            Assert.IsTrue(((ApiQuestion)((OkObjectResult)result).Value).Number == number);
        }

        [TestMethod]
        [Description("GetQuestion - Should Throw")]
        public async Task GetQuestion_Error()
        {
            Guid testId = Guid.NewGuid();
            int number = 1;

            _mockTestTaskService.Setup(c => c.GetQuestion(It.IsAny<Guid>(), It.IsAny<int>())).Throws(new Exception());

            var result = await controller.GetQuestion(testId, number);

            _mockTestTaskService.Verify(c => c.GetQuestion(It.IsAny<Guid>(), It.IsAny<int>()), Times.Once);
            Assert.IsTrue(result is ObjectResult);
            Assert.IsTrue(((ObjectResult)result).StatusCode == StatusCodes.Status500InternalServerError);
        }

        [TestMethod]
        [Description("GetTestCompilationResults - Ok")]
        public async Task GetTestCompilationResults_Ok()
        {
            Guid compilationId = Guid.NewGuid();
            Guid testId = Guid.NewGuid();

            var compilation = new ApiTestCompilationResults { Id = compilationId, TestId = testId, PersonName = "person 1", CorrectAnswers = 1, NumberQuestions = 3 };

            _mockTestTaskService.Setup(c => c.GetTestCompilationResults(It.IsAny<Guid>())).Returns(Task.FromResult(compilation));

            var result = await controller.GetTestCompilationResults(compilationId);

            Assert.IsNotNull(result);
            _mockTestTaskService.Verify(c => c.GetTestCompilationResults(It.IsAny<Guid>()), Times.Once);
            Assert.IsTrue(result is OkObjectResult);
            Assert.IsTrue(((OkObjectResult)result).StatusCode == StatusCodes.Status200OK);
            Assert.IsTrue(((OkObjectResult)result).Value is ApiTestCompilationResults);
            Assert.IsTrue(((ApiTestCompilationResults)((OkObjectResult)result).Value).Id == compilationId);
        }

        [TestMethod]
        [Description("GetTestCompilationResults - Should Throw")]
        public async Task GetTestCompilationResults_Error()
        {
            Guid compilationId = Guid.NewGuid();

            _mockTestTaskService.Setup(c => c.GetTestCompilationResults(It.IsAny<Guid>())).Throws(new Exception());

            var result = await controller.GetTestCompilationResults(compilationId);

            _mockTestTaskService.Verify(c => c.GetTestCompilationResults(It.IsAny<Guid>()), Times.Once);
            Assert.IsTrue(result is ObjectResult);
            Assert.IsTrue(((ObjectResult)result).StatusCode == StatusCodes.Status500InternalServerError);
        }

        [TestMethod]
        [Description("CreateTestCompilation - Ok")]
        public async Task CreateTestCompilation_Ok()
        {
            Guid compilationId = Guid.NewGuid();
            Guid testId = Guid.NewGuid();

            var newTestCompilation = new ApiTestCompilation { Id = compilationId, TestId = testId, PersonName = "person 1" };

            _mockTestTaskService.Setup(c => c.CreateTestCompilation(It.IsAny<ApiTestCompilation>())).Returns(Task.FromResult(newTestCompilation));

            var result = await controller.CreateTestCompilation(newTestCompilation);

            Assert.IsNotNull(result);
            _mockTestTaskService.Verify(c => c.CreateTestCompilation(It.IsAny<ApiTestCompilation>()), Times.Once);
            Assert.IsTrue(result is OkObjectResult);
            Assert.IsTrue(((OkObjectResult)result).StatusCode == StatusCodes.Status200OK);
            Assert.IsTrue(((OkObjectResult)result).Value  != null);
        }

        [TestMethod]
        [Description("CreateTestCompilation - Should Throw")]
        public async Task CreateTestCompilation_Error()
        {
            Guid compilationId = Guid.NewGuid();
            Guid testId = Guid.NewGuid();

            var newTestCompilation = new ApiTestCompilation { Id = compilationId, TestId = testId, PersonName = "person 1" };

            _mockTestTaskService.Setup(c => c.CreateTestCompilation(It.IsAny<ApiTestCompilation>())).Throws(new Exception());

            var result = await controller.CreateTestCompilation(newTestCompilation);

            _mockTestTaskService.Verify(c => c.CreateTestCompilation(It.IsAny<ApiTestCompilation>()), Times.Once);
            Assert.IsTrue(result is ObjectResult);
            Assert.IsTrue(((ObjectResult)result).StatusCode == StatusCodes.Status500InternalServerError);

            result = await controller.CreateTestCompilation(null);

            Assert.IsTrue(result is BadRequestResult);
        }

        [TestMethod]
        [Description("SaveSelectedQuestionAnswer - Ok")]
        public async Task SaveSelectedQuestionAnswer_Ok()
        {
            Guid compilationId = Guid.NewGuid();

            var answer = new ApiSelectedQuestionAnswer { TestCompilationId = compilationId, QuestionNumber = 1, QuestionAnswerNumber = 1 };

            _mockTestTaskService.Setup(c => c.SaveSelectedQuestionAnswer(It.IsAny<ApiSelectedQuestionAnswer>())).Returns(Task.FromResult(true));

            var result = await controller.SaveSelectedQuestionAnswer(answer);

            Assert.IsNotNull(result);
            _mockTestTaskService.Verify(c => c.SaveSelectedQuestionAnswer(It.IsAny<ApiSelectedQuestionAnswer>()), Times.Once);
            Assert.IsTrue(result is OkObjectResult);
            Assert.IsTrue(((OkObjectResult)result).StatusCode == StatusCodes.Status200OK);
            Assert.IsTrue(((OkObjectResult)result).Value != null);
        }

        [TestMethod]
        [Description("SaveSelectedQuestionAnswer - Should Throw")]
        public async Task SaveSelectedQuestionAnswer_Error()
        {
            Guid compilationId = Guid.NewGuid();
            Guid testId = Guid.NewGuid();

            var answer = new ApiSelectedQuestionAnswer { TestCompilationId = compilationId, QuestionNumber = 1, QuestionAnswerNumber = 1 };

            _mockTestTaskService.Setup(c => c.SaveSelectedQuestionAnswer(It.IsAny<ApiSelectedQuestionAnswer>())).Throws(new Exception());

            var result = await controller.SaveSelectedQuestionAnswer(answer);

            _mockTestTaskService.Verify(c => c.SaveSelectedQuestionAnswer(It.IsAny<ApiSelectedQuestionAnswer>()), Times.Once);
            Assert.IsTrue(result is ObjectResult);
            Assert.IsTrue(((ObjectResult)result).StatusCode == StatusCodes.Status500InternalServerError);

            result = await controller.SaveSelectedQuestionAnswer(null);

            Assert.IsTrue(result is BadRequestResult);
        }

        #endregion - Test Methods -
    }
}
