using Newtonsoft.Json;

namespace DataLayer.Models
{
    /// <summary>
    /// Holds the Question one Answer Details for a test document data
    /// </summary>
    public class Answer
    {
        /// <summary>
        /// Gets or sets the answer number.
        /// </summary>
        /// <value>
        /// The answer number.
        /// </value>
        [JsonProperty(PropertyName = "number")]
        public int Number { get; set; }

        /// <summary>
        /// Gets or sets the answer name.
        /// </summary>
        /// <value>
        /// The answer name.
        /// </value>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

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
