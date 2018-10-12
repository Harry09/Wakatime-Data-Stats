using Avalonia;
using Avalonia.Markup.Xaml;

namespace WTStats.GUI
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
   }
}