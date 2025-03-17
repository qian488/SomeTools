using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    enum E_Change_Type
    {
        Left,
        Right,
    }
    class BlockWorker : IDraw
    {
        private List<DrawObject> blocks;
        private Dictionary<E_DrawType, BlockInfo> blocksInfo;
        private BlockInfo nowBlockInfo;
        private int nowBlockIndex;
        private Map map;

        public BlockWorker(Map map)
        {
            this.map = map;
            blocksInfo = new Dictionary<E_DrawType, BlockInfo>()
                {
                    { E_DrawType.Cube, new BlockInfo(E_DrawType.Cube) },
                    { E_DrawType.Line, new BlockInfo(E_DrawType.Line) },
                    { E_DrawType.Tank, new BlockInfo(E_DrawType.Tank) },
                    { E_DrawType.Left_Ladder, new BlockInfo(E_DrawType.Left_Ladder) },
                    { E_DrawType.Right_Ladder, new BlockInfo(E_DrawType.Right_Ladder) },
                    { E_DrawType.Left_Long_Ladder, new BlockInfo(E_DrawType.Left_Long_Ladder) },
                    { E_DrawType.Right_Long_Ladder, new BlockInfo(E_DrawType.Right_Long_Ladder) }
                };
            RandomCreateBlock();
        }

        public void Draw()
        {
            for (int i = 0; i < blocks.Count; i++)
            {
                blocks[i].Draw();
            }
        }

        public void RandomCreateBlock()
        {
            Random random = new Random();
            E_DrawType type = (E_DrawType)random.Next(1, 8);
            blocks =
                [
                    new DrawObject(type),
                    new DrawObject(type),
                    new DrawObject(type),
                    new DrawObject(type),
                ];

            int pos = random.Next(4, map.width - 2) / 2 * 2;
            blocks[0].SetPosition(pos, -1);

            nowBlockInfo = blocksInfo[type];
            nowBlockIndex = random.Next(0, nowBlockInfo.Count);
            Position[] positions = nowBlockInfo[nowBlockIndex];
            for (int i = 0; i < positions.Length; i++)
            {
                blocks[i + 1].SetPosition(blocks[0].GetPosition() + positions[i]);
            }
        }

        public void Clear()
        {
            for (int i = 0; i < blocks.Count; i++)
            {
                blocks[i].Clear();
            }
        }

        public void Change(E_Change_Type type)
        {
            Clear();
            switch (type)
            {
                case E_Change_Type.Left:
                    nowBlockIndex -= 1;
                    if (nowBlockIndex < 0)
                    {
                        nowBlockIndex = nowBlockInfo.Count - 1;
                    }
                    break;
                case E_Change_Type.Right:
                    nowBlockIndex += 1;
                    if (nowBlockIndex >= nowBlockInfo.Count)
                    {
                        nowBlockIndex = 0;
                    }
                    break;
            }
            Position[] positions = nowBlockInfo[nowBlockIndex];
            for (int i = 0; i < positions.Length; i++)
            {
                blocks[i + 1].SetPosition(blocks[0].GetPosition() + positions[i]);
            }
            Draw();
        }

        public bool CanChange(E_Change_Type type)
        {
            int nowIndex = nowBlockIndex;
            switch (type)
            {
                case E_Change_Type.Left:
                    nowIndex -= 1;
                    if (nowIndex < 0)
                    {
                        nowIndex = nowBlockInfo.Count - 1;
                    }
                    break;
                case E_Change_Type.Right:
                    nowIndex += 1;
                    if (nowIndex >= nowBlockInfo.Count)
                    {
                        nowIndex = 0;
                    }
                    break;
            }
            Position[] nowPos = nowBlockInfo[nowIndex];
            Position tempPos;
            for (int i = 0; i < nowPos.Length; i++)
            {
                tempPos = blocks[0].GetPosition() + nowPos[i];
                if (tempPos.x < 2 || tempPos.x >= map.width || tempPos.y >= map.height)
                {
                    return false;
                }
            }
            for (int i = 0; i < nowPos.Length; i++)
            {
                tempPos = blocks[0].GetPosition() + nowPos[i];
                for (int j = 0; j < map.dynamicWalls.Count; j++)
                {
                    if (map.dynamicWalls[j].GetPosition() == tempPos)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void MoveLR(E_Change_Type type)
        {
            Clear();
            Position movePos = new Position(type == E_Change_Type.Left ? -2 : 2, 0);
            for (int i = 0; i < blocks.Count; i++)
            {
                blocks[i].SetPosition(blocks[i].GetPosition() + movePos);
            }
            Draw();
        }

        public bool CanMoveLR(E_Change_Type type)
        {
            Position movePos = new Position(type == E_Change_Type.Left ? -2 : 2, 0);
            Position tempPos;
            for (int i = 0; i < blocks.Count; i++)
            {
                tempPos = blocks[i].GetPosition() + movePos;
                if (tempPos.x < 2 || tempPos.x >= map.width || tempPos.y >= map.height)
                {
                    return false;
                }
            }
            for (int i = 0; i < blocks.Count; i++)
            {
                tempPos = blocks[i].GetPosition() + movePos;
                for (int j = 0; j < map.dynamicWalls.Count; j++)
                {
                    if (map.dynamicWalls[j].GetPosition() == tempPos)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void AutoMoveDown()
        {
            Clear();
            Position movePos = new Position(0, 1);
            for (int i = 0; i < blocks.Count; i++)
            {
                blocks[i].SetPosition(blocks[i].GetPosition() + movePos);
            }
            Draw();
        }

        public bool CanAutoMoveDown()
        {
            Position movePos = new Position(0, 1);
            Position tempPos;
            for (int i = 0; i < blocks.Count; i++)
            {
                tempPos = blocks[i].GetPosition() + movePos;
                if (tempPos.y >= map.height)
                {
                    map.AddDynamicWall(blocks);
                    RandomCreateBlock();
                    return false;
                }
            }
            for (int i = 0; i < blocks.Count; i++)
            {
                tempPos = blocks[i].GetPosition() + movePos;
                for (int j = 0; j < map.dynamicWalls.Count; j++)
                {
                    if (map.dynamicWalls[j].GetPosition() == tempPos)
                    {
                        map.AddDynamicWall(blocks);
                        RandomCreateBlock();
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
