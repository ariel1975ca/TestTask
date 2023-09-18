using System.Text.Json.Serialization;

namespace TestTaskWebApi.Models
{
    /// <summary>
    /// Holds the Test's Question data
    /// </summary>
    public class ApiQuestion
    {
        /// <summary>
        /// Gets or sets the question number. [Required]
        /// </summary>
        /// <value>
        /// The question number.
        /// </value>
        [JsonPropertyName("number")]
        public int Number { get; set; }

        /// <summary>
        /// Gets or sets the question name. [Required, Max: 250 chars]
        /// </summary>
        /// <value>
        /// The question name.
        /// </value>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the question number. [Required]
        /// </summary>
        /// <value>
        /// The question number.
        /// </value>
        [JsonPropertyName("answers")]
        public ApiQuestionAnswer[] Answers { get; set; }
    }
}
