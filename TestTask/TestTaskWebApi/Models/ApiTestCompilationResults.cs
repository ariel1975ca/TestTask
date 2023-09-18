using System.Text.Json.Serialization;

namespace TestTaskWebApi.Models
{
    /// <summary>
    /// Holds the Test Compilation result data
    /// </summary>
    public class ApiTestCompilationResults: ApiTestCompilation
    {
        /// <summary>
        /// Gets or sets the number of correct answers in the compilation. [Required]
        /// </summary>
        /// <value>
        /// The number of correct answers.
        /// </value>
        [JsonPropertyName("correct_answers")]
        public int CorrectAnswers { get; set; }

        /// <summary>
        /// Gets or sets the compiled test's number of questions.
        /// </summary>
        /// <value>
        /// The compiled test's number of questions.
        /// </value>
        [JsonPropertyName("number_questions")]
        public int NumberQuestions { get; set; }
    }
}
