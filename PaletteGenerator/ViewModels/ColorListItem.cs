using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace PaletteGenerator.ViewModels
{
    public class ColorListItem
    {
        public SolidColorBrush Background { get; set; }
        public SolidColorBrush Foreground { get; set; }

        public string BackgroundRgb
        {
            get
            {
                return FormatColor(Background.Color);
            }
        }
        public string ForegroundRgb
        {
            get
            {
                return FormatColor(Foreground.Color);
            }
        }
        private string FormatColor(Color color)
        {
            return color.R.ToString("x2") + color.G.ToString("x2") + color.B.ToString("x2");

        }
    }
}
