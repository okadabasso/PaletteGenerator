using PaletteGenerator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaletteGenerator.Services
{
    public class CssGenerator
    {
        private string templateString = @"
/* primary */
{{ i = 0 }}
{{ for color in primary_colors }}.bg-primary-{{ i }} {
    background-color:{{ color.background_rgb }};
}
{{ i = i + 1 }}{{ end }}
{{ i = 0 }}
{{ for color in primary_colors }}.text-primary-{{ i }} {
    color:{{ color.background_rgb }};
}
{{ i = i + 1 }}{{ end }}
/* complemental */
{{ i = 0 }}
{{ for color in complemental_colors }}.bg-complemental-{{ i }} {
    background-color:{{ color.background_rgb }};
}
{{ i = i + 1 }}{{ end }}
{{ i = 0 }}
{{ for color in complemental_colors }}.text-complemental-{{ i }} {
    color:{{ color.background_rgb }};
}
{{ i = i + 1 }}{{ end }}
/* analogous1 */
{{ i = 0 }}
{{ for color in analogous_colors1 }}.bg-analogous1-{{ i }} {
    background-color:{{ color.background_rgb }};
}
{{ i = i + 1 }}{{ end }}
{{ i = 0 }}
{{ for color in analogous_colors1 }}.text-analogous1-{{ i }} {
    color:{{ color.background_rgb }};
}
{{ i = i + 1 }}{{ end }}
/* analogous2 */
{{ i = 0 }}
{{ for color in analogous_colors2 }}.bg-analogous2-{{ i }} {
    background-color:{{ color.background_rgb }};
}
{{ i = i + 1 }}{{ end }}
{{ i = 0 }}
{{ for color in analogous_colors2 }}.text-analogous2-{{ i }} {
    color:{{ color.background_rgb }};
}
{{ i = i + 1 }}{{ end }}
/* triadic1 */
{{ i = 0 }}
{{ for color in triadic_colors1 }}.bg-triadic1-{{ i }} {
    background-color:{{ color.background_rgb }};
}
{{ i = i + 1 }}{{ end }}
{{ i = 0 }}
{{ for color in triadic_colors1 }}.text-triadic1-{{ i }} {
    color:{{ color.background_rgb }};
}
{{ i = i + 1 }}{{ end }}
/* triadic2 */
{{ i = 0 }}
{{ for color in triadic_colors2 }}.bg-triadic2-{{ i }} {
    background-color:{{ color.background_rgb }};
}
{{ i = i + 1 }}{{ end }}
{{ i = 0 }}
{{ for color in triadic_colors2 }}.text-triadic2-{{ i }} {
    color:{{ color.background_rgb }};
}
{{ i = i + 1 }}{{ end }}
/* variant1 */
{{ i = 0 }}
{{ for color in variant_colors1 }}.bg-variant1-{{ i }} {
    background-color:{{ color.background_rgb }};
}
{{ i = i + 1 }}{{ end }}
{{ i = 0 }}
{{ for color in variant_colors1 }}.text-variant1-{{ i }} {
    color:{{ color.background_rgb }};
}
{{ i = i + 1 }}{{ end }}
/* variant2 */
{{ i = 0 }}
{{ for color in variant_colors2 }}.bg-variant2-{{ i }} {
    background-color:{{ color.background_rgb }};
}
{{ i = i + 1 }}{{ end }}
{{ i = 0 }}
{{ for color in variant_colors2 }}.text-variant2-{{ i }} {
    color:{{ color.background_rgb }};
}
{{ i = i + 1 }}{{ end }}
/* warning */
{{ i = 0 }}
{{ for color in warning_colors }}.bg-warning-{{ i }} {
    background-color:{{ color.background_rgb }};
}
{{ i = i + 1 }}{{ end }}
{{ i = 0 }}
{{ for color in warning_colors }}.text-warning-{{ i }} {
    color:{{ color.background_rgb }};
}
{{ i = i + 1 }}{{ end }}
/* errors */
{{ i = 0 }}
{{ for color in error_colors }}.bg-error-{{ i }} {
    background-color:{{ color.background_rgb }};
}
{{ i = i + 1 }}{{ end }}
{{ i = 0 }}
{{ for color in error_colors }}.text-error-{{ i }} {
    color:{{ color.background_rgb }};
}
{{ i = i + 1 }}{{ end }}
/* gray scale */
{{ i = 0 }}
{{ for color in gray_scale }}.bg-gray-{{ i }} {
    background-color:{{ color.background_rgb }};
}
{{ i = i + 1 }}{{ end }}
{{ i = 0 }}
{{ for color in gray_scale }}.text-gray-{{ i }} {
    color:{{ color.background_rgb }};
}
{{ i = i + 1 }}{{ end }}
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
