using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    enum EMoveDir
    {
        up,
        down,
        left,
        right,
    }
    internal class Snake : IDraw
    {
        SnakeBody[] bodys;
        int nowNum;
        EMoveDir moveDir;

        public Snake(int x,int y)
        {
            bodys = new SnakeBody[200];
            bodys[0] = new SnakeBody(ESnakeBodyType.Head, x, y);
            nowNum = 1;

            moveDir = EMoveDir.right;
        }
        public void Draw()
        {
            for(int i = 0; i < nowNum; i++)
            {
                bodys[i].Draw();
            }
        }

        public void Move()
        {
            SnakeBody lastBody = bodys[nowNum - 1];
            Console.SetCursorPosition(lastBody.pos.x, lastBody.pos.y);
            Console.Write(" ");

            for(int i = nowNum - 1; i > 0; i--)
            {
                bodys[i].pos = bodys[i - 1].pos;
            }

            switch(moveDir)
            {
                case EMoveDir.up:
                    bodys[0].pos.y--;
                    break;
                case EMoveDir.down:
                    bodys[0].pos.y++;
                    break;
                case EMoveDir.left:
                    bodys[0].pos.x--;
                    break;
                case EMoveDir.right:
                    bodys[0].pos.x++;
                    break;
            }
        }

        public void ChangeDir(EMoveDir dir)
        {
            if(dir == this.moveDir ||
                nowNum > 1 &&
                (this.moveDir == EMoveDir.left && dir == EMoveDir.right ||
                 this.moveDir == EMoveDir.right && dir == EMoveDir.left ||
                 this.moveDir == EMoveDir.up && dir == EMoveDir.down ||
                 this.moveDir ==EMoveDir.down && dir == EMoveDir.up))
            {
                return;
            }

            this.moveDir = dir;
        }

        public bool CheckEnd(Map map)
        {
            for(int i = 0; i < map.walls.Length; i++)
            {
                if (bodys[0].pos == map.walls[i].pos)
                {
                    return true;
                }
            }

            for(int i = 1; i < nowNum; i++)
            {
                if (bodys[0].pos == bodys[i].pos)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CheckSamePos(Position pos)
        {
            for (int i = 1; i < nowNum; i++)
            {
                if (pos == bodys[i].pos)
                {
                    return true;
                }
            }
            return false;
        }

        public void CheckEatFood(Food food)
        {
            if( bodys[0].pos == food.pos)
            {
                food.RandomPos(this);
                AddBody();
            }
        }

        private void AddBody()
        {
            SnakeBody frontBody = bodys[nowNum - 1];
            bodys[nowNum] = new SnakeBody(ESnakeBodyType.Body, frontBody.pos.x, frontBody.pos.y);
            nowNum++;
        }
    }
}
