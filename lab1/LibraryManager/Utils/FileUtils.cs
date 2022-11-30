using System.Text.Json;

namespace LibraryManager.Utils
{
    public static class FileUtils
    {
        public static List<T> ReadFromFile<T>(string filename)
        {
            using (StreamReader r = new StreamReader(filename))
            {
                string json = r.ReadToEnd();
                var result = JsonSerializer.Deserialize<List<T>>(json);
                return result;
            }
        }

        public static void WriteToFile<T>(string filename, List<T> data)
        {
            string jsonString = JsonSerializer.Serialize(data, new JsonSerializerOptions() { WriteIndented = true });
            using (StreamWriter outputFile = new StreamWriter(filename))
            {
                outputFile.WriteLine(jsonString);
            }
        }

    }
}
