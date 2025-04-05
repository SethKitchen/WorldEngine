using Expr = MathNet.Symbolics.SymbolicExpression;
using WorldEngine.Finance;
using MathNet.Symbolics;


namespace WorldEngineTest.Finance
{
    public class Interest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void BalanceAfterYearsWithNumbers()
        {
            Interest<double> A = new() { principal = 180_000, yearlyInterestRate =0.07  };

            Assert.That(System.Math.Round(A.BalanceAfterYears(10), 3), Is.EqualTo(362475.487));
        }

        [Test]
        public void BalanceAfterYearsWithExpr()
        {
            Interest<double> A = new() { principal = 180_000, yearlyInterestRate = 0.07 };

            Assert.That(System.Math.Round(A.BalanceAfterYears(10), 3), Is.EqualTo(362475.487));
        }

        /// <summary>
        /// As owner and operating manager of a large fleet of farm equipment,
        /// you are contracting with an outside mechanic's shop to have all full
        /// engine overhauls for tractors, combines, and harvesters completed at
        /// the rate of $7,200 per engine. To lower costs, you are considering
        /// overhauling the engines yourself and have determined the investment
        /// needed to construct a new building and equip it for the overhauls
        /// would be $180,000. Assume the operating cost to perform engine overhauls
        /// at your facility is $5,500 per engine and the interest rate is 7% per
        /// year. The minimum number of engines you need to overhaul per year to
        /// make the investment in equipment and facilities pay back within 10 years
        /// is most nearly:
        /// </summary>
        [Test]
        public void PE_AG_BIO_6()
        {
            Interest<Expr> inHouseUpfront = new() { principal = 180_000, yearlyInterestRate = 0.07 };
            Expr inHouse = inHouseUpfront.AnnualBasisExpr(10) + "5500*X";
            Expr external = "7200*X";

            double offBy = double.MaxValue;
            double answer = 0;
            for (int engineGuess = 4; engineGuess < 25; engineGuess++)
            {
                var variables = new Dictionary<string, FloatingPoint>
                {
                    ["X"] = engineGuess
                };

                var result = Evaluate.Evaluate(variables, (inHouse - external).Expression).RealValue;
                if(result < 0)
                {
                    result *= -1;
                    if (result < offBy)
                    {
                        offBy = result;
                        answer = engineGuess;
                    }
                }
            }

            Assert.That(answer, Is.EqualTo(16));
        }
    }
}
