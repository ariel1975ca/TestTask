using System.Text.Json.Serialization;

namespace TestTaskWebApi.Models
{
    /// <summary>
    /// Holds the Question's answer data
    /// </summary>
    public class ApiQuestionAnswer
    {
        /// <summary>
        /// Gets or sets the answer number. [Required]
        /// </summary>
        /// <value>
        /// The answer number.
        /// </value>
        [JsonPropertyName("number")]
        public int Number { get; set; }

        /// <summary>
        /// Gets or sets the answer name. [Required, Max: 250 chars]
        /// </summary>
        /// <value>
        /// The answer name.
        /// </value>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets if the answer is the correct answer for the question. [Required]
        /// </summary>
        /// <value>
        /// true if this is the question's correct answer; false otherwise.
        /// </value>
        [JsonPropertyName("is_correct")]
        public bool IsCorrect { get; set; }
    }
}
