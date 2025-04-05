using MathNet.Symbolics;
using System.Numerics;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace WorldEngine.Math
{
    public class Ellipse<T> : IEquatable<Ellipse<T>>
    {
        public required T semiMajorAxisA;
        public required T semiMinorAxisB;

        #region Equality Members

        public override bool Equals(object? obj)
        {
            return Equals(obj as Ellipse<T>);
        }


        public override int GetHashCode()
        {
            return HashCode.Combine(semiMajorAxisA, semiMinorAxisB);
        }

        public bool Equals(Ellipse<T>? other)
        {
            if (other is null)
                return false;

            bool semiMajorsEqual = semiMajorAxisA?.ToString()?.Equals(other.semiMajorAxisA?.ToString()) ?? false;
            bool semiMinorsEqual = semiMinorAxisB?.ToString()?.Equals(other?.semiMinorAxisB?.ToString()) ?? false;

            return semiMajorsEqual && semiMinorsEqual;
        }

        public static bool operator ==(Ellipse<T> left, Ellipse<T> right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (left is null)
                return false;
            return left.Equals(right);
        }

        public static bool operator !=(Ellipse<T> left, Ellipse<T> right)
        {
            return !(left == right);
        }

        #endregion
    }

    public static class EllipseExtensions
    {
        public static T EccentricityE<T>(this Ellipse<T> ellipse)
            where T : IFloatingPoint<T>, IRootFunctions<T>
        {
            T ratio = ellipse.semiMinorAxisB / ellipse.semiMajorAxisA;
            T squaredRatio = ratio * ratio;

            return T.Sqrt(T.One - squaredRatio);
        }

        public static T EccentricityEExpr<T>(this Ellipse<T> ellipse)
            where T : Expr
        {
            T ratio = (T)(ellipse.semiMinorAxisB / ellipse.semiMajorAxisA);
            T squaredRatio = (T)(ratio * ratio);


            return (T)Expr.Parse("sqrt(1-(" + squaredRatio.ToString() + "))");
        }

        public static T? PerimeterP<T>(this Ellipse<T> ellipse)
    where T : IFloatingPoint<T>, IRootFunctions<T>
        {
            T eSquared = ellipse.EccentricityE() * ellipse.EccentricityE();

            // Create a properly formatted Wolfram expression
            string wolframExpr = $"4*{ellipse.semiMajorAxisA}*EllipticE[{eSquared}]";

            // Execute Wolfram command
            var inp = WolframEngine.ExecuteToInputForm(wolframExpr, 0);

            var res = WolframEngine.ExecuteToOutput(inp, 0);

            // Attempt parsing directly into T
            if (T.TryParse(res, null, out var result))
                return result;

            // If parsing fails, return null
            return default;
        }

        public static T PerimeterPExpr<T>(this Ellipse<T> ellipse)
            where T : Expr
        {
            Expr eSquared = ellipse.EccentricityEExpr() * ellipse.EccentricityEExpr();

            // Create a properly formatted Wolfram expression
            string wolframExpr = $"4*{ellipse.semiMajorAxisA}*EllipticE[{eSquared}]";

            // Execute Wolfram command
            // Execute Wolfram command
            var inp = WolframEngine.ExecuteToInputForm(wolframExpr, 0);

            var res = WolframEngine.ExecuteToOutput("N["+inp+"]", 0);

            // Attempt parsing directly into T
            return (T)Expr.Parse(res);
        }
    }
}
