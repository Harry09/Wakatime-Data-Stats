using System;
using Avalonia;
using Avalonia.Logging.Serilog;
using WTStats.GUI.ViewModels;
using WTStats.GUI.Views;

namespace WTStats.GUI
{
    class Program
    {
        static void Main(string[] args)
        {
            BuildAvaloniaApp()
				.Start<MainWindow>(() => new MainWindowViewModel());
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .UseReactiveUI()
                .LogToDebug();
    }
}
