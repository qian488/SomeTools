using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    internal class Map : IDraw
    {
        public Wall[] walls;
        public Map()
        {
            walls = new Wall[Game.width * 2 + (Game.height - 3) * 4];

            int index = 0; // 索引墙的个数

            for(int i = 0; i < Game.width; i ++)
            {
                walls[index] = new Wall(i, 0); // 最上方
                index++;

                walls[index] = new Wall(i, Game.height - 2); // 最下方
                index++;
            }

            for(int i = 1;i < Game.height - 2; i++)
            {
                walls[index] = new Wall(0, i); // 左边两排
                index++;
                walls[index] = new Wall(1, i);
                index++;

                walls[index] = new Wall(Game.width - 1, i); // 右边两排
                index++;
                walls[index] = new Wall(Game.width - 2, i);
                index++;
            }
        }

        public void Draw()
        {
            foreach(Wall wall in walls)
            {
                wall.Draw();
            }
        }
    }
}
