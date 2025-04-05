using WorldEngine;
using WorldEngine.Math;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace WorldEngineTest.Math
{
    public class EllipseTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void EccentricityWithNumbers()
        {
            Ellipse<double> A = new() { semiMajorAxisA = 10, semiMinorAxisB = 5 };

            Assert.That(System.Math.Round(A.EccentricityE(), 3), Is.EqualTo(0.866));
        }

        [Test]
        public void EccentricityWithExpr()
        {
            Ellipse<Expr> A = new() { semiMajorAxisA = 15, semiMinorAxisB = 5 };

            Assert.That(A.EccentricityEExpr().ToString(), Is.EqualTo(Expr.Parse("sqrt(8/9)").ToString()));
        }

        [Test]
        public void PerimeterWithNumbers()
        {
            Ellipse<double> A = new() { semiMajorAxisA = 10, semiMinorAxisB = 5 };

            Assert.That(System.Math.Round(A.PerimeterP(), 2), Is.EqualTo(48.44));
        }

        [Test]
        public void PerimeterWithExpr()
        {
            Ellipse<Expr> A = new() { semiMajorAxisA = 15, semiMinorAxisB = 5 };

            Assert.That(A.PerimeterPExpr().ToString(), Is.EqualTo(Expr.Parse("66.8245").ToString()));
        }
    }
}
