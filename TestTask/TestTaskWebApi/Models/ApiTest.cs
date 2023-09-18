using System.Text.Json.Serialization;

namespace TestTaskWebApi.Models
{
    /// <summary>
    /// Holds the Test data
    /// </summary>
    public class ApiTest
    {
        /// <summary>
        /// Gets or sets the test identifier. [Required]
        /// </summary>
        /// <value>
        /// The test identifier.
        /// </value>
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the test name. [Required, Max: 250 chars]
        /// </summary>
        /// <value>
        /// The test name.
        /// </value>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the test's number of questions.
        /// </summary>
        /// <value>
        /// The test's number of questions.
        /// </value>
        [JsonPropertyName("number_questions")]
        public int? NumberQuestions { get; set; }
    }
}
