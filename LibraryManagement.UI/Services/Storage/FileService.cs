using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace LibraryManagement.UI.Models.Storage
{
    public class FileService : IStorageService
    {
        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _userContentFolder = Path.Combine(webHostEnvironment.WebRootPath);
            _securityContentFolder = Path.Combine($"{webHostEnvironment.ContentRootPath}\\MyStaticFiles");

        }

        private readonly string _userContentFolder;
        private readonly string _securityContentFolder;

        public string GetFileUrl(string fileName, bool security = false)
        {
            return $@"/{fileName}";
        }


        private async Task SaveFileAsync(Stream mediaBinaryStream, string fileName, bool security = false)
        {
            var filePath = security == false ? Path.Combine(_userContentFolder, fileName) : Path.Combine(_securityContentFolder, fileName);
            await using var output = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            await mediaBinaryStream.CopyToAsync(output);
            await output.DisposeAsync();
        }

        public DirectoryInfo CreateDirectory(string path, bool security = false)
        {
            var newPath = security == false ? Path.Combine(_userContentFolder, path) : Path.Combine(_securityContentFolder, path);
            return Directory.CreateDirectory(newPath);
        }

        public void CreateFile(string fileName, bool security = false)
        {
            var newPath = security == false ? Path.Combine(_userContentFolder, fileName) : Path.Combine(_securityContentFolder, fileName);
            File.Create(newPath);
        }


        public bool FolderExists(string path, bool security = false)
        {
            var newPath = security == false ? Path.Combine(_userContentFolder, path) : Path.Combine(_securityContentFolder, path);
            return Directory.Exists(newPath);
        }


        public bool FileExists(string fileName, bool security = false)
        {
            var newPath = security == false ? Path.Combine(_userContentFolder, fileName) : Path.Combine(_securityContentFolder, fileName);
            return File.Exists(newPath);
        }
        public void Move(string sourceDirName, string destDirName, bool security = false)
        {
            sourceDirName = security == false ? Path.Combine(_userContentFolder, sourceDirName) : Path.Combine(_securityContentFolder, sourceDirName);
            destDirName = security == false ? Path.Combine(_userContentFolder, destDirName) : Path.Combine(_securityContentFolder, destDirName);
            Directory.Move(sourceDirName, destDirName);
        }

        public async Task<int> DeleteFileAsync(string fileName, bool security = false)
        {
            var filePath = security == false ? Path.Combine(_userContentFolder, fileName) : Path.Combine(_securityContentFolder, fileName);
            if (!File.Exists(filePath)) return StatusCodes.Status404NotFound;
            try
            {
                await Task.Run(() => File.Delete(filePath));
            }
            catch (Exception)
            {
                return StatusCodes.Status500InternalServerError;
            }
            return StatusCodes.Status200OK;

        }

        public async Task<string> SaveFileAsync(IFormFile file, string path, bool security = false)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim().Value;
            var fileName = $@"{path}/{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            var pathFile = security == false ? Path.Combine(_userContentFolder, fileName) : Path.Combine(_securityContentFolder, fileName);
            await SaveFileAsync(file.OpenReadStream(), pathFile);
            return fileName;
        }

        public async Task<int> DeleteFolderAsync(string folder, bool security = false)
        {
            var folderPath = security == false ? Path.Combine(_userContentFolder, folder) : Path.Combine(_securityContentFolder, folder);

            if (!Directory.Exists(folderPath)) return StatusCodes.Status404NotFound;

            var di = new DirectoryInfo(folderPath);

            foreach (var file in di.GetFiles())
            {
                try
                {
                    await Task.Run(() => file.Delete());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return StatusCodes.Status500InternalServerError;
                }
            }
            foreach (var dir in di.GetDirectories())
            {
                try
                {
                    await Task.Run(() => dir.Delete(true));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return StatusCodes.Status500InternalServerError;
                }
            }
            await Task.Run(() => di.Delete(true));
            return 200;
        }
    }
}
