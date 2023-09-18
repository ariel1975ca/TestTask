using Newtonsoft.Json;

namespace DataLayer.Models
{
    /// <summary>
    /// Holds the Question Details inside a test document data
    /// </summary>
    public class Question
    {
        /// <summary>
        /// Gets or sets the question number.
        /// </summary>
        /// <value>
        /// The question number.
        /// </value>
        [JsonProperty(PropertyName = "number")]
        public int Number { get; set; }

        /// <summary>
        /// Gets or sets the question name.
        /// </summary>
        /// <value>
        /// The question name.
        /// </value>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the questions's answers.
        /// </summary>
        /// <value>
        /// The questions's answers.
        /// </value>
        [JsonProperty(PropertyName = "answers")]
        public Answer[] Answers { get; set; }
    }
}
