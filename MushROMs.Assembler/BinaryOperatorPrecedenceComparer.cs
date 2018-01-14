using System.Collections.Generic;

namespace MushROMs.Assembler
{
    public class BinaryOperatorPrecedenceComparer : IComparer<MathExpression>
    {
        public static readonly BinaryOperatorPrecedenceComparer Default = new BinaryOperatorPrecedenceComparer();

        private static Dictionary<BinaryOperator, int> _precedence;

        private static Dictionary<BinaryOperator, int> Precedence
        {
            get
            {
                if (_precedence == null)
                {
                    _precedence = new Dictionary<BinaryOperator, int>
                    {
                        { BinaryOperator.Multiplication, 5 },
                        { BinaryOperator.Division, 5 },
                        { BinaryOperator.Modulo, 5 },
                        { BinaryOperator.Addition, 6 },
                        { BinaryOperator.Subtraction, 6 },
                        { BinaryOperator.BitShiftRight, 7 },
                        { BinaryOperator.BitShiftLeft, 8 },
                        { BinaryOperator.BitwiseAND, 10 },
                        { BinaryOperator.BitwiseXOR, 11 },
                        { BinaryOperator.BitwiseOR, 12 }
                    };
                }
                return _precedence;
            }
        }

        internal static bool IsKnownOperator(BinaryOperator value)
        {
            return Precedence.ContainsKey(value);
        }

        internal static int GetPrecedence(BinaryOperator value)
        {
            if (IsKnownOperator(value))
            {
                return Precedence[value];
            }

            return -1;
        }

        public static int CompareExpressions(MathExpression x, MathExpression y)
        {
            if (x == null && y == null)
            {
                return 0;
            }

            if (x == null)
            {
                return +1;
            }

            if (y == null)
            {
                return -1;
            }

            var left = x.BinaryOperator;
            var right = y.BinaryOperator;

            if (!Precedence.ContainsKey(left) && !Precedence.ContainsKey(left))
            {
                return 0;
            }

            if (!Precedence.ContainsKey(left))
            {
                return +1;
            }

            if (!Precedence.ContainsKey(right))
            {
                return -1;
            }

            return Precedence[left] - Precedence[right];
        }

        public int Compare(MathExpression x, MathExpression y)
        {
            return CompareExpressions(x, y);
        }
    }
}
