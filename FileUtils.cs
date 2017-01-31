using System.IO;

namespace GenericSettingsLoader
{
    public static class FileUtils
    {
        public static void MakeSurePath(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path); 
        }
    }
}
