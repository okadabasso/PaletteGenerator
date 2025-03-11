using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalletSample;
class CreatePalletSample
{
    public void Create()
    {
        using (var writer = new StreamWriter("sample.html"))
        {
            writer.WriteLine("<div>");
            for (var hue = 0; hue < 360; hue += 10)
            {
                writer.WriteLine(@"<div style=""margin-top: 16px;"">");
                writer.Write($@"<span style=""display:inline-block; padding: 2px;width:96px;"" >{hue}</span>");
                var steps = new int[] { 100, 200, 300, 400, 500, 600, 700, 800, 900 };
                double minL = AdjustLightness(hue);  // L の最小値（900の暗さ）
                for (var step = 0; step < steps.Length; step++)
                {
                    var background = HslColor.ToRgb(GetRedShade(hue, steps[step]));
                    var contrast1 = Contrast(Color.Black, background);
                    var contrast2 = Contrast(Color.White, background);


                    var foreground = contrast1 <= contrast2 ? Color.White : Color.Black;
                    writer.WriteLine($@" <span style=""background-color:{ColorToHex(background)};color:{ColorToHex(foreground)}; display:inline-block; padding: 2px;width:96px;"" >{ColorToHex(background)}</span>");

                }
                writer.WriteLine("</div>");
            }
            writer.WriteLine("</div>");

        }

    }
    string ColorToHex(Color color)
    {
        return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
    }
    HslColor GetRedShade(float hue, int step)
    {
        double minL = AdjustLightness(hue);
        double maxL = 95;  // L の最大値（100の明るさ）

        // 3/4, 1/1.2, 1/1.07 で試す
        double lightness = maxL - (maxL - minL) * Math.Pow((double)step / 900, 1 / 1.2);

        double minS = 30; // 最小彩度（暗い色ほど低彩度）
        double maxS = 97; // 最大彩度（明るい色ほど高彩度）

        double saturation = maxS - (maxS - minS) * Math.Pow((double)step / 900, 1.2);

        return new HslColor(hue, (float)saturation / 100, (float)lightness / 100);
    }
    double AdjustLightness(double hue)
    {
        var value = 1.0 - 0.5 * Math.Cos((hue - 60) * Math.PI / 180);
        if (value < 0) value = 0;
        if (value > 1) value = 1;
        return value * 15;
    }

    double Contrast(Color color1, Color color2)
    {
        var l1 = Luminance(color1);
        var l2 = Luminance(color2);
        return l1 > l2 ? (l1 + 0.05) / (l2 + 0.05) : (l2 + 0.05) / (l1 + 0.05);
    }
    double Luminance(Color color)
    {
        Func<double, double> linear = v => (v / 255) < 0.003928 ? (v / 255) / 12.92 : Math.Pow((((v / 255) + 0.055) / 1.055), 2.4F);

        var luminanceR = linear((double)color.R);
        var luminanceG = linear((double)color.G);
        var luminanceB = linear((double)color.B);

        var l = 0.2126 * luminanceR + 0.7152 * luminanceG + 0.0722 * luminanceB;
        return l;

    }

}
