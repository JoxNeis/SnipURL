using System;
using System.Collections.Generic;
using SnipURL.Models;
using SnipURL.Repositories;

namespace SnipURL.Services
{
    public class ShortUrlService
    {
        #region FIELDS
        private readonly ShortenedUrlRepository repository;
        private readonly RepositorySerializer serializer;
        #endregion

        #region CONSTRUCTORS
        public ShortUrlService(ShortenedUrlRepository repository, RepositorySerializer serializer)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }
        #endregion

        #region METHODS
        public async Task<ShortenedUrl> CreateShortUrl(string originalUrl, string shortUrl, int? daysValid = null)
        {
            ShortenedUrl entity = new ShortenedUrl(originalUrl, shortUrl, daysValid);

            this.repository.Add(entity);
            await this.serializer.SaveToDisk(this.repository);

            return entity;
        }

        public ShortenedUrl? GetMapping(string shortUrl)
        {
            return this.repository.GetByShortUrl(shortUrl);
        }

        public List<ShortenedUrl> GetAllUrls()
        {
            return this.repository.GetShortenedUrls();
        }

        public async Task RemoveShortUrl(string shortUrl)
        {
            this.repository.Delete(shortUrl);
            await this.serializer.SaveToDisk(this.repository);
        }
        #endregion
    }
}