using WorldEngine;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace WorldEngineTest
{
    public class SimplifyTest
    {
        [Test]
        public void Five()
        {
            var x = Expr.Parse("1+1+1+1+1");
            var ans = Simplify.Execute(x);

            var waExpected = "5";
            Assert.That(ans.ToString(), Is.EqualTo(waExpected.ToString()));
        }

        [Test]
        public void Factorization()
        {
            var x = Expr.Parse("1/(x+x^(-1))*x^2");
            var ans = Simplify.Execute(x);

            var waExpected = Expr.Parse("x^3/(1+x^2)");
            Assert.That(ans.ToString(), Is.EqualTo(waExpected.ToString()));
        }
    }
}
