using System.Net;
using System.Text.Json.Serialization;

namespace TestTaskWebApi.Models
{
    /// <summary>
    ///  Base class for the Api error types
    /// </summary>
    public class ApiError
    {
        #region - Fields -

        /// <summary>
        /// Gets the status code.
        /// </summary>
        /// <value>
        /// The status code.
        /// </value>
        [JsonPropertyName("status_code")]
        public int StatusCode { get; private set; }

        /// <summary>
        /// Gets the status description.
        /// </summary>
        /// <value>
        /// The status description.
        /// </value>
        [JsonPropertyName("status_description")]
        public string StatusDescription { get; private set; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        [JsonPropertyName("error")]
        public ApiErrorMessage ErrorMessage { get; private set; }

        #endregion - Fields -

        #region - Constructors and initializers -

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiError" /> class.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="statusDescription">The status description.</param>
        public ApiError(int statusCode, string statusDescription) : this(statusCode, statusDescription, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiError" /> class.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="statusDescription">The status description.</param>
        /// <param name="message">The message.</param>
        public ApiError(int statusCode, string statusDescription, ApiErrorMessage message)
        {
            this.StatusCode = statusCode;
            this.StatusDescription = statusDescription;
            this.ErrorMessage = message;
        }

        #endregion - Constructors and initializers -

        #region - Public methods -

        /// <summary>
        /// Custom error for Status code 500 [InternalServerError]
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public static ApiError InternalServerError(ApiErrorMessage message = null)
        {
            return new ApiError((int)HttpStatusCode.InternalServerError, HttpStatusCode.InternalServerError.ToString(), message);
        }

        /// <summary>
        /// Custom error for Status code 404 [NotFound]
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public static ApiError NotFoundError(ApiErrorMessage message = null)
        {
            return new ApiError((int)HttpStatusCode.NotFound, HttpStatusCode.NotFound.ToString(), message);
        }

        /// <summary>
        /// Custom error for Status code 400 [BadRequest]
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public static ApiError BadRequestError(ApiErrorMessage message = null)
        {
            return new ApiError((int)HttpStatusCode.BadRequest, HttpStatusCode.BadRequest.ToString(), message);
        }

        #endregion - Public methods -
    }
}
