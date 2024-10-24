using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    internal class Food : GameObject
    {
        public Food(Snake snake)
        {
            RandomPos(snake);
        }
        public override void Draw()
        {
            Console.SetCursorPosition(pos.x, pos.y);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("O");
        }

        public void RandomPos(Snake snake)
        {
            Random r = new Random();
            int x = r.Next(2, Game.width - 2);
            int y = r.Next(1, Game.height - 4);
            pos = new Position(x, y);
            if(snake.CheckSamePos(pos))
            {
                RandomPos(snake);
            }
        }
    }
}
