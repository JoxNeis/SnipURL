using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SnipURL.Models
{
    public class ShortenedUrl
    {
        #region FIELDS
        private string originalUrl = string.Empty;
        private string shortUrl = string.Empty;
        private DateTime createdAt;
        private DateTime? expiresAt;
        #endregion

        #region CONSTRUCTORS
        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public ShortenedUrl()
        {
            this.createdAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public ShortenedUrl(string originalUrl, string shortUrl, int? daysValid = null)
        {
            this.OriginalUrl = originalUrl;
            this.ShortUrl = shortUrl;
            this.createdAt = DateTime.UtcNow;

            if (daysValid.HasValue)
            {
                this.expiresAt = DateTime.UtcNow.AddDays(daysValid.Value);
            }
        }
        #endregion

        #region PROPERTIES
        [Required]
        public string OriginalUrl
        {
            get => this.originalUrl;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Original URL cannot be empty.", nameof(value));
                }
                this.originalUrl = value;
            }
        }

        [Required]
        public string ShortUrl
        {
            get => this.shortUrl;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Short URL/Code cannot be empty.", nameof(value));
                }
                this.shortUrl = value;
            }
        }

        public DateTime CreatedAt
        {
            get => this.createdAt;
            set => this.createdAt = value;
        }

        public DateTime? ExpiresAt
        {
            get => this.expiresAt;
            set => this.expiresAt = value;
        }

        [JsonInclude]
        public bool IsValid
        {
            get => this.checkUrlValidity();
        }
        #endregion

        #region METHODS
        private bool checkUrlValidity()
        {
            return !this.expiresAt.HasValue || this.expiresAt.Value > DateTime.UtcNow;
        }
        #endregion
    }
}