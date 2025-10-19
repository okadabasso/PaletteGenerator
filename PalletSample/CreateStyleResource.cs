namespace PalletSample;

using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Humanizer;
public class CreateStyleResource
{
    public void Create()
    {

        var colorDictionary = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(File.ReadAllText("colors.json"));
        if(colorDictionary == null)
        {
            return;
        }

        using (var writer = new StreamWriter("Color.xaml"))
        {
            writer.WriteLine("<ResourceDictionary xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"");
            writer.WriteLine("                    xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\">");

            foreach(var entry in colorDictionary)
            {
                var name =  entry.Key.Replace('-', '_').Replace('-', '_').Pascalize();
                writer.WriteLine("    <!-- color " + name + " -->");
                foreach(var color in entry.Value)
                {
                    var colorName = name + color.Key;
                    writer.WriteLine($"    <Color x:Key=\"Color_{colorName}\">{color.Value}</Color>");
                }
                writer.WriteLine();
            }
            writer.WriteLine();
            foreach(var entry in colorDictionary)
            {
                var name =  entry.Key.Replace('-', '_').Pascalize();
                writer.WriteLine("    <!-- solid color brush " + name + " -->");
                foreach(var color in entry.Value)
                {
                    var colorName = name + color.Key;
                    writer.WriteLine($"    <SolidColorBrush x:Key=\"{colorName}\" Color=\"{{StaticResource Color_{colorName} }}\" />");
                }
                writer.WriteLine();
            }

            writer.WriteLine("</ResourceDictionary>");
        }
    }
}