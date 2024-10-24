using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    abstract class BeginOrEndBaseScene : ISceneUpdate
    {
        protected int nowSelIndex = 0;
        protected string strTitle;
        protected string strOne;

        public abstract void EnterJKeyDo(); 

        public void Update()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(Game.width / 2 - strTitle.Length / 2, 5);
            Console.Write(strTitle);

            Console.SetCursorPosition(Game.width / 2 - strOne.Length / 2, 8);
            Console.ForegroundColor = nowSelIndex == 0 ? ConsoleColor.Red : ConsoleColor.White;
            Console.Write(strOne);
            Console.SetCursorPosition(Game.width / 2 - 4 / 2, 10);
            Console.ForegroundColor = nowSelIndex == 1 ? ConsoleColor.Red : ConsoleColor.White;
            Console.Write("Exit");

            switch(Console.ReadKey(true).Key)
            {
                // w 或者 上
                case ConsoleKey.W:
                    nowSelIndex--;
                    if (nowSelIndex < 0)
                    {
                        nowSelIndex = 0;
                    }
                    break;
                case ConsoleKey.UpArrow:
                    nowSelIndex--;
                    if (nowSelIndex < 0)
                    {
                        nowSelIndex = 0;
                    }
                    break;
                // s 或者 下
                case ConsoleKey.S:
                    nowSelIndex++;
                    if (nowSelIndex > 1)
                    {
                        nowSelIndex = 1;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    nowSelIndex++;
                    if (nowSelIndex > 1)
                    {
                        nowSelIndex = 1;
                    }
                    break;
                // j 或者 enter
                case ConsoleKey.J:
                    EnterJKeyDo();
                    break;
                case ConsoleKey.Enter:
                    EnterJKeyDo();
                    break;
            }
        }
    }
}
