using MathNet.Spatial.Euclidean;
using MathNet.Symbolics;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace WorldEngine
{
    /// <summary>
    /// Vector3D is numerical - this class is symbolic so we can see full expressions.
    /// </summary>
    public class Symbolic3D:IEquatable<Symbolic3D>
    {
        public Symbolic3D()
        {
            X = Expr.Zero;
            Y = Expr.Zero;
            Z = Expr.Zero;
        }

        public Symbolic3D(Expr x, Expr y, Expr z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// The X Component.
        /// </summary>
        public Expr X { get; set; }

        /// <summary>
        /// The Y Component.
        /// </summary>
        public Expr Y { get; set; }

        /// <summary>
        /// The Z Component.
        /// </summary>
        public Expr Z { get; set; }

        public static Symbolic3D operator *(Symbolic3D v1, double c)
        {
            return new Symbolic3D(v1.X * c, v1.Y * c, v1.Z * c);
        }

        public static Symbolic3D operator *(Symbolic3D v1, Symbolic3D v2)
        {
            return new Symbolic3D(v1.X * v2.X, v1.Y * v2.Y, v1.Z * v2.Z);
        }

        public static Symbolic3D operator +(Symbolic3D v1, Symbolic3D v2)
        {
            return new Symbolic3D(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Symbolic3D operator +(Symbolic3D v1, double c)
        {
            return new Symbolic3D(v1.X + c, v1.Y + c, v1.Z + c);
        }

        public static Symbolic3D operator /(Symbolic3D v1, Symbolic3D v2)
        {
            return new Symbolic3D(v1.X / v2.X, v1.Y / v2.Y, v1.Z / v2.Z);
        }

        public static Symbolic3D operator /(Symbolic3D v1, double c)
        {
            return new Symbolic3D(v1.X / c, v1.Y / c, v1.Z / c);
        }

        public static Symbolic3D operator -(Symbolic3D v1, Symbolic3D v2)
        {
            return new Symbolic3D(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        public static Symbolic3D operator -(Symbolic3D v1, double c)
        {
            return new Symbolic3D(v1.X - c, v1.Y - c, v1.Z - c);
        }

        public Symbolic3D CrossProduct(Symbolic3D v2)
        {
            return new Symbolic3D(Y * v2.Z - Z * v2.Y, Z * v2.X - X * v2.Z, X * v2.Y - Y * v2.X);
        }

        public Expr DotProduct(Symbolic3D v2)
        {
            return X*v2.X+Y*v2.Y+Z*v2.Z;
        }

        /// <summary>
        /// Hacks the definition of a derivative to get a approximation of velocity at a certain time using a small interval
        /// Ginsberg, Jerry.Engineering Dynamics(pp. 12). Cambridge University Press. Kindle Edition.
        /// </summary>
        /// <param name="time">Time at which to evaluate the derivative.</param>
        /// <param name="interval">dt: The small length of time over which a change occurs ie 1 ms.</param>
        /// <returns></returns>
        public Vector3D GetFiniteCentralDifferenceApproximation(double time, double interval)
        {
            var partOne = SolveAtTime(time + interval / 2.0);
            var partTwo = SolveAtTime(time - interval / 2.0);
            return (partOne - partTwo) / interval;
        }

        /// <summary>
        /// Evaluates the vector expression at the given time.
        /// </summary>
        /// <param name="time">Time to evaluate the symbolic expression at to get a constant.</param>
        /// <returns></returns>
        public Vector3D SolveAtTime(double time)
        {
            var symbols = new Dictionary<string, FloatingPoint> { { "t", time } };
            return new Vector3D(X.Evaluate(symbols).RealValue, Y.Evaluate(symbols).RealValue, Z.Evaluate(symbols).RealValue);
        }

        /// <summary>
        /// Returns the position vector based on distance, angle in the horizontal xy plane, and angle of elevation with respect to time.
        /// APPROXIMATELY BECAUSE: Does not consider analytical derivatives.
        /// Ginsberg, Jerry.Engineering Dynamics(p. 11). Cambridge University Press. Kindle Edition.
        /// </summary>
        /// <param name="r">Distance with respect to time. r(t)</param>
        /// <param name="theta">Angle in the horizontal xy plane in radians. theta(t)</param>
        /// <param name="beta">Angle of elevation in radians. beta(t)</param>
        /// <returns>Position Vector based on given parameters.</returns>
        /// <remarks>Formula (1) in example 1.2</remarks>
        public static Symbolic3D PositionVectorApprox(Expr r, Expr theta, Expr beta)
        {
            var cosBeta = Expr.Parse("cos(" + beta.ToString() + ")");
            var cosTheta = Expr.Parse("cos(" + theta.ToString() + ")");
            var sinBeta = Expr.Parse("sin(" + beta.ToString() + ")");
            var sinTheta = Expr.Parse("sin(" + theta.ToString() + ")");
            return new Symbolic3D(r * cosBeta * cosTheta, r * cosBeta * sinTheta, r * sinBeta);
        }

        /// <summary>
        /// Returns the position vector based on distance, angle in the horizontal xy plane, and angle of elevation with respect to time.
        /// Ginsberg, Jerry.Engineering Dynamics(p. 12). Cambridge University Press. Kindle Edition.
        /// </summary>
        /// <param name="r">Distance with respect to time. r(t)</param>
        /// <param name="theta">Angle in the horizontal xy plane in radians. theta(t)</param>
        /// <param name="beta">Angle of elevation in radians. beta(t)</param>
        /// <returns>Position Vector based on given parameters.</returns>
        /// <remarks>Formula (1) in example 1.2</remarks>
        public static Symbolic3D PositionVector(Expr r, Expr theta, Expr beta)
        {
            var cosBeta = Expr.Parse("cos(" + beta.ToString() + ")");
            var cosTheta = Expr.Parse("cos(" + theta.ToString() + ")");
            var sinBeta = Expr.Parse("sin(" + beta.ToString() + ")");
            var sinTheta = Expr.Parse("sin(" + theta.ToString() + ")");
            var thetaDot = theta.Differentiate(Expr.Variable("t"));
            var betaDot = beta.Differentiate(Expr.Variable("t"));
            var rDot = r.Differentiate(Expr.Variable("t"));

            var partOne = new Symbolic3D(rDot * cosBeta * cosTheta, rDot * cosBeta * sinTheta, rDot * sinBeta);
            var partTwo = new Symbolic3D(thetaDot * -(r * cosBeta * sinTheta), thetaDot * r * cosBeta * cosTheta, Expr.Zero);
            var partThree = new Symbolic3D(betaDot * -(r * sinBeta * cosTheta), betaDot * -(r * sinBeta * sinTheta), betaDot * r * cosBeta);
            return partOne + partTwo + partThree;
        }

        #region Equality Members


        public override bool Equals(object? obj)
        {
            return Equals(obj as Symbolic3D);
        }


        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }

        public bool Equals(Symbolic3D? other)
        {
            if (other is null)
                return false;

            // Assuming that Expr provides a robust Equals method.
            return X.ToString().Equals(other.X.ToString()) && Y.ToString().Equals(other.Y.ToString()) && Z.ToString().Equals(other.Z.ToString());
        }

        public static bool operator ==(Symbolic3D left, Symbolic3D right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (left is null)
                return false;
            return left.Equals(right);
        }

        public static bool operator !=(Symbolic3D left, Symbolic3D right)
        {
            return !(left == right);
        }

        #endregion
    }
}
