using Microsoft.AspNetCore.Mvc;
using SnipURL.Models;
using SnipURL.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SnipURL.Controllers
{
    [ApiController]
    [Route("/")]
    public class ServiceController : ControllerBase
    {
        #region FIELDS
        private readonly ShortUrlService service;
        #endregion

        #region CONSTRUCTORS
        /// <summary>
        /// Initializes the controller by injecting the required business logic service layer.
        /// </summary>
        public ServiceController(ShortUrlService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }
        #endregion

        #region API ENDPOINTS
        /// <summary>
        /// GET: /{shortUrl} -> Evaluates link existence, checks lifetime expiration thresholds, 
        /// and routes a standard 302 Found browser redirection command to the target destination.
        /// </summary>
        /// <param name="shortUrl">The unique path segment matching the stored entity database lookup key.</param>
        /// <returns>An HTTP 302 Redirect to the original URL if valid; otherwise, a 404 NotFound or 400 BadRequest.</returns>
        [HttpGet("{shortUrl}")]
        public ActionResult RedirectUrl(string shortUrl)
        {
            ShortenedUrl? mapping = this.service.GetMapping(shortUrl);
            if (mapping == null)
            {
                return NotFound($"The shortened URL link matching '{shortUrl}' does not exist.");
            }
            if (!mapping.IsValid)
            {
                return BadRequest("This shortened URL link has passed its expiration threshold and is no longer valid.");
            }
            return Redirect(mapping.OriginalUrl);
        }
        #endregion
    }
}