using ConsoleTools;
using BetterConsoles.Tables.Configuration;
using BetterConsoles.Tables.Builders;
using BetterConsoles.Core;
using BetterConsoles.Colors.Extensions;
using BetterConsoles.Tables.Builders.Interfaces;
using BetterConsoles.Colors.Builders;
using BetterConsoles.Colors;
using BetterConsoles.Tables.Common;


namespace AdaCredit.ConsoleUI {


    internal static class ConsoleUtils {


        public static void AwaitForKey() {
            Console.WriteLine("Pressione qualquer tecla para continuar.");
            Console.ReadKey();
        }


        //Menu Configs
        public static Action<MenuConfig> ConfigureMenu(string title, string selector = "--> ", char separator = '/') {
            return (menuConfig) => {
                menuConfig.EnableFilter = false;
                menuConfig.EnableBreadcrumb = true;
                menuConfig.SelectedItemBackgroundColor = ConsoleColor.White;
                menuConfig.SelectedItemForegroundColor = ConsoleColor.Black;
                menuConfig.Selector = selector;
                menuConfig.Title = title;
                menuConfig.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join($" {separator} ", titles));
            };
        }

        public static Action<MenuConfig> ConfigureList(string title, string selector = "--> ", char separator = '/') {
            return (menuConfig) => {
                menuConfig.EnableFilter = true;
                menuConfig.EnableBreadcrumb = true;
                menuConfig.SelectedItemBackgroundColor = ConsoleColor.White;
                menuConfig.SelectedItemForegroundColor = ConsoleColor.Black;
                menuConfig.Selector = selector;
                menuConfig.Title = title;
                menuConfig.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join($" {separator} ", titles));
            };
        }


        //Table Configs


        //

    }
}
