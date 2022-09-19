using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using PaletteGenerator.Infrastructure;

namespace PaletteGenerator.ViewModels
{
    public class SampleTextViewModel : BindableBase
    {
        public ReactiveCommand<string> PanelClick { get; set; } = new ReactiveCommand<string>();
        private  ContentCategory _activeArea { get; set; } = ContentCategory.None;

        public ObservableCollection<SampleTextItem> SampleTextItems { get; set; }
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

        private TargetField _targetField { get; set; }
        private bool _targetFieldBackground { get; set; }

        private static readonly string DefaultContent = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
        private enum ContentCategory
        {
            Primary,
            Secondary,
            Info,
            Success,
            Warning,
            Danger,
            None = 0xff
        }
        public enum TargetField
        {
            Forground,
            Background
        }
        public void SetTargetField(TargetField field)
        {
            _targetField = field;
        }
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


            SampleTextItems = new ObservableCollection<SampleTextItem>();
            foreach(ContentCategory i in Enum.GetValues(typeof(ContentCategory)))
            {
                if(i == ContentCategory.None)
                {
                    continue;
                }
                var item = new SampleTextItem
                {
                    BorderBrush = new ReactiveProperty<Brush>(new SolidColorBrush(Colors.Transparent)),
                    BackgroundBrush = new ReactiveProperty<Brush>(new SolidColorBrush(Colors.White)),
                    ForegroundBrush = new ReactiveProperty<Brush>(new SolidColorBrush(Colors.DarkSlateGray)),
                    Title = new ReactiveProperty<string>(i.ToString()),
                    Content = new ReactiveProperty<string>(DefaultContent),
                    Contrast = new ReactiveProperty<double>(ColorFunctions.Contrast(Colors.White, Colors.DarkSlateGray))
                };
                SampleTextItems.Add(item);
            }

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
        private void SelectArea(string areaName)
        {
            if(_activeArea != ContentCategory.None)
            {
                SampleTextItems[(int)_activeArea].BorderBrush.Value = new SolidColorBrush(Colors.Transparent);
            }
            var area = (ContentCategory) Enum.Parse(typeof(ContentCategory), areaName);
            if(activeBrush != null)
            {
                activeBrush.Value = CreateTransparentBorder();
            }
            var item = SampleTextItems[(int)area];
            item.BorderBrush.Value = CreateSelectedBorder();

            _activeArea = area;;
            RaisePropertyChanged(null);
        }


        private void SelectColor(SolidColorBrush brush)
        {
            var item = SampleTextItems[(int)_activeArea];
            switch (_targetField)
            {
                case TargetField.Forground:
                    item.ForegroundBrush.Value = brush;
                    break;
                case TargetField.Background:
                    item.BackgroundBrush.Value = brush;
                    break;
            }
            item.Contrast.Value = ColorFunctions.Contrast(
                ((SolidColorBrush)item.ForegroundBrush.Value).Color,
                ((SolidColorBrush)item.BackgroundBrush.Value).Color);

            RaisePropertyChanged(null);
        }
    }
    public class SampleTextItem
    {
        public ReactiveProperty<Brush> BorderBrush { get; set; }
        public ReactiveProperty<Brush> ForegroundBrush { get; set; }
        public ReactiveProperty<Brush> BackgroundBrush { get; set; }

        public ReactiveProperty<string> Title { get; set; }
        public ReactiveProperty<string> Content { get; set; }
        public ReactiveProperty<double> Contrast{ get; set; }
    }
}
