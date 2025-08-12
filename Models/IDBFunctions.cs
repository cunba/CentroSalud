using System.Text.Json;
using Spectre.Console;

namespace Models
{
    public interface IDBFunctions<T>
    {
        public void Save(string filePath, T t, int id)
        {
            try
            {
                var jsonString = File.ReadAllText(filePath);
                var data = JsonSerializer.Deserialize<Dictionary<int, T>>(jsonString)!;
                data.Add(id, t);

                var options = new JsonSerializerOptions { WriteIndented = true };
                jsonString = JsonSerializer.Serialize(data, options);
                File.WriteAllText(filePath, jsonString);
            }
            catch
            {
                var data = new Dictionary<int, T>();
                data.Add(id, t);


                var options = new JsonSerializerOptions { WriteIndented = true };
                var jsonString = JsonSerializer.Serialize(data, options);
                File.WriteAllText(filePath, jsonString);
            }
        }

        public void Delete(string filePath, int id)
        {
            try
            {
                var jsonString = File.ReadAllText(filePath);
                var data = JsonSerializer.Deserialize<Dictionary<int, T>>(jsonString)!;
                if (!data.TryGetValue(id, out T t))
                {
                    Console.WriteLine("No data found");
                    return;
                }
                data.Remove(id);

                var options = new JsonSerializerOptions { WriteIndented = true };
                jsonString = JsonSerializer.Serialize(data, options);
                File.WriteAllText(filePath, jsonString);
            }
            catch
            {
                Console.WriteLine("Something went wrong");
            }
        }

        public void Update(string filePath, T t, int id)
        {
            try
            {
                var jsonString = File.ReadAllText(filePath);
                var data = JsonSerializer.Deserialize<Dictionary<int, T>>(jsonString)!;
                if (!data.TryGetValue(id, out T oldT))
                {
                    Console.WriteLine("No data found");
                    return;
                }
                else
                {
                    data[id] = t;
                }

                var options = new JsonSerializerOptions { WriteIndented = true };
                jsonString = JsonSerializer.Serialize(data, options);
                File.WriteAllText(filePath, jsonString);
            }
            catch
            {
                Console.WriteLine("Something went wrong");
            }
        }

        public T Get<T>(string filePath, int id) where T : class
        {
            try
            {
                var jsonString = File.ReadAllText(filePath);
                var data = JsonSerializer.Deserialize<Dictionary<int, T>>(jsonString)!;
                if (!data.TryGetValue(id, out T t))
                {
                    Console.WriteLine("No data found");
                    return null;
                }
                else
                {
                    return t;
                }
            }
            catch
            {
                Console.WriteLine("Something went wrong");
                return null;
            }
        }
    }
}