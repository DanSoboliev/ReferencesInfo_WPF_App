using System;
using Common.ConsoleIO;
using ReferencesInfo.ConsoleEditor.ConsoleUI;
using ReferencesInfo.Data;
using ReferencesInfo.IO;

namespace ReferencesInfo.ConsoleEditor {
    class Program {
        static DataContext dataContext;
        static MainManager mainManager;
        static string directory = "files";
        static BinarySerializationController bsIoController = new BinarySerializationController();

        static void Main(string[] args) {
            Console.Title = "ReferencesInfo.ConsoleEditor (Соболєв Д. О.)";
            Settings.SetConsoleParam();
            Console.WriteLine("Реалізація класів для предметної області");

            dataContext = new DataContext(bsIoController) {
                Directory = directory
            };
            mainManager = new MainManager(dataContext);
            mainManager.Load();
            mainManager.Run();

            Console.ReadKey(true);
        }
    }
}
