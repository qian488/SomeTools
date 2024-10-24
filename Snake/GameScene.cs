using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class GameScene : ISceneUpdate
    {
        Map map;
        Snake snake;
        Food food;
        int updateindex = 0;

        public GameScene()
        {
            map = new Map();
            snake = new Snake(Game.width / 2 ,Game.height / 2);
            food = new Food(snake);
        }
        public void Update()
        {
            if(updateindex % 6000 == 0)
            {
                map.Draw();
                food.Draw();
                snake.Move();
                snake.Draw();

                if(snake.CheckEnd(map))
                {
                    Game.ChangeScene(ESceneType.End);
                }

                snake.CheckEatFood(food);

                updateindex = 0;
            }
            updateindex++;

            if(Console.KeyAvailable)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.W:
                        snake.ChangeDir(EMoveDir.up); 
                        break;
                    case ConsoleKey.A:
                        snake.ChangeDir(EMoveDir.left);
                        break;
                    case ConsoleKey.S:
                        snake.ChangeDir(EMoveDir.down);
                        break;
                    case ConsoleKey.D:
                        snake.ChangeDir(EMoveDir.right);
                        break;
                }
            }
        }
    }
}
