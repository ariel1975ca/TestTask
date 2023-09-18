using System.Text.Json.Serialization;

namespace TestTaskWebApi.Models
{
    /// <summary>
    /// An structured Error response
    /// </summary>
    public class ApiErrorMessage
    {
        /// <summary>
        /// Gets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [JsonPropertyName("code")]
        public int Code { get; private set; }

        /// <summary>
        /// Gets the status description.
        /// </summary>
        /// <value>
        /// The status description.
        /// </value>
        [JsonPropertyName("message")]
        public string Message { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiErrorMessage"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="message">The message.</param>
        public ApiErrorMessage(int code, string message)
        {
            this.Code = code;
            this.Message = message;
        }
    }
}
