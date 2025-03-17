using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    enum E_DrawType
    {
        Wall,
        Cube,
        Line,
        Tank,
        Left_Ladder, 
        Right_Ladder,
        Left_Long_Ladder,
        Right_Long_Ladder,
    }
    class DrawObject : IDraw
    {
        private Position pos;
        private E_DrawType type;
        public DrawObject(E_DrawType type)
        {
            this.type = type;
        }
        public DrawObject(E_DrawType type, int x,int y):this(type)
        {
            this.pos = new Position(x,y);
        }
        public void Draw()
        {
            if (pos.y < 0) return;
            Console.SetCursorPosition(pos.x, pos.y);

            switch(type)
            {
                case E_DrawType.Wall:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case E_DrawType.Cube:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case E_DrawType.Line:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case E_DrawType.Tank:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case E_DrawType.Left_Ladder:
                case E_DrawType.Right_Ladder:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case E_DrawType.Left_Long_Ladder:
                case E_DrawType.Right_Long_Ladder:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
            }

            Console.Write("■");
        }

        public void Clear()
        {
            if (pos.y < 0) return;
            Console.SetCursorPosition(pos.x, pos.y);
            Console.Write("  "); // 两个空格
        }

        /// <summary>
        /// 主要用于落地后的方块变成墙
        /// </summary>
        /// <param name="type"></param>
        public void ChangeType(E_DrawType type)
        {
            this.type = type;
        }

        public void SetPosition(int x, int y)
        {
            this.pos = new Position(x, y);
        }

        public void SetPosition(Position pos)
        {
            this.pos = pos;
        }

        public Position GetPosition()
        {
            return pos;
        }
    }
}
