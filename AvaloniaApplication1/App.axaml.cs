using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaApplication1.ViewModels;
using AvaloniaApplication1.Views;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace AvaloniaApplication1
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            LiveCharts.Configure(config =>
    config
        // registers SkiaSharp as the library backend
        // REQUIRED unless you build your own
        .AddSkiaSharp()

        // adds the default supported types
        // OPTIONAL but highly recommend
        .AddDefaultMappers()

        // select a theme, default is Light
        // OPTIONAL
        //.AddDarkTheme()
        .AddLightTheme());


        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
