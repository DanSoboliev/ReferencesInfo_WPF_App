using System;
using Common.ConsoleIO;

namespace ReferencesInfo.ConsoleEditor.ConsoleUI {
    public abstract class CommandManager {
        protected static int indicator { get; set; }
        public CommandManager() {
            IniCommandInfoArray();
        }
        protected abstract void IniCommandInfoArray();
        protected abstract void PrepareScreen();
        protected CommandInfo[] commandsInfo;

        //Показ команд
        protected void ShowCommands(CommandInfo[] commandsArray, string prompt) {
            Console.WriteLine(prompt);
            for (int i = 0; i < commandsArray.Length; i++) {
                Console.WriteLine("\t{0,2} - {1}", i, commandsArray[i].name);
            }
        }

        //Вибір команди
        protected Command EnterCommand(CommandInfo[] commandsArray, string prompt) {
            Console.WriteLine();
            int number = Entering.EnterInt(prompt, 0, commandsArray.Length - 1);
            return commandsArray[number].command;
        }

        //Run
        public void Run() {
            while (true) {
                PrepareScreen();
                ShowCommands(commandsInfo, "  Список команд: ");
                Command command = EnterCommand(commandsInfo, "Введіть номер команди меню");
                if (command == null) {
                    return;
                }
                command();
            }
        }

        //PressToContinue
        protected virtual void PressToContinue() {
            Console.Write("\tДля продовження натисніть будь-яку клавішу ");
            Console.ReadKey(true);
        }
    }
}
