using WorldEngine;
using MathNet.Spatial.Euclidean;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace WorldEngineTest
{
    public class Symbolic3DTests
    {
        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        ///  Ginsberg, Jerry.Engineering Dynamics(p. 11). Cambridge University Press. Kindle Edition.
        /// </summary>
        [Test]
        public void Test_Ex_1_2_1()
        {
            var r_t = Expr.Parse("2000 + 100 * t");
            var theta_t = Expr.Parse("3.14159265/2 * (1 - 2.71828^(-0.15 * t))");
            var beta_t = Expr.Parse("3.14159265/3 - 0.1*t^(0.5)");
            WorldEngine.Symbolic3D toSolve = WorldEngine.Symbolic3D.PositionVectorApprox(r_t, theta_t, beta_t);
            Vector3D ans = toSolve.GetFiniteCentralDifferenceApproximation(2, 1e-3);
            Assert.Multiple(() =>
            {
                Assert.That(Math.Round(ans.X, 1), Is.EqualTo(19.0));
                Assert.That(Math.Round(ans.Y, 1), Is.EqualTo(266.3));
                Assert.That(Math.Round(ans.Z, 1), Is.EqualTo(30.7));
            });
        }

        /// <summary>
        ///  Ginsberg, Jerry.Engineering Dynamics(p. 11). Cambridge University Press. Kindle Edition.
        /// </summary>
        [Test]
        public void Test_Ex_1_2_2()
        {
            var r_t = Expr.Parse("2000 + 100 * t");
            var theta_t = Expr.Parse("3.14159265/2 * (1 - 2.71828^(-0.15 * t))");
            var beta_t = Expr.Parse("3.14159265/3 - 0.1*t^(0.5)");
            WorldEngine.Symbolic3D toSolve = WorldEngine.Symbolic3D.PositionVector(r_t, theta_t, beta_t);
            Vector3D ans = toSolve.SolveAtTime(2);
            Assert.Multiple(() =>
            {
                Assert.That(Math.Round(ans.X, 1), Is.EqualTo(19.0));
                Assert.That(Math.Round(ans.Y, 1), Is.EqualTo(266.3));
                Assert.That(Math.Round(ans.Z, 1), Is.EqualTo(30.7));
            });
        }

        [Test]
        public void DotProduct()
        {
            var Ax = Expr.Parse("2");
            var Ay = Expr.Parse("3");
            var Az = Expr.Parse("4");

            var Bx = Expr.Parse("5");
            var By = Expr.Parse("6");
            var Bz = Expr.Parse("7");

            var A = new Symbolic3D(Ax, Ay, Az);
            var B = new Symbolic3D(Bx, By, Bz);

            var ans = A.DotProduct(B);
            Assert.That(ans.ToString(), Is.EqualTo(Expr.Parse("56").ToString()));
        }
    }
}