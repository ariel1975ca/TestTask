using System.Text.Json.Serialization;

namespace TestTaskWebApi.Models
{
    /// <summary>
    /// Holds the Test Compilation data
    /// </summary>
    public class ApiTestCompilation
    {
        /// <summary>
        /// Gets or sets the test compilation identifier. [Required]
        /// </summary>
        /// <value>
        /// The test compilation identifier.
        /// </value>
        [JsonPropertyName("id")]
        public Guid? Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the test been compiled. [Required]
        /// </summary>
        /// <value>
        /// The identifier of the test been compiled.
        /// </value>
        [JsonPropertyName("test_id")]
        public Guid TestId { get; set; }

        /// <summary>
        /// Gets or sets the name of the person compiling the test. [Required, Max: 250 chars]
        /// </summary>
        /// <value>
        /// The name of the person compiling the test.
        /// </value>
        [JsonPropertyName("person_name")]
        public string PersonName { get; set; }
    }
}
