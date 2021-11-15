using System;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace LeagueCalculator.Tools
{
    public static class FileTools
    {
        public static string GetFileStringContents(IFormFile file)
        {
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                return Convert.ToBase64String(fileBytes);
            }
        }
    }
}