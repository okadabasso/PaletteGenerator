using PaletteGenerator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaletteGenerator.Services
{
    public class WpfResourceGenerator
    {
        private string templateString = @"
<ResourceDictionary xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
                   >
<!-- primary -->
{{ i = 0 }}
{{ for color in primary_colors }}   <Color x:Key=""ColorPrimary_{{ i }}"" >{{ color.background_rgb }}</Color>
{{ i = i + 1 }}{{ end }}
<!-- complemental -->
{{ i = 0 }}
{{ for color in complemental_colors }}   <Color x:Key=""ColorComplemental_{{ i }}"" >{{ color.background_rgb }}</Color>
{{ i = i + 1 }}{{ end }}
<!-- analogous 1 -->
{{ i = 0 }}
{{ for color in analogous_colors1 }}   <Color x:Key=""ColorAnalogous1_{{ i }}"" >{{ color.background_rgb }}</Color>
{{ i = i + 1 }}{{ end }}
<!-- analogous 2 -->
{{ i = 0 }}
{{ for color in analogous_colors2 }}   <Color x:Key=""ColorAnalogous2_{{ i }}"" >{{ color.background_rgb }}</Color>
{{ i = i + 1 }}{{ end }}
<!-- triadic 1 -->
{{ i = 0 }}
{{ for color in triadic_colors1 }}   <Color x:Key=""ColorTriadic1_{{ i }}"" >{{ color.background_rgb }}</Color>
{{ i = i + 1 }}{{ end }}
<!-- triadic 2 -->
{{ i = 0 }}
{{ for color in triadic_colors2 }}   <Color x:Key=""ColorTriadic2_{{ i }}"" >{{ color.background_rgb }}</Color>
{{ i = i + 1 }}{{ end }}
<!-- variant 1 -->
{{ i = 0 }}
{{ for color in variant_colors1 }}   <Color x:Key=""ColorVariant1_{{ i }}"" >{{ color.background_rgb }}</Color>
{{ i = i + 1 }}{{ end }}
<!-- variant 2 -->
{{ i = 0 }}
{{ for color in variant_colors2 }}   <Color x:Key=""ColorVariant2_{{ i }}"" >{{ color.background_rgb }}</Color>
{{ i = i + 1 }}{{ end }}
<!-- warning -->
{{ i = 0 }}
{{ for color in warning_colors }}   <Color x:Key=""ColorWarning_{{ i }}"" >{{ color.background_rgb }}</Color>
{{ i = i + 1 }}{{ end }}
<!-- error -->
{{ i = 0 }}
{{ for color in error_colors }}   <Color x:Key=""ColorError_{{ i }}"" >{{ color.background_rgb }}</Color>
{{ i = i + 1 }}{{ end }}
<!-- gray scale -->
{{ i = 0 }}
{{ for color in gray_scale }}   <Color x:Key=""ColorGray_{{ i }}"" >{{ color.background_rgb }}</Color>
{{ i = i + 1 }}{{ end }}
</ResourceDictionary>
";

        public string Generate(
            IEnumerable<ColorListItem> primaryColors,
            IEnumerable<ColorListItem> complementalColors,
            IEnumerable<ColorListItem> analogousColors1,
            IEnumerable<ColorListItem> analogousColors2,
            IEnumerable<ColorListItem> triadicColors1,
            IEnumerable<ColorListItem> triadicColors2,
            IEnumerable<ColorListItem> variantColors1,
            IEnumerable<ColorListItem> variantColors2,
            IEnumerable<ColorListItem> warningColors,
            IEnumerable<ColorListItem> errorColors,
            IEnumerable<ColorListItem> grayScale
            )
        {
            var template = Scriban.Template.Parse(templateString);

            // パース済みのテンプレートに対して変数を埋め込んで文字列を出力する
            var rendered = template.Render(new
            {
                PrimaryColors = primaryColors,
                complementalColors = complementalColors,
                analogousColors1 = analogousColors1,
                analogousColors2 = analogousColors2,
                triadicColors1 = triadicColors1,
                triadicColors2 = triadicColors2,
                variantColors1 = variantColors1,
                variantColors2 = variantColors2,
                warningColors = warningColors,
                errorColors = errorColors,
                grayScale = grayScale,
            });
            return rendered;
        }
    }
}
