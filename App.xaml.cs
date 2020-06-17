using Autofac;
using System.Windows;

namespace P4_Projekt_2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Użycie Autofac'a by wykorzystać DI do działania programu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<theBankVievModel>();
            builder.RegisterType<theBankLoader>();
            builder.Register(bankData => new theBankLoader().GetViewModel());
            builder.RegisterType<MainWindow>();
            builder.RegisterType<LoginWindow>();

            var container = builder.Build();

            var window = container.Resolve<LoginWindow>();
            window.Show();
        }
    }
}
