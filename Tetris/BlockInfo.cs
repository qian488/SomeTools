using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class BlockInfo
    {
        private List<Position[]> positions = new List<Position[]>();

        public BlockInfo(E_DrawType type)
        {
            switch (type)
            {
                case E_DrawType.Cube:
                    positions.Add(new Position[] {new Position(2, 0), new Position(0, 1), new Position(2, 1) });
                    break;
                case E_DrawType.Line:
                    positions.Add(new Position[] { new Position(0, -1), new Position(0, 1), new Position(0, 2) });
                    positions.Add(new Position[] { new Position(-4, 0), new Position(-2, 0), new Position(2, 0) });
                    positions.Add(new Position[] { new Position(0, -2), new Position(0, -1), new Position(0, 1) });
                    positions.Add(new Position[] { new Position(-2, 0), new Position(2, 0), new Position(4, 0) });
                    break;
                case E_DrawType.Tank:
                    positions.Add(new Position[] { new Position(-2, 0), new Position(2, 0), new Position(0, 1) });
                    positions.Add(new Position[] { new Position(-2, 0), new Position(0, -1), new Position(0, 1) });
                    positions.Add(new Position[] { new Position(-2, 0), new Position(0, -1), new Position(2, 0) });
                    positions.Add(new Position[] { new Position(0, -1), new Position(2, 0), new Position(0, 1) });
                    break;
                case E_DrawType.Left_Ladder:
                    positions.Add(new Position[] {new Position(0, -1), new Position(2, 0), new Position(2, 1) });
                    positions.Add(new Position[] {new Position(2, 0), new Position(0, 1), new Position(-2, 1) });
                    positions.Add(new Position[] { new Position(-2, -1), new Position(0, 1), new Position(-2, 0) });
                    positions.Add(new Position[] { new Position(-2, 0), new Position(0, -1), new Position(2, -1) });
                    break;
                case E_DrawType.Right_Ladder:
                    positions.Add(new Position[] { new Position(0, -1), new Position(-2, 1), new Position(-2, 0) });
                    positions.Add(new Position[] { new Position(-2, -1), new Position(0, -1), new Position(2, 0) });
                    positions.Add(new Position[] { new Position(2, -1), new Position(2, 0), new Position(0, 1) });
                    positions.Add(new Position[] { new Position(-2, 0), new Position(2, 1), new Position(0, 1) });
                    break;
                case E_DrawType.Left_Long_Ladder:
                    positions.Add(new Position[] { new Position(0, -1), new Position(2, -1), new Position(0, 1) });
                    positions.Add(new Position[] { new Position(-2, 0), new Position(2, 0), new Position(2, 1) });
                    positions.Add(new Position[] { new Position(0, -1), new Position(0, 1), new Position(-2, 1) });
                    positions.Add(new Position[] { new Position(-2, -1), new Position(2, 0), new Position(-2, 0) });
                    break;
                case E_DrawType.Right_Long_Ladder:
                    positions.Add(new Position[] { new Position(-2, -1), new Position(0, -1), new Position(0, 1) });
                    positions.Add(new Position[] { new Position(2, -1), new Position(2, 0), new Position(-2, 0) });
                    positions.Add(new Position[] { new Position(0, -1), new Position(2, 1), new Position(0, 1) });
                    positions.Add(new Position[] { new Position(-2, 0), new Position(2, 0), new Position(-2, 1) });
                    break;
            }
        }

        public Position[] this[int index]
        {
            get
            {
                if(index < 0)
                {
                    return positions[0];
                }else if (index >= positions.Count)
                {
                    return positions[positions.Count - 1];
                }
                else
                {
                    return positions[index];
                }
            }
        }
        public int Count { get => positions.Count; }
    }
}
