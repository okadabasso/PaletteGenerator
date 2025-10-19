namespace PalletSample
{
    using System.IO;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Humanizer;

    public class CreateStyleCss
    {
        public void Create()
        {
            var colorDictionary = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(File.ReadAllText("colors.json"));
            if (colorDictionary == null)
            {
                return;
            }
            if(!Directory.Exists("html"))
            {
                Directory.CreateDirectory("html");
            }
            using (var writer = new StreamWriter("html/styles.css"))
            {
                writer.WriteLine(":root {");

                foreach (var entry in colorDictionary)
                {
                    var name = entry.Key;
                    writer.WriteLine("    /* color " + name + " */");
                    foreach (var color in entry.Value)
                    {
                        var colorName = name + color.Key;
                        writer.WriteLine($"    --color-{colorName}: {color.Value};");
                    }
                    writer.WriteLine();
                }

                writer.WriteLine("}");
            }
        }
    }
}