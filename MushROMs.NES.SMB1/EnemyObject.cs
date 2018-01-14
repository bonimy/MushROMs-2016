namespace MushROMs.NES.SMB1
{
    public struct EnemyObject
    {
        public byte Value1
        {
            get;
            set;
        }

        public byte Value2
        {
            get;
            set;
        }

        public byte Value3
        {
            get;
            set;
        }

        public int Size
        {
            get { return Y == 0x0E ? 3 : 2; }
        }

        public int X
        {
            get
            {
                return Value1 >> 4;
            }

            set
            {
                Value1 &= 0x0F;
                Value1 |= (byte)((value & 0x0F) << 4);
            }
        }

        public int Y
        {
            get
            {
                return Value1 & 0x0F;
            }

            set
            {
                Value1 &= 0xF0;
                Value1 |= (byte)(value & 0x0F);
            }
        }

        public bool PageFlag
        {
            get
            {
                return (Value2 & 0x80) != 0;
            }

            set
            {
                if (value)
                {
                    Value2 |= 0x80;
                }
                else
                {
                    Value2 &= 0x7F;
                }
            }
        }

        public bool HardWorldFlag
        {
            get
            {
                return (Value2 & 0x40) != 0;
            }

            set
            {
                if (value)
                {
                    Value2 |= 0x40;
                }
                else
                {
                    Value2 &= 0xBF;
                }
            }
        }

        public int Command
        {
            get
            {
                return Value2 & 0x3F;
            }

            set
            {
                Value2 &= 0xC0;
                Value2 |= (byte)(value & 0x3F);
            }
        }

        public EnemyObject(byte value1, byte value2, byte value3)
        {
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
        }

        public static bool operator ==(EnemyObject left, EnemyObject right)
        {
            if (left.Size == 2 && right.Size == 2)
            {
                return left.Value1 == right.Value1 &&
                    left.Value2 == right.Value2;
            }
            else if (left.Size == 3 && right.Size == 3)
            {
                return left.Value1 == right.Value1 &&
                    left.Value2 == right.Value2 &&
                    left.Value3 == right.Value3;
            }
            else
            {
                return false;
            }
        }

        public static bool operator !=(EnemyObject left, EnemyObject right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is EnemyObject))
            {
                return false;
            }

            return (EnemyObject)obj == this;
        }

        public override int GetHashCode()
        {
            return Size == 3 ?
                (Value1) | (Value2 << 8) | (Value3 << 0x10) :
                (Value1) | (Value2 << 8);
        }

        public override string ToString()
        {
            return System.String.Format("({0}, {1}): {2}", X.ToString("X"), Y.ToString("X"), Command);
        }
    }
}
