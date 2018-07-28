using System;
namespace GameEngine
{
    partial class EngineClass
    {
        public void PrintText(debugType USERdebugType, string str)
        {
            ConsoleColor old = Console.BackgroundColor;
            switch (USERdebugType)
            {
                case debugType.Error:
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n\n IT LOOKS LIKE WE HAVE DETECTED AN ERROR! \n\n");

                        break;
                    }
                case debugType.Warning:
                    {
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        break;
                    }
                case debugType.Info:
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        break;
                    }
                case debugType.Debug:
                    {
                        Console.BackgroundColor = ConsoleColor.Magenta;
                        break;
                    }
                default:
                    {
                        Console.BackgroundColor = old;
                        break;
                    }
            }
            Console.WriteLine(USERdebugType.ToString() + ":" + " " + str);
            Console.BackgroundColor = old;
        }
    }
}