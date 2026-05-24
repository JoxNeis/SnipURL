using System;
using System.Collections.Generic;
using System.Text.Json;
using SnipURL.Models;
using SnipURL.Utils;

namespace SnipURL.Repositories
{
    /// <summary>
    /// Handles data translation responsibilities by transforming running repository data 
    /// into structured text payloads and back using JSON schema configurations.
    /// </summary>
    public class RepositorySerializer
    {
        #region FIELDS
        private readonly FileStorageUtil fileStorageUtil;
        private readonly JsonSerializerOptions jsonOptions;
        #endregion

        #region CONSTRUCTORS
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositorySerializer"/> class 
        /// utilizing the specified file engine configuration.
        /// </summary>
        /// <param name="fileStorageUtil">The file helper infrastructure instance.</param>
        /// <exception cref="ArgumentNullException">Thrown if the provided file storage utility is null.</exception>
        public RepositorySerializer(FileStorageUtil fileStorageUtil)
        {
            this.fileStorageUtil = fileStorageUtil ?? throw new ArgumentNullException(nameof(fileStorageUtil));
            this.jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };
        }
        #endregion

        #region METHODS
        /// <summary>
        /// Serializes the current data stack from the repository directly to disk.
        /// </summary>
        /// <param name="repository">The live running repository containing mapping data.</param>
        /// <exception cref="ArgumentNullException">Thrown if the input repository parameter is null.</exception>
        public async Task SaveToDisk(ShortenedUrlRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }
            List<ShortenedUrl> currentData = repository.GetShortenedUrls();
            string jsonString = JsonSerializer.Serialize(currentData, this.jsonOptions);
            await this.fileStorageUtil.SaveFile(jsonString);
        }

        /// <summary>
        /// Deserializes a saved JSON storage file back into a clean repository instance.
        /// </summary>
        /// <returns>A populated or empty instance of <see cref="ShortenedUrlRepository"/>.</returns>
        public ShortenedUrlRepository LoadFromDisk()
        {
            ShortenedUrlRepository repository = new ShortenedUrlRepository();
            try
            {
                if (!this.fileStorageUtil.CheckFileExist())
                {
                    return repository;
                }
                string jsonString = this.fileStorageUtil.ReadFile();
                List<ShortenedUrl>? deserializedList = JsonSerializer.Deserialize<List<ShortenedUrl>>(jsonString, this.jsonOptions);
                if (deserializedList != null)
                {
                    foreach (ShortenedUrl url in deserializedList)
                    {
                        repository.Add(url);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to load repository file:\n {ex.Message}");
            }
            return repository;
        }
        #endregion
    }
}