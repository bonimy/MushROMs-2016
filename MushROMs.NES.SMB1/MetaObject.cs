﻿using System;

namespace MushROMs.NES.SMB1
{
    public struct MetaObject
    {
        public byte Value1
        {
            get;
            private set;
        }
        public byte Value2
        {
            get;
            private set;
        }

        public bool PageFlag
        {
            get;
            private set;
        }

        public int Parameter
        {
            get;
            private set;
        }

        public int Page
        {
            get { return X >> 4; }
        }
        public int X
        {
            get;
            private set;
        }
        public int Y
        {
            get;
            private set;
        }
        public int Width
        {
            get;
            private set;
        }
        public int Height
        {
            get;
            private set;
        }
        public int Left
        {
            get { return Width >= 0 ? X : X - Width; }
        }
        public int Right
        {
            get { return Width >= 0 ? X + Width : X; }
        }
        public int Top
        {
            get { return Height >= 0 ? Y : Y - Height; }
        }
        public int Bottom
        {
            get { return Height >= 0 ? Y + Height : Y; }
        }
        public ObjectType ObjectType
        {
            get;
            private set;
        }

        public bool ContainsPoint(int x, int y)
        {
            switch (ObjectType)
            {
            case ObjectType.QuestionBlockMushroom:
            case ObjectType.QuestionBlockCoin:
            case ObjectType.HiddenBlockCoin:
            case ObjectType.HiddenBlock1UP:
            case ObjectType.BrickMushroom:
            case ObjectType.BrickBeanstalk:
            case ObjectType.BrickStar:
            case ObjectType.Brick10Coins:
            case ObjectType.Brick1UP:
            case ObjectType.UsedBlock:
            case ObjectType.Empty:
            case ObjectType.Empty2:
            case ObjectType.PageSkip:
            case ObjectType.BowserAxe:
            case ObjectType.RopeForAxe:
            case ObjectType.ScrollStopWarpZone:
            case ObjectType.ScrollStop:
            case ObjectType.AltScrollStop:
            case ObjectType.RedCheepCheepFlying:
            case ObjectType.BulletBillGenerator:
            case ObjectType.StopContinuation:
            case ObjectType.LoopCommand:
            case ObjectType.BrickAndSceneryChange:
            case ObjectType.BackgroundChange:
            case ObjectType.Empty3:
            case ObjectType.Empty4:
                return ContainsPointSingleTile(x, y);
            case ObjectType.SidewaysPipe:
            case ObjectType.Trampoline:
            case ObjectType.Cannon:
            case ObjectType.VerticalBricks:
            case ObjectType.VerticalBlocks:
            case ObjectType.RopeForLift:
            case ObjectType.LiftBalanceVerticalRope:
            case ObjectType.VerticalBalls:
                return ContainsPointVertical(x, y);
            case ObjectType.CloudGround:
            case ObjectType.HorizontalBricks:
            case ObjectType.HorizontalBlocks:
            case ObjectType.HorizontalCoins:
            case ObjectType.BalanceHorizontalRope:
            case ObjectType.BridgeV7:
            case ObjectType.BridgeV8:
            case ObjectType.BridgeV10:
            case ObjectType.HorizontalQuestionBlocksV3:
            case ObjectType.HorizontalQuestionBlocksV7:
            case ObjectType.BowserBridge:
                return ContainsPointHorizontal(x, y);
            case ObjectType.UnenterablePipe:
            case ObjectType.EnterablePipe:
            case ObjectType.Hole:
            case ObjectType.HoleWithWater:
            case ObjectType.Castle:
                return ContainPointRectangular(x, y);
            case ObjectType.FlagPole:
            case ObjectType.AltFlagPole:
                return ContainsPointFlagPole(x, y);
            case ObjectType.ReverseLPipe:
            case ObjectType.AltReverseLPipe:
            case ObjectType.LongReverseLPipe:
                return ContainsPointReverseLPipe(x, y);
            case ObjectType.Staircase:
                return ContainsPointStairCase(x, y);
            case ObjectType.GreenIsland:
                return ContainsPointGreenIsalnd(x, y);
            case ObjectType.MushroomIsland:
                return ContainsPointMushroomIsland(x, y);
            default:
                return false;
            }
        }

        public MetaObject(int x, int y, bool pageFlag, ObjectType objectType, int parameter) : this()
        {
            var code = (int)objectType;
            if ((code & ~0xFF) != 0)
                y = code >> 8;

            var page = x >> 4;

            Value1 = (byte)(((x & 0x0F) << 4) | (y & 0x0F));
            Value2 = (byte)(pageFlag ? 0x80 : 0);
            Value2 |= (byte)(code & 0xF0);
            Value2 |= (byte)parameter;
            this = new MetaObject(Value1, Value2, (ObjectMode)(code & 3), ref page);
        }

        public MetaObject(byte value1, byte value2, ObjectMode objectMode, ref int page)
        {
            Value1 = value1;
            Value2 = value2;

            X = value1 >> 4;
            Y = value1 & 0x0F;
            Width = Height = 0;

            if (PageFlag = (value2 & 0x80) != 0)
                page++;

            var code = value2 & 0x7F;
            Parameter = code & 0x0F;
            ObjectType = ObjectType.Invalid;
            switch (Y)
            {
            case 0x0C:
                ObjectType = ObjectType.Hole | (ObjectType)(code & 0x70);
                Width = Parameter;

                switch (ObjectType)
                {
                case ObjectType.Hole:
                    Height = -3;
                    break;
                case ObjectType.BalanceHorizontalRope:
                    Y = 0;
                    break;
                case ObjectType.BridgeV7:
                    Y = 7;
                    Height = -1;
                    break;
                case ObjectType.BridgeV8:
                    Y = 8;
                    Height = -1;
                    break;
                case ObjectType.BridgeV10:
                    Y = 10;
                    Height = -1;
                    break;
                case ObjectType.HoleWithWater:
                    Height = -2;
                    break;
                case ObjectType.HorizontalQuestionBlocksV3:
                    Y = 3;
                    break;
                case ObjectType.HorizontalQuestionBlocksV7:
                    Y = 7;
                    break;
                }
                break;
            case 0x0D:
                ObjectType = ObjectType.PageSkip;
                if (code < 0x40)
                    Parameter = Width = code & 0x3F;
                else
                    ObjectType |= (ObjectType)code;
                break;
            case 0x0E:
                if (code < 0x40)
                {
                    ObjectType = ObjectType.BrickAndSceneryChange;
                    Parameter = code;
                }
                else
                {
                    ObjectType = ObjectType.BackgroundChange;
                    Parameter = (code - 0x40) & 7;
                }
                break;
            case 0x0F:
                ObjectType = ObjectType.RopeForLift | (ObjectType)(code & 0x70);
                Height = Parameter;
                switch (ObjectType)
                {
                case ObjectType.RopeForLift:
                    Y = 0;
                    Height = 0x10;
                    break;
                case ObjectType.LiftBalanceVerticalRope:
                    Y = 1;
                    break;
                case ObjectType.Castle:
                    Y = Height;
                    Width = 4;
                    Height = 10 - Y;
                    break;
                case ObjectType.Staircase:
                    Y = 10 - Height;
                    Width = Height;
                    break;
                }
                break;
            default:
                if (code < 0x10)
                {
                    ObjectType = (ObjectType)code;
                    switch (ObjectType)
                    {
                    case ObjectType.SidewaysPipe:
                    case ObjectType.Trampoline:
                        Height = -1;
                        break;
                    case ObjectType.ReverseLPipe:
                        Height = -3;
                        break;
                    case ObjectType.FlagPole:
                        Height = 0x0A;
                        break;
                    }
                }
                else
                {
                    ObjectType = (ObjectType)(code & 0x70);
                    if (ObjectType == ObjectType.GreenIsland)
                        ObjectType |= (ObjectType)objectMode;

                    switch (ObjectType)
                    {
                    case ObjectType.GreenIsland:
                    case ObjectType.MushroomIsland:
                    case ObjectType.CloudGround:
                    case ObjectType.HorizontalBricks:
                    case ObjectType.HorizontalBlocks:
                        Width = Parameter;
                        break;
                    case ObjectType.Cannon:
                    case ObjectType.VerticalBricks:
                    case ObjectType.VerticalBlocks:
                        Height = Parameter;
                        break;
                    case ObjectType.UnenterablePipe:
                        if ((Parameter & 0x08) != 0)
                            ObjectType = ObjectType.EnterablePipe;
                        Width = 1;
                        Height = Parameter & 0x07;
                        break;
                    }
                }
                break;
            }

            X |= (page << 4);
        }

        private bool ContainsPointSingleTile(int x, int y)
        {
            return x == X && y == Y;
        }

        private bool ContainsPointHorizontal(int x, int y)
        {
            return (x >= Left && x <= Right) && y == Y;
        }

        private bool ContainsPointVertical(int x, int y)
        {
            return x == X && (y >= Top && y <= Bottom);
        }

        private bool ContainPointRectangular(int x, int y)
        {
            return (x >= Left && x <= Right) && (y >= Top && y <= Bottom);
        }

        private bool ContainsPointFlagPole(int x, int y)
        {
            if (x == X - 1 && y == 0)
                return true;
            return x == X && (y >= 0 && y <= 0x0A);
        }

        private bool ContainsPointReverseLPipe(int x, int y)
        {
            if ((y >= Y && y <= Y + Math.Min(Height, 2)) && (x >= X + 2 && x < X + 4))
                return true;
            if (Height == 0)
                return false;
            return (x >= X && x < X + 2) && (y >= Y + Height && y < Y + Height + 2);
        }

        private bool ContainsPointStairCase(int x, int y)
        {
            if (Width == 0x0F && x == X + Width && y == Y + 4)
                return true;
            if (x == X + 8 && Width == 8)
                return y >= Y && y < +8;
            return (x >= X && x <= X + Width) && (y >= Y + 7 && y <= Y + 7 - (x - X));
        }

        private bool ContainsPointGreenIsalnd(int x, int y)
        {
            if (y == Y && (x >= X & x <= X + Width))
                return true;
            return (Y > y && (x > X && x < X + Width));
        }

        private bool ContainsPointMushroomIsland(int x, int y)
        {
            if (y == Y && (x >= X & x <= X + Width))
                return true;
            return (Y > y && Width >= 2 && (x == X + (Width / 2)));
        }

        public static bool operator ==(MetaObject left, MetaObject right)
        {
            return left.Value1 == right.Value1 &&
                left.Value2 == right.Value2;
        }
        public static bool operator !=(MetaObject left, MetaObject right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is MetaObject))
                return false;

            return (MetaObject)obj == this;
        }

        public override string ToString()
        {
            return String.Format("({0}, {1}): {2}: {3}", X.ToString("X2"), Y.ToString("X"), ObjectType);
        }
    }
}
