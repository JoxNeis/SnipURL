using System;
using System.Collections.Generic;
using System.IO;
using SnipURL.Models;

namespace SnipURL.Repositories
{
    public class ShortenedUrlRepository
    {
        #region FIELDS
        private Dictionary<string, ShortenedUrl> data;
        #endregion

        #region CONSTRUCTORS
        public ShortenedUrlRepository()
        {
            this.data = new Dictionary<string, ShortenedUrl>();
        }
        #endregion

        #region METHODS

        /// <summary>
        /// Starts up the repository with serialized files.
        /// </summary>
        public void StartUp(List<ShortenedUrl> data)
        {
            foreach (ShortenedUrl shortUrl in data)
            {
                this.Add(shortUrl);
            }
        }

        /// <summary>
        /// Saves a new shortened URL mapping.
        /// </summary>
        public void Add(ShortenedUrl shortenedUrl)
        {
            if (shortenedUrl == null)
            {
                throw new ArgumentNullException(nameof(shortenedUrl));
            }

            if (!this.data.ContainsKey(shortenedUrl.ShortUrl))
            {
                this.data.Add(shortenedUrl.ShortUrl, shortenedUrl);
            }
            else
            {
                throw new ArgumentException("A tracking entry for this short URL already exists.");
            }
        }

        /// <summary>
        /// Retrieves a shortened URL entity by its short code/url.
        /// Returns null if not found.
        /// </summary>
        public ShortenedUrl? GetByShortUrl(string shortUrl)
        {
            if (this.data.TryGetValue(shortUrl, out ShortenedUrl? urlData))
            {
                return urlData;
            }
            return null;
        }

        /// <summary>
        /// Retrieves all shortened URL entities stored in memory.
        /// </summary>
        public List<ShortenedUrl> GetShortenedUrls()
        {
            return new List<ShortenedUrl>(this.data.Values);
        }

        /// <summary>
        /// Updates the destination/original URL for an existing short link.
        /// </summary>
        public void Update(string shortUrl, string newOriginalUrl)
        {
            if (string.IsNullOrWhiteSpace(shortUrl))
            {
                throw new ArgumentException("Short URL cannot be empty.", nameof(shortUrl));
            }

            if (this.data.TryGetValue(shortUrl, out ShortenedUrl? urlData))
            {
                // Updating via the public property ensures property validation triggers
                urlData.OriginalUrl = newOriginalUrl;
            }
            else
            {
                throw new KeyNotFoundException($"No short URL found matching '{shortUrl}' to update.");
            }
        }

        /// <summary>
        /// Extends the expiration date of a specific short URL by a set number of days.
        /// </summary>
        public void ExtendExpiryDate(string shortUrl, int daysToExtend)
        {
            if (daysToExtend <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(daysToExtend), "Extension days must be greater than zero.");
            }
            if (this.data.TryGetValue(shortUrl, out ShortenedUrl? urlData))
            {
                DateTime baseDate = urlData.ExpiresAt ?? DateTime.UtcNow;
                urlData.ExpiresAt = baseDate.AddDays(daysToExtend);
            }
            else
            {
                throw new KeyNotFoundException($"No short URL found matching '{shortUrl}' to extend.");
            }
        }

        /// <summary>
        /// Deletes a specific short URL tracking entry.
        /// </summary>
        public void Delete(string shortUrl)
        {
            if (this.data.ContainsKey(shortUrl))
            {
                this.data.Remove(shortUrl);
            }
            else
            {
                throw new KeyNotFoundException($"No short URL found matching '{shortUrl}' to delete.");
            }
        }

        /// <summary>
        /// Clears all tracking records from the memory stack.
        /// </summary>
        public void DeleteAll()
        {
            this.data.Clear();
        }
        #endregion
    }
}