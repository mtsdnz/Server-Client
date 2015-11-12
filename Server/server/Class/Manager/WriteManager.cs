using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Class
{
    public class WriteManager
    {
        public static void wl(string s, ConsoleColor fg = ConsoleColor.Gray, ConsoleColor bg = ConsoleColor.Black)
        {
            ConsoleColor c_fg = Console.ForegroundColor;
            ConsoleColor c_bg = Console.BackgroundColor;

            Console.ForegroundColor = fg;
            Console.BackgroundColor = bg;

            Console.WriteLine(s);

            Console.ForegroundColor = c_fg;
            Console.BackgroundColor = c_bg;
        }

        public static void w(string s, ConsoleColor fg = ConsoleColor.Gray, ConsoleColor bg = ConsoleColor.Gray)
        {
            ConsoleColor c_fg = Console.ForegroundColor;
            //ConsoleColor c_bg = Console.BackgroundColor;

            Console.ForegroundColor = fg;

            Console.Write(s);

            Console.ForegroundColor = c_fg;
        }
    }
}
