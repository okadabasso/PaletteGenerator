using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace PaletteGenerator.ViewModels
{
    public class SampleTextViewModel : BindableBase
    {
        public ReactiveCommand<string> PanelClick { get; set; } = new ReactiveCommand<string>();
        public ReactiveProperty<string> AreaName { get; set; } = new ReactiveProperty<string>();

        public ReactiveProperty<Brush> PrimaryBorder { get; set; } = new ReactiveProperty<Brush>();
        public ReactiveProperty<Brush> SecondaryBorder { get; set; } = new ReactiveProperty<Brush>();
        public ReactiveProperty<Brush> InfoBorder { get; set; } = new ReactiveProperty<Brush>();
        public ReactiveProperty<Brush> SuccessBorder { get; set; } = new ReactiveProperty<Brush>();
        public ReactiveProperty<Brush> WarningBorder { get; set; } = new ReactiveProperty<Brush>();
        public ReactiveProperty<Brush> DangerBorder { get; set; } = new ReactiveProperty<Brush>();

        public ReactiveProperty<SolidColorBrush> PrimaryTextBrush { get; set; } = new ReactiveProperty<SolidColorBrush>(new SolidColorBrush(Colors.DarkSlateGray));
        public ReactiveProperty<SolidColorBrush> SecondaryTextBrush { get; set; } = new ReactiveProperty<SolidColorBrush>(new SolidColorBrush(Colors.DarkSlateGray));
        public ReactiveProperty<SolidColorBrush> InfoTextBrush { get; set; } = new ReactiveProperty<SolidColorBrush>(new SolidColorBrush(Colors.DarkSlateGray));
        public ReactiveProperty<SolidColorBrush> SuccessTextBrush { get; set; } = new ReactiveProperty<SolidColorBrush>(new SolidColorBrush(Colors.DarkSlateGray));
        public ReactiveProperty<SolidColorBrush> WarningTextBrush { get; set; } = new ReactiveProperty<SolidColorBrush>(new SolidColorBrush(Colors.DarkSlateGray));
        public ReactiveProperty<SolidColorBrush> DangerTextBrush { get; set; } = new ReactiveProperty<SolidColorBrush>(new SolidColorBrush(Colors.DarkSlateGray));
        public ReactiveProperty<SolidColorBrush> BackgroundBrush { get; set; } = new ReactiveProperty<SolidColorBrush>(new SolidColorBrush(Colors.DarkSlateGray));

        private ReactiveProperty<Brush> activeBrush;

        public ReactiveCommand<SolidColorBrush> SelectColorCommand { get; set; } = new ReactiveCommand<SolidColorBrush>();
        public SampleTextViewModel()
        {
            PanelClick.Subscribe(areaName => { SelectArea(areaName); });

            PrimaryBorder.Value = CreateTransparentBorder();
            SecondaryBorder.Value = CreateTransparentBorder();
            InfoBorder.Value = CreateTransparentBorder();
            SuccessBorder.Value = CreateTransparentBorder();
            WarningBorder.Value = CreateTransparentBorder();
            DangerBorder.Value = CreateTransparentBorder();
            BackgroundBrush.Value = new SolidColorBrush(Colors.Transparent);

            SelectColorCommand.Subscribe(brush => { SelectColor(brush); });
        }
        private Brush CreateTransparentBorder()
        {

            return Brushes.Transparent;
        }
        private Brush CreateSelectedBorder()
        {
            var style = new Style(typeof(Border));
            var color = BackgroundBrush.Value.Color;
            var xorColor = Color.FromRgb((byte)(0xff ^ color.R), (byte)(0xff ^ color.G), (byte)(0xff ^ color.B));
            var brush = new SolidColorBrush(xorColor);
            

            return brush;
        }
        private void SelectArea(string area)
        {
            if(activeBrush != null)
            {
                activeBrush.Value = CreateTransparentBorder();
            }
            switch (area)
            {
                case "Primary":
                    activeBrush = PrimaryBorder;
                    break;
                case "Secondary":
                    activeBrush = SecondaryBorder;
                    break;
                case "Info":
                    activeBrush = InfoBorder;
                    break;
                case "Success":
                    activeBrush = SuccessBorder;
                    break;
                case "Warning":
                    activeBrush = WarningBorder;
                    break;
                case "Danger":
                    activeBrush = DangerBorder;
                    break;
                default:
                    AreaName.Value = area;
                    return;
            }
            activeBrush.Value = CreateSelectedBorder();

            AreaName.Value = area;
            RaisePropertyChanged(null);
        }
        private void SelectColor(SolidColorBrush brush)
        {
            ReactiveProperty<SolidColorBrush> activeTextColor;
            switch (AreaName.Value)
            {
                case "Primary":
                    activeTextColor = PrimaryTextBrush;
                    break;
                case "Secondary":
                    activeTextColor = SecondaryTextBrush;
                    break;
                case "Info":
                    activeTextColor = InfoTextBrush;
                    break;
                case "Success":
                    activeTextColor = SuccessTextBrush;
                    break;
                case "Warning":
                    activeTextColor = WarningTextBrush;
                    break;
                case "Danger":
                    activeTextColor = DangerTextBrush;
                    break;
                case "Background":
                    activeTextColor = BackgroundBrush;
                    break;
                default:
                    return;
            }
            activeTextColor.Value = brush;
            RaisePropertyChanged(null);
        }
    }
}
