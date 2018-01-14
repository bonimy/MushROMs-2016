using System;
using System.Collections.Generic;

namespace MushROMs.Assembler
{
    public delegate int Calculation();

    public delegate int BinaryOperation(int left, int right);

    public class BinaryExpression
    {
        private static Dictionary<BinaryOperator, BinaryOperation> _binaryOperations;

        private static Dictionary<BinaryOperator, BinaryOperation> BinaryOperations
        {
            get
            {
                if (_binaryOperations == null)
                {
                    _binaryOperations = new Dictionary<BinaryOperator, BinaryOperation>
                    {
                        { BinaryOperator.None, (x, y) => x },
                        { BinaryOperator.Addition, (x, y) => x + y },
                        { BinaryOperator.Subtraction, (x, y) => x - y },
                        { BinaryOperator.Multiplication, (x, y) => x * y },
                        { BinaryOperator.Division, (x, y) => x / y },
                        { BinaryOperator.Modulo, (x, y) => x % y },
                        { BinaryOperator.BitShiftLeft, (x, y) => x << y },
                        { BinaryOperator.BitShiftRight, (x, y) => x >> y },
                        { BinaryOperator.BitwiseAND, (x, y) => x & y },
                        { BinaryOperator.BitwiseOR, (x, y) => x | y },
                        { BinaryOperator.BitwiseXOR, (x, y) => x ^ y }
                    };
                }
                return _binaryOperations;
            }
        }

        public Calculation Left
        {
            get;
            set;
        }

        public Calculation Right
        {
            get;
            set;
        }

        public BinaryOperation Operation
        {
            get;
            set;
        }

        public Calculation Result
        {
            get { return () => Operation(Left(), Right()); }
        }

        public BinaryExpression(Calculation left, Calculation right, BinaryOperator operation) :
            this(left, right, GetBinaryOperation(operation))
        { }

        public BinaryExpression(Calculation left, Calculation right, BinaryOperation operation)
        {
            Left = left ?? throw new ArgumentNullException(nameof(left));
            Right = right ?? throw new ArgumentNullException(nameof(right));
            Operation = operation ?? throw new ArgumentNullException(nameof(operation));
        }

        public static BinaryOperation GetBinaryOperation(BinaryOperator value)
        {
            if (!BinaryOperations.ContainsKey(value))
            {
                return null;
            }

            return BinaryOperations[value];
        }
    }
}
