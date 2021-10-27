using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LibraryManagement.UI.Models.Storage
{
    public interface IStorageService
    {
        public string GetFileUrl(string fileName, bool security = false);
        /// <summary>
        /// Lưu file từ <see cref="IFormFile"/>
        /// </summary>
        /// <param name="file">File cần lưu</param>
        /// <param name="path">Đường dẫn sẽ lưu</param>
        /// <param name="security">
        ///     <para>false: sử dụng thư mục wwwroot</para>
        ///     <para>true: sử dụng thư mục MyStaticfile</para>
        /// </param>
        /// <returns>Trả về đường dẫn của file vừa lưu</returns>
        public Task<string> SaveFileAsync(IFormFile file, string path, bool security = false);

        /// <summary>
        /// Tạo Thư mục mới
        /// </summary>
        /// <param name="path">Đường dẫn</param>
        /// <param name="security">
        ///     <para>false: sử dụng thư mục wwwroot</para>
        ///     <para>true: sử dụng thư mục MyStaticfile</para>
        /// </param>
        /// <returns></returns>
        public DirectoryInfo CreateDirectory(string path, bool security = false);

        /// <summary>
        /// <para>Tạo File mới</para>
        /// </summary>
        /// <param name="fileName">Tên File</param>
        /// <param name="security">
        ///     <para>false: sử dụng thư mục wwwroot</para>
        ///     <para>true: sử dụng thư mục MyStaticfile</para>
        /// </param>
        public void CreateFile(string fileName, bool security = false);
        /// <summary>
        /// <para>Kiểm tra sự tồn tại cảu folder</para>
        /// </summary>
        /// <param name="path"></param>
        /// <param name="security">
        ///     <para>false: sử dụng thư mục wwwroot</para>
        ///     <para>true: sử dụng thư mục MyStaticfile</para>
        /// </param>
        /// <returns><para>Trả về kiểu bool</para></returns>
        public bool FolderExists(string path, bool security = false);

        /// <summary>
        ///     <para>Kiểm tra sự tồn tại của file</para>
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="security">
        ///     <para>false: sử dụng thư mục wwwroot</para>
        ///     <para>true: sử dụng thư mục MyStaticfile</para>
        /// </param>
        /// <returns><para>Trả về kiểu bool</para></returns>
        public bool FileExists(string path, bool security = false);
        /// <summary>
        /// <para>Di chuyển một file hoặc một folder từ <see cref="sourceDirName"/> đến <see cref="destDirName"/></para>
        /// <para>Có thể sử dụng để đổi tên file khi <see cref="sourceDirName"/> và <see cref="destDirName"/> giống nhau</para>
        /// </summary>
        /// <param name="sourceDirName">Đường dẫn gốc</param>
        /// <param name="destDirName">Đường dẫn cần chuyển qua</param>
        /// <param name="security">
        ///     <para>false: sử dụng thư mục wwwroot</para>
        ///     <para>true: sử dụng thư mục MyStaticfile</para>
        /// </param>
        public void Move(string sourceDirName, string destDirName, bool security = false);
        public Task<int> DeleteFileAsync(string fileName, bool security = false);
        public Task<int> DeleteFolderAsync(string folder, bool security = false);
    }
}
