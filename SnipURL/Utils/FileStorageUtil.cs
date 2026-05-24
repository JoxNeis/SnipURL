using System;
using System.IO;

namespace SnipURL.Utils
{
    /// <summary>
    /// Utility class responsible for managing file system pathing, 
    /// tracking existence, and reading/writing raw text data to disk.
    /// </summary>
    public class FileStorageUtil
    {
        #region FIELDS
        private readonly string _path = "SnipUrl"; 
        private readonly string directoryPath;
        private readonly string fileName;
        #endregion

        #region CONSTRUCTORS
        /// <summary>
        /// Initializes a new instance of the <see cref="FileStorageUtil"/> class, 
        /// configuring the application storage path inside the local app data folder.
        /// </summary>
        public FileStorageUtil()
        {
            string systemAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            this.fileName = $"{this._path}.json";
            this.directoryPath = Path.Combine(systemAppData, this._path);
        }
        #endregion

        #region PROPERTIES
        /// <summary>
        /// Gets the absolute string path where the JSON storage file resides on the file system.
        /// </summary>
        public string FilePath
        {
            get
            {
                return Path.Combine(this.directoryPath, this.fileName);
            }
        }
        #endregion

        #region METHODS
        /// <summary>
        /// Ensures that the destination directory exists on disk before read/write operations.
        /// </summary>
        private void EnsureDirectoryExists()
        {
            if (!Directory.Exists(this.directoryPath))
            {
                Directory.CreateDirectory(this.directoryPath);
            }
        }

        /// <summary>
        /// Validates whether the designated JSON backup file exists on disk.
        /// </summary>
        /// <returns>True if the file exists; otherwise, false.</returns>
        public bool CheckFileExist()
        {
            if (!File.Exists(this.FilePath))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Safely commits a JSON text string down to the storage file pathway.
        /// </summary>
        /// <param name="jsonString">The raw JSON string payload to write.</param>
        public async Task SaveFile(string jsonString)
        {
            this.EnsureDirectoryExists();
            await File.WriteAllTextAsync(this.FilePath, jsonString);
        }

        /// <summary>
        /// Reads and extracts the entire raw text contents of the storage file.
        /// </summary>
        /// <returns>A string containing the file's entire textual payload.</returns>
        public string ReadFile()
        {
            return File.ReadAllText(this.FilePath);
        }
        #endregion
    }
}