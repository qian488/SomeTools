using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    enum ESnakeBodyType
    {
        Head,
        Body,
    }
    internal class SnakeBody : GameObject
    {
        private ESnakeBodyType type;
        public SnakeBody(ESnakeBodyType type,int x,int y)
        {
            this.type = type;
            this.pos = new Position(x,y);
        }
        public override void Draw()
        {
            Console.SetCursorPosition(pos.x, pos.y);
            Console.ForegroundColor = type == ESnakeBodyType.Head ? ConsoleColor.Green : ConsoleColor.Yellow;
            Console.Write(type == ESnakeBodyType.Head ? "*" : "=");
        }
    }
}
