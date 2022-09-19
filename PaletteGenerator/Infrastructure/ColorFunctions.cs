using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace PaletteGenerator.Infrastructure
{
    public static class ColorFunctions
    {
        public static double Contrast(Color color1, Color color2)
        {
            var l1 = Luminance(color1);
            var l2 = Luminance(color2);
            return l1 > l2 ? (l1 + 0.05) / (l2 + 0.05) : (l2 + 0.05) / (l1 + 0.05);
        }
        public static double Luminance(Color color)
        {
            Func<double, double> linear = v => (v / 255) < 0.003928 ? (v / 255) / 12.92 : Math.Pow((((v / 255) + 0.055) / 1.055), 2.4F);

            var luminanceR = linear((double)color.R);
            var luminanceG = linear((double)color.G);
            var luminanceB = linear((double)color.B);

            var l = 0.2126 * luminanceR + 0.7152 * luminanceG + 0.0722 * luminanceB;
            return l;

        }
    }
}
