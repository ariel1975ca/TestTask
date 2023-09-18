using Newtonsoft.Json;

namespace DataLayer.Models
{
    /// <summary>
    /// Holds the Test Details document data
    /// </summary>
    public class Test
    {
        /// <summary>
        /// Gets or sets the test identifier.
        /// </summary>
        /// <value>
        /// The test identifier.
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
        /// Gets or sets the test name.
        /// </summary>
        /// <value>
        /// The test name.
        /// </value>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the test's questions.
        /// </summary>
        /// <value>
        /// The test's questions.
        /// </value>
        [JsonProperty(PropertyName = "questions")]
        public Question[] Questions { get; set; }
    }
}
