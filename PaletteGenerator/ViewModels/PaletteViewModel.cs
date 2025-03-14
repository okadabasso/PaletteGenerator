﻿using Prism.Commands;
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


        public ReactiveCommand<SolidColorBrush> SelectColorCommand { get; set; } = new ReactiveCommand<SolidColorBrush>();
        public SampleTextViewModel SampleTextViewModel { get; set; }
        public ReactiveProperty<bool> TargetFieldForeground { get; set;  } = new ReactiveProperty<bool>(true);
        public ReactiveProperty<bool> TargetFieldBackground { get; set; } = new ReactiveProperty<bool>(false);

        public ReactiveProperty<SolidColorBrush> WhiteBrush { get; set; } = new ReactiveProperty<SolidColorBrush>(new SolidColorBrush(Colors.White));
        public ReactiveProperty<SolidColorBrush> BlackBrush{ get; set; } = new ReactiveProperty<SolidColorBrush>(new SolidColorBrush(Colors.Black));

        public PaletteViewModel(IContainer container, SampleTextViewModel sampleTextViewModel)
        {
            _container = container;
            SampleTextViewModel = sampleTextViewModel;
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

            SelectColorCommand.Subscribe(brush => {
                sampleTextViewModel.SetTargetField(TargetFieldForeground.Value ? SampleTextViewModel.TargetField.Forground : SampleTextViewModel.TargetField.Background);
                SampleTextViewModel.SelectColorCommand.Execute(brush);
            });
        }
        private void CreatePalette(SolidColorBrush baseColorBrush)
        {
            Color c = baseColorBrush.Color;
            var hsl = HslColor.FromRgb(c);
            var baseColor = HslColor.ToRgb(new HslColor(hsl.H, 1, 0.75F));
            PrimaryColors = new ObservableCollection<ColorListItem>(CreateColorList(baseColor, 0f));
            ComplementalColors = new ObservableCollection<ColorListItem>(CreateColorList(baseColor, 180f));
            AnalogousColors1 = new ObservableCollection<ColorListItem>(CreateColorList(baseColor, -30f));
            AnalogousColors2 = new ObservableCollection<ColorListItem>(CreateColorList(baseColor, 30f));
            TriadicColors1 = new ObservableCollection<ColorListItem>(CreateColorList(baseColor, -120f));
            TriadicColors2 = new ObservableCollection<ColorListItem>(CreateColorList(baseColor, 120f));
            VariantColors1 = new ObservableCollection<ColorListItem>(CreateColorList(baseColor, -60f));
            VariantColors2 = new ObservableCollection<ColorListItem>(CreateColorList(baseColor, 60f));

            // 原色ではなく彩度をベースカラーにあわせる
            var baseHsl = HslColor.FromRgb(c);
            var warning = new HslColor(30, 0.75F, 0.5F);
            WarningColors = new ObservableCollection<ColorListItem>(CreateColorList(HslColor.ToRgb(warning), 0f));

            var error = new HslColor(5, 1F, 0.75F);
            ErrorColors = new ObservableCollection<ColorListItem>(CreateColorList(HslColor.ToRgb(error), 0f));

            var gray = new HslColor(baseHsl.H, 0F, 0F);

            GrayScale = new ObservableCollection<ColorListItem>(CreateGrayScale(HslColor.ToRgb(gray), 0f));

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

            var steps = new int[] { 100, 200, 300, 400, 500, 600, 700, 800, 900 };
            for (int step = 0; step < steps.Length; step++)
            {
                var color = HslColor.ToRgb(GetRedShade(h, steps[step]));
                var brush = new SolidColorBrush(color);
                brush.Freeze();

                // コントラスト比計算
                var contrast1 = ColorFunctions.Contrast(Colors.Black, color);
                var contrast2 = ColorFunctions.Contrast(Colors.White, color);


                var foreground = contrast1 < contrast2 ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Black);
                list.Add(new ColorListItem { Background = brush, Foreground = foreground });
            }


            return list;
        }
        private List<ColorListItem> CreateGrayScale(Color baseColor, float offset)
        {
            var baseHsl = HslColor.FromRgb(baseColor);
            var h = baseHsl.H + offset;
            if (h < 0) h += 360;
            if (h >= 360) h -= 360;
            var hsl = new HslColor(h, baseHsl.S, baseHsl.L);
            var list = new List<ColorListItem>();

            var steps = new int[] { 100, 200, 300, 400, 500, 600, 700, 800, 900 };
            for (int step = 0; step < steps.Length; step++)
            {
                var lightness = 95F - (95F - 15F) * Math.Pow((double)steps[step] / 900, 0.75);
                var color = HslColor.ToRgb(new HslColor(0, 0, (float)lightness /100));
                var brush = new SolidColorBrush(color);
                brush.Freeze();

                // コントラスト比計算
                var contrast1 = ColorFunctions.Contrast(Colors.Black, color);
                var contrast2 = ColorFunctions.Contrast(Colors.White, color);


                var foreground = contrast1 < contrast2 ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Black);
                list.Add(new ColorListItem { Background = brush, Foreground = foreground });
            }


            return list;
        }
        HslColor GetRedShade(float hue, int step)
        {
            double minL = ComputeLightnessScale(hue, 30, 15);  // L の最小値（900の暗さ）
            double maxL = 97;  // L の最大値（100の明るさ）
            
            // 3/4, 1/1.2, 1/1.07 で試す
            double lightness = maxL - (maxL - minL) * Math.Pow((double)step / 900, 1/1.2);

            double minS = 50; // 最小彩度（暗い色ほど低彩度）
            double maxS = 97; // 最大彩度（明るい色ほど高彩度）

            double saturation = maxS - (maxS - minS) * Math.Pow((double)step / 900, 1.2);

            return new HslColor(hue, (float)saturation / 100, (float)lightness / 100);
        }
        static double ComputeLightnessScale(float x, float middle, float magnitude)
        {
            float A = middle;  // 中央の高さ
            float B = magnitude;  // 振幅（最大値と最小値の差の半分）
            float C = 150;  // 波の中心（(60+240)/2）
            double D = 90;   // スケーリング（変化のなだらかさ調整）

            return A + B * Math.Cos(Math.PI * (x - C) / D);
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
        static Color HSLToRGB(double hue, double saturation, double lightness)
        {
            double c = (1 - Math.Abs(2 * lightness - 1)) * saturation;
            double x = c * (1 - Math.Abs((hue * 6) % 2 - 1));
            double m = lightness - c / 2;

            double r, g, b;
            if (hue < 1.0 / 6.0)
            {
                r = c;
                g = x;
                b = 0;
            }
            else if (hue < 2.0 / 6.0)
            {
                r = x;
                g = c;
                b = 0;
            }
            else if (hue < 3.0 / 6.0)
            {
                r = 0;
                g = c;
                b = x;
            }
            else if (hue < 4.0 / 6.0)
            {
                r = 0;
                g = x;
                b = c;
            }
            else if (hue < 5.0 / 6.0)
            {
                r = x;
                g = 0;
                b = c;
            }
            else
            {
                r = c;
                g = 0;
                b = x;
            }

            return Color.FromRgb(
                (byte)((r + m) * 255),
                (byte)((g + m) * 255),
                (byte)((b + m) * 255)
            );
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
