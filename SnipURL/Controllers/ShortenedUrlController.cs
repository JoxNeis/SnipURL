using Microsoft.AspNetCore.Mvc;
using SnipURL.Models;
using SnipURL.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SnipURL.Controllers
{
    [ApiController]
    [Route("/entry")]
    public class ShortenedUrlController : ControllerBase
    {
        #region FIELDS
        private readonly ShortUrlService service;
        #endregion

        #region CONSTRUCTORS
        /// <summary>
        /// Initializes the controller by injecting the required business logic service layer.
        /// </summary>
        public ShortenedUrlController(ShortUrlService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }
        #endregion

        #region API ENDPOINTS
        /// <summary>
        /// GET: /entry/{shortUrl} -> Retrieves a single tracking entity by its short string match.
        /// </summary>
        [HttpGet("{shortUrl}")]
        public ActionResult<ShortenedUrl> GetShortenedUrlByShortUrl(string shortUrl)
        {
            ShortenedUrl? mapping = this.service.GetMapping(shortUrl);
            if (mapping == null)
            {
                return NotFound($"No shortened URL found matching: {shortUrl}");
            }
            return Ok(mapping);
        }

        /// <summary>
        /// GET: /entry -> Retrieves all registered shortened URL entities.
        /// </summary>
        [HttpGet]
        public ActionResult<List<ShortenedUrl>> GetShortenedUrls()
        {
            List<ShortenedUrl> urls = this.service.GetAllUrls();
            return Ok(urls);
        }

        /// <summary>
        /// POST: /entry -> Creates a brand-new shortened URL record and commits it to memory and disk.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateNewShortenedUrl([FromBody] ShortenedUrl entry)
        {
            try
            {
                ShortenedUrl result = await this.service.CreateShortUrl(entry.OriginalUrl, entry.ShortUrl);

                return CreatedAtAction(
                    nameof(GetShortenedUrlByShortUrl),
                    new { shortUrl = result.ShortUrl },
                    result
                );
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// DELETE: /entry/{shortUrl} -> Deletes a shortened URL record tracking stack.
        /// </summary>
        [HttpDelete("{shortUrl}")]
        public async Task<IActionResult> DeleteShortenedUrl(string shortUrl)
        {
            try
            {
                await this.service.RemoveShortUrl(shortUrl);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        #endregion
    }
}