using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class Map : IDraw
    {
        public List<DrawObject> walls = new List<DrawObject>();
        public List<DrawObject> dynamicWalls = new List<DrawObject>();

        public int width = Game.width - 2;
        public int height = Game.height - 6;

        public int cnt;

        private int[] recordInfo;

        private GameScene nowGameScene;

        public Map(GameScene scene)
        {
            this.nowGameScene = scene;
            recordInfo = new int[height];

            cnt = 0;
            for (int i = 0; i < Game.width; i += 2)
            {
                walls.Add(new DrawObject(E_DrawType.Wall, i, height));
                cnt++;
            }
            cnt -= 2;

            for (int i = 0; i < height; i++)
            {
                walls.Add(new DrawObject(E_DrawType.Wall, 0, i));
                walls.Add(new DrawObject(E_DrawType.Wall, width, i));
            }
        }

        public void Draw()
        {
            for (int i = 0; i < walls.Count; i++)
            {
                walls[i].Draw();
            }

            for (int i = 0; i < dynamicWalls.Count; i++)
            {
                dynamicWalls[i].Draw();
            }
        }

        public void ClearDynamicWall()
        {
            for (int i = 0; i < dynamicWalls.Count; i++)
            {
                dynamicWalls[i].Clear();
            }
        }

        public void AddDynamicWall(List<DrawObject> walls)
        {
            for (int i = 0; i < walls.Count; i++)
            {
                walls[i].ChangeType(E_DrawType.Wall);
                dynamicWalls.Add(walls[i]);

                if (walls[i].GetPosition().y <= 0)
                {
                    this.nowGameScene?.StopInputThread();

                    Game.ChangeScene(ESceneType.End);
                    return;
                }

                recordInfo[height - 1 - walls[i].GetPosition().y] += 1;
            }
            ClearDynamicWall();
            CheckClear();
            Draw();
        }

        public void CheckClear()
        {
            List<DrawObject> delList = new List<DrawObject>();
            for (int i = 0; i < recordInfo.Length; i++)
            {
                if (recordInfo[i] == cnt)
                {
                    for (int j = 0; j < dynamicWalls.Count; j++)
                    {
                        if (i == height - 1 - dynamicWalls[j].GetPosition().y)
                        {
                            delList.Add(dynamicWalls[j]);
                        }
                        else if (i < height - 1 - dynamicWalls[j].GetPosition().y)
                        {
                            dynamicWalls[j].SetPosition(dynamicWalls[j].GetPosition().x, dynamicWalls[j].GetPosition().y + 1);
                        }
                    }
                    for (int j = 0; j < delList.Count; j++)
                    {
                        dynamicWalls.Remove(delList[j]);
                    }
                    for (int j = i; j < recordInfo.Length - 1; j++)
                    {
                        recordInfo[j] = recordInfo[j + 1];
                    }
                    recordInfo[recordInfo.Length - 1] = 0;
                    CheckClear();
                    break;
                }
            }
        }
    }
}