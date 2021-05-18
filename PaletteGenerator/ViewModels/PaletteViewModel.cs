using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System.Windows.Media;
using System.Reactive.Linq;
using System.Collections.ObjectModel;
using DryIoc;
using PaletteGenerator.Services;
using System.Windows;
using System.Threading.Tasks;
using PaletteGenerator.Infrastructure;
using System.Threading;
using System.Windows.Threading;

namespace PaletteGenerator.ViewModels
{
    public class PaletteViewModel : BindableBase
    {
        private readonly IContainer _container;
        public ReactiveCommand ToWpfResourceCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand ToCssCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand ToHtmlCommand { get; set; } = new ReactiveCommand();
        public ReactiveProperty<string> Message { get; set; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> ColorCode { get; set; } = new ReactiveProperty<string>();
        public ReactiveProperty<byte> RedValue { get; set; } = new ReactiveProperty<byte>();
        public ReactiveProperty<byte> GreenValue { get; set; } = new ReactiveProperty<byte>();
        public ReactiveProperty<byte> BlueValue { get; set; } = new ReactiveProperty<byte>();
        public ReactiveProperty<SolidColorBrush> ForegroundBrush { get; set; } = new ReactiveProperty<SolidColorBrush>();
        public ReactiveProperty<SolidColorBrush> BaseColorBrush { get; set; }
        public ObservableCollection<ColorListItem> PrimaryColors { get; set; } = new ObservableCollection<ColorListItem>();
        public ObservableCollection<ColorListItem> ComplementalColors { get; set; } = new ObservableCollection<ColorListItem>();
        public ObservableCollection<ColorListItem> AnalogousColors1 { get; set; } = new ObservableCollection<ColorListItem>();
        public ObservableCollection<ColorListItem> AnalogousColors2 { get; set; } = new ObservableCollection<ColorListItem>();
        public ObservableCollection<ColorListItem> TriadicColors1 { get; set; } = new ObservableCollection<ColorListItem>();
        public ObservableCollection<ColorListItem> TriadicColors2 { get; set; } = new ObservableCollection<ColorListItem>();
        public ObservableCollection<ColorListItem> VariantColors1 { get; set; } = new ObservableCollection<ColorListItem>();
        public ObservableCollection<ColorListItem> VariantColors2 { get; set; } = new ObservableCollection<ColorListItem>();
        public ObservableCollection<ColorListItem> WarningColors { get; set; } = new ObservableCollection<ColorListItem>();
        public ObservableCollection<ColorListItem> ErrorColors { get; set; } = new ObservableCollection<ColorListItem>();
        public ObservableCollection<ColorListItem> GrayScale { get; set; } = new ObservableCollection<ColorListItem>();
        public PaletteViewModel(IContainer container)
        {
            _container = container;
            BaseColorBrush = Observable.CombineLatest(
                RedValue,
                GreenValue,
                BlueValue,
                (r, g, b) => new SolidColorBrush(new Color { R = r, G = g, B = b, A = 255 }))
                .ToReactiveProperty<SolidColorBrush>()
                ;
            BaseColorBrush.Subscribe(x =>
            {
                BaseColorBrush.Value.Freeze();
                var hsl = HslColor.FromRgb(x.Color);
                ForegroundBrush.Value = hsl.L < 0.55 ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Black);

                CreatePalette(x);

                ColorCode.Value = x.Color.R.ToString("x2") + x.Color.G.ToString("x2") + x.Color.B.ToString("x2");
            });
            ColorCode.Subscribe(x => {
                if (x.Length == 6)
                {
                    RedValue.Value = byte.Parse(x.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                    GreenValue.Value = byte.Parse(x.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                    BlueValue.Value = byte.Parse(x.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                }
            });

            ToHtmlCommand.Subscribe(() => {
                using (var scope = _container.OpenScope())
                {
                    ClearFlash();
                    var service = scope.Resolve<HtmlGenerator>();
                    var content = service.Generate(
                        PrimaryColors,
                        ComplementalColors,
                        AnalogousColors1,
                        AnalogousColors2,
                        TriadicColors1,
                        TriadicColors2,
                        VariantColors1,
                        VariantColors2,
                        WarningColors,
                        ErrorColors,
                        GrayScale
                        );
                    Clipboard.SetText(content);
                    ShowFlash("クリップボードにコピーしました。");
                }
            });
            ToCssCommand.Subscribe(() => {
                using (var scope = _container.OpenScope())
                {
                    ClearFlash();
                    var service = scope.Resolve<CssGenerator>();
                    var content = service.Generate(
                        PrimaryColors.Reverse(),
                        ComplementalColors.Reverse(),
                        AnalogousColors1.Reverse(),
                        AnalogousColors2.Reverse(),
                        TriadicColors1.Reverse(),
                        TriadicColors2.Reverse(),
                        VariantColors1.Reverse(),
                        VariantColors2.Reverse(),
                        WarningColors.Reverse(),
                        ErrorColors.Reverse(),
                        GrayScale.Reverse()
                        );
                    Clipboard.SetText(content);
                    ShowFlash("クリップボードにコピーしました。");
                }
            });
            ToWpfResourceCommand.Subscribe(() => {
                using (var scope = _container.OpenScope())
                {
                    ClearFlash();
                    var service = scope.Resolve<WpfResourceGenerator>();
                    var content = service.Generate(
                        PrimaryColors.Reverse(),
                        ComplementalColors.Reverse(),
                        AnalogousColors1.Reverse(),
                        AnalogousColors2.Reverse(),
                        TriadicColors1.Reverse(),
                        TriadicColors2.Reverse(),
                        VariantColors1.Reverse(),
                        VariantColors2.Reverse(),
                        WarningColors.Reverse(),
                        ErrorColors.Reverse(),
                        GrayScale.Reverse()
                        );
                    Clipboard.SetText(content);
                    ShowFlash("クリップボードにコピーしました。");
                }
            });
        }
        private void CreatePalette(SolidColorBrush baseColorBrush)
        {
            Color c = baseColorBrush.Color;
            PrimaryColors = new ObservableCollection<ColorListItem>(CreateColorList(baseColorBrush.Color, 0f));
            ComplementalColors = new ObservableCollection<ColorListItem>(CreateColorList(baseColorBrush.Color, 180f));
            AnalogousColors1 = new ObservableCollection<ColorListItem>(CreateColorList(baseColorBrush.Color, -30f));
            AnalogousColors2 = new ObservableCollection<ColorListItem>(CreateColorList(baseColorBrush.Color, 30f));
            TriadicColors1 = new ObservableCollection<ColorListItem>(CreateColorList(baseColorBrush.Color, -120f));
            TriadicColors2 = new ObservableCollection<ColorListItem>(CreateColorList(baseColorBrush.Color, 120f));
            VariantColors1 = new ObservableCollection<ColorListItem>(CreateColorList(baseColorBrush.Color, -60f));
            VariantColors2 = new ObservableCollection<ColorListItem>(CreateColorList(baseColorBrush.Color, 60f));

            // 原色ではなく彩度をベースカラーにあわせる
            var baseHsl = HslColor.FromRgb(c);
            var warning = new HslColor(30, (baseHsl.S + 1) / 2, (baseHsl.L + 1) / 2);
            WarningColors = new ObservableCollection<ColorListItem>(CreateColorList(HslColor.ToRgb(warning), 0f));

            var error = new HslColor(0, (baseHsl.S + 1) / 2, (baseHsl.L + 1) / 2);
            ErrorColors = new ObservableCollection<ColorListItem>(CreateColorList(HslColor.ToRgb(error), 0f));

            var gray = new HslColor(baseHsl.H, (float)(baseHsl.S * 0.10f), baseHsl.L);

            GrayScale = new ObservableCollection<ColorListItem>(CreateColorList(HslColor.ToRgb(gray), 0f));

            RaisePropertyChanged(nameof(PrimaryColors));
            RaisePropertyChanged(nameof(ComplementalColors));
            RaisePropertyChanged(nameof(AnalogousColors1));
            RaisePropertyChanged(nameof(AnalogousColors2));
            RaisePropertyChanged(nameof(TriadicColors1));
            RaisePropertyChanged(nameof(TriadicColors2));
            RaisePropertyChanged(nameof(VariantColors1));
            RaisePropertyChanged(nameof(VariantColors2));
            RaisePropertyChanged(nameof(ErrorColors));
            RaisePropertyChanged(nameof(WarningColors));
            RaisePropertyChanged(nameof(GrayScale));
        }
        private List<ColorListItem> CreateColorList(Color baseColor, float offset)
        {
            var baseHsl = HslColor.FromRgb(baseColor);
            var h = baseHsl.H + offset;
            if (h < 0) h += 360;
            if (h >= 360) h -= 360;
            var hsl = new HslColor(h, baseHsl.S, baseHsl.L);
            var list = new List<ColorListItem>();

            var ratio = (0.95f - 0.15) / 9.0f;
            for (var i = 0; i < 10; i++)
            {
                var l = 0.15 + i * ratio;
                if (l < 0) l = 0;
                if (l > 1) l = 1;
                var color = new SolidColorBrush(HslColor.ToRgb(new HslColor(hsl.H, hsl.S, (float)l)));
                color.Freeze();

                var foreground = l < 0.55 ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Black);
                list.Add(new ColorListItem { Background = color, Foreground = foreground });
            }


            return list;
        }
        private void ShowFlash(string message)
        {
            Message.Value = message;
            RaisePropertyChanged(nameof(Message));
            DoEvents();
            Task.Run(() => {
                Thread.Sleep(5000);
                Message.Value = "";
                RaisePropertyChanged(nameof(Message));

            });   
        }
        private void ClearFlash()
        {
            Message.Value = "";
            RaisePropertyChanged(nameof(Message));
        }
        public void DoEvents()
        {
            DispatcherFrame frame = new DispatcherFrame();
            var callback = new DispatcherOperationCallback(obj =>
            {
                ((DispatcherFrame)obj).Continue = false;
                return null;
            });
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, callback, frame);
            Dispatcher.PushFrame(frame);
        }
    }
}
