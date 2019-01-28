#if UNITY_EDITOR

using System.IO;

namespace UnityExpansionInternal
{
    public class InternalIO
    {
        /// <summary>
        /// Recursively searches file starting from provided directory
        /// </summary>
        /// <param name="path">Directory</param>
        /// <param name="filename">Filename with extension</param>
        /// <returns>Directory that contains target file or null if file is not found</returns>
        public static string FileSearch(string path, string filename)
        {
            string[] files = Directory.GetFiles(path);
            string[] directories = Directory.GetDirectories(path);

            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Contains(filename))
                {
                    return files[i].Substring(0, files[i].Length - filename.Length);
                }
            }

            for (int i = 0; i < directories.Length; i++)
            {
                string result = FileSearch(directories[i], filename);

                if (!string.IsNullOrEmpty(result))
                {
                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Reads and returns all text in provided file
        /// </summary>
        /// <param name="file">Full path of target file</param>
        /// <returns>Text string or null if file is not exists</returns>
        public static string FileRead(string file)
        {
            if (File.Exists(file))
            {
                return File.ReadAllText(file);
            }

            return null;
        }

        public static void FileWrite(string file, string data)
        {
            File.WriteAllText(file, data);
        }

        public static void FileRemove(string file)
        {
            File.Delete(file);
        }
    }
}
#endif