using Newtonsoft.Json;

namespace DataLayer.Models
{
    /// <summary>
    /// Holds the Test Compilation Question selected Answer data
    /// </summary>
    public class CompilationQuestionAnswer
    {
        /// <summary>
        /// Gets or sets the question number.
        /// </summary>
        /// <value>
        /// The question number.
        /// </value>
        [JsonProperty(PropertyName = "questionNumber")]
        public int QuestionNumber { get; set; }

        /// <summary>
        /// Gets or sets the selected answer number for this question.
        /// </summary>
        /// <value>
        /// The selected answer number for this question.
        /// </value>
        [JsonProperty(PropertyName = "answerNumber")]
        public int AnswerNumber { get; set; }

        /// <summary>
        /// Gets or sets if this is the correct answer for the question.
        /// </summary>
        /// <value>
        /// true: if this is the correct answer for the question, false: otherwise
        /// </value>
        [JsonProperty(PropertyName = "isCorrect")]
        public bool IsCorrect { get; set; }
    }
}
