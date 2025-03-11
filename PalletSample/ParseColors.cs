using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Drawing;

namespace PalletSample;
class ParseColors
{
    public void Execute()
    {
        var colors = Parse();
        if (colors == null)
        {
            Console.WriteLine("Failed to parse colors.json");
            return;
        }
        foreach(var item in colors)
        {
            Console.WriteLine($"Name: {item.Name}");
            foreach (var color in item.ColorValues)
            {
                var colorValue = ColorTranslator.FromHtml(color.Code);
                var hslColor = HslColor.FromRgb(colorValue);
                Console.WriteLine($"{color.Number}\t{color.Code}\t{hslColor.H}\t{hslColor.S}\t{hslColor.L}");
            }
        }
    }
    public List<ColorPallete> Parse()
    {
        using (var reader = new StreamReader("colors.json"))
        {
            string json = reader.ReadToEnd();
            var colorDictionary = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(json);
            if(colorDictionary == null)
            {
                return null;
            }
            var colors = new List<ColorPallete>();

            foreach (var color in colorDictionary)
            {
                var item = new ColorPallete
                {
                    Name = color.Key
                };
                foreach (var colorValue in color.Value)
                {
                    item.ColorValues.Add(new ColorValue
                    {
                        Number = colorValue.Key,
                        Code = colorValue.Value
                    });
                }
                colors.Add(item);
            }

            return colors;
        }
    }
}

public class ColorPallete
{
    public string Name { get; set; } = "";
    public List<ColorValue> ColorValues { get; set; } = new List<ColorValue>();
}
public class ColorValue
{
    public string Number { get; set; } = "";
    public string Code { get; set; } = "";
}
