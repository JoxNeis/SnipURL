using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using SnipURL.Repositories;

namespace SnipURL.Services
{
    public class StartUpService : IHostedService
    {
        #region FIELDS
        private readonly ShortenedUrlRepository liveRepository;
        private readonly RepositorySerializer serializer;
        #endregion

        #region CONSTRUCTORS
        public StartUpService(ShortenedUrlRepository liveRepository, RepositorySerializer serializer)
        {
            this.liveRepository = liveRepository ?? throw new ArgumentNullException(nameof(liveRepository));
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }
        #endregion

        #region METHODS
        public Task StartAsync(CancellationToken cancellationToken)
        {
            ShortenedUrlRepository savedData = this.serializer.LoadFromDisk();
            foreach (var url in savedData.GetShortenedUrls())
            {
                this.liveRepository.Add(url);
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.serializer.SaveToDisk(this.liveRepository);
            return Task.CompletedTask;
        }
        #endregion
    }
}