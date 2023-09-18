using System.Text.Json.Serialization;

namespace TestTaskWebApi.Models
{
    /// <summary>
    /// Holds the Selected Anwser for a Question
    /// </summary>
    public class ApiSelectedQuestionAnswer
    {
        /// <summary>
        /// Gets or sets the identifier of the test compilation the question belogs to. [Required]
        /// </summary>
        /// <value>
        /// The identifier of the test compilation the question belogs to.
        /// </value>
        [JsonPropertyName("test_compilation_id")]
        public Guid TestCompilationId { get; set; }

        /// <summary>
        /// Gets or sets the question identifier. [Required]
        /// </summary>
        /// <value>
        /// The question identifier.
        /// </value>
        [JsonPropertyName("question_number")]
        public int QuestionNumber { get; set; }

        /// <summary>
        /// Gets or sets the question answer identifier. [Required]
        /// </summary>
        /// <value>
        /// The question answer identifier.
        /// </value>
        [JsonPropertyName("answer_number")]
        public int QuestionAnswerNumber { get; set; }
    }
}
