using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaletteGenerator.ViewModels;
using Scriban.Parsing;

namespace PaletteGenerator.Services
{
    public class HtmlGenerator
    {
        private string templateString = @"
<html>
    <head>
        <style>
            table{
                border-spacing: 0;
            }
            tr{
            }
            td{
                padding: 1px 2px;
            }
        </style>
    </head>
    <body>
        <table>
            <tr>
                <td>primary</td>
  {{ for color in primary_colors }}<td style=""background-color:#{{ color.background_rgb }};color:#{{ color.foreground_rgb }};"">#{{ color.background_rgb }}</td>
  {{ end }}
            </tr>
            <tr>
                <td>complemental</td>
  {{ for color in complemental_colors }}<td style=""background-color:#{{ color.background_rgb }};color:#{{ color.foreground_rgb }};"">#{{ color.background_rgb }}</td>
  {{ end }}
            </tr>
            <tr>
                <td>analogous -</td>
  {{ for color in analogous_colors1 }}<td style=""background-color:#{{ color.background_rgb }};color:#{{ color.foreground_rgb }};"">#{{ color.background_rgb }}</td>
  {{ end }}
            </tr>
            <tr>
                <td>analogous +</td>
  {{ for color in analogous_colors2 }}<td style=""background-color:#{{ color.background_rgb }};color:#{{ color.foreground_rgb }};"">#{{ color.background_rgb }}</td>
  {{ end }}
            </tr>
            <tr>
                <td>triadic -</td>
  {{ for color in triadic_colors1 }}<td style=""background-color:#{{ color.background_rgb }};color:#{{ color.foreground_rgb }};"">#{{ color.background_rgb }}</td>
  {{ end }}
            </tr>
            <tr>
                <td>triadic +</td>
  {{ for color in triadic_colors2 }}<td style=""background-color:#{{ color.background_rgb }};color:#{{ color.foreground_rgb }};"">#{{ color.background_rgb }}</td>
  {{ end }}
            </tr>
            <tr>
                <td>variant -</td>
  {{ for color in variant_colors1 }}<td style=""background-color:#{{ color.background_rgb }};color:#{{ color.foreground_rgb }};"">#{{ color.background_rgb }}</td>
  {{ end }}
            </tr>
            <tr>
                <td>variant +</td>
  {{ for color in variant_colors2 }}<td style=""background-color:#{{ color.background_rgb }};color:#{{ color.foreground_rgb }};"">#{{ color.background_rgb }}</td>
  {{ end }}
            </tr>
            <tr>
                <td>warning</td>
  {{ for color in warning_colors }}<td style=""background-color:#{{ color.background_rgb }};color:#{{ color.foreground_rgb }};"">#{{ color.background_rgb }}</td>
  {{ end }}
            </tr>
            <tr>
                <td>error</td>
  {{ for color in error_colors }}<td style=""background-color:#{{ color.background_rgb }};color:#{{ color.foreground_rgb }};"">#{{ color.background_rgb }}</td>
  {{ end }}
            </tr>
            <tr>
                <td>gray scale</td>
  {{ for color in gray_scale }}<td style=""background-color:#{{ color.background_rgb }};color:#{{ color.foreground_rgb }};"">#{{ color.background_rgb }}</td>
  {{ end }}
            </tr>
        </table>
    </body>
</html>";

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
            var rendered = template.Render(new { 
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
