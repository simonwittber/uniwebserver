using System;
using System.IO;

namespace UniWebServer
{
    public static class Helper
    {
        public static string GetFolderRoot(string folderPath)
        {
            string folderName = Path.GetDirectoryName(folderPath);
            if (folderName.Length <= 2) return folderName;
            var idx = folderName.IndexOf("/", 1, StringComparison.InvariantCulture);
            return idx != -1 ? folderName.Substring(0, idx) : folderName;
        }
    }
}
