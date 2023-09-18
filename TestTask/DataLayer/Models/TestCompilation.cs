using Newtonsoft.Json;

namespace DataLayer.Models
{
    /// <summary>
    /// Holds the Test Compilation Details document data
    /// </summary>
    public class TestCompilation
    {
        /// <summary>
        /// Gets or sets the test compilation identifier.
        /// </summary>
        /// <value>
        /// The test compilation identifier.
        /// </value>
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the type of document.
        /// </summary>
        /// <value>
        /// The type of document.
        /// </value>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the test been compiled.
        /// </summary>
        /// <value>
        /// The identifier of the test been compiled.
        /// </value>
        [JsonProperty(PropertyName = "testId")]
        public Guid TestId { get; set; }

        /// <summary>
        /// Gets or sets the name of the person compiling the test.
        /// </summary>
        /// <value>
        /// The name of the person compiling the test.
        /// </value>
        [JsonProperty(PropertyName = "personName")]
        public string PersonName { get; set; }

        /// <summary>
        /// Gets or sets the compilation's answers to test questions.
        /// </summary>
        /// <value>
        /// The compilation's answers to test questions.
        /// </value>
        [JsonProperty(PropertyName = "questionAnswers")]
        public CompilationQuestionAnswer[] QuestionAnswers { get; set; }
    }
}
