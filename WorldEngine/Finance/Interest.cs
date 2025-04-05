using System.Numerics;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace WorldEngine.Finance
{
    public class Interest<T>
    {
        public required T principal;
        public required T yearlyInterestRate;
    }

    public static class InterestExtensions
    {
        public static T BalanceAfterYears<T>(this Interest<T> interest, T years) where T : IFloatingPoint<T>, IExponentialFunctions<T>
        {
            T exponent = interest.yearlyInterestRate * years;
            T result = T.Exp(exponent);

            return interest.principal * result;
        }

        public static T BalanceAfterYearsExpr<T>(this Interest<T> interest, T years) where T : Expr
        {
            T exponent = (T)(interest.yearlyInterestRate * years);
            T result = (T)Expr.Parse("e^("+exponent.ToString()+")");

            return (T)(interest.principal * result);
        }

        public static T AnnualBasis<T>(this Interest<T> interest, T years) where T: IFloatingPoint<T>, IPowerFunctions<T>
        {
            T onePlusIToN = T.Pow(T.One + interest.yearlyInterestRate, years);
            return interest.principal * ((interest.yearlyInterestRate * onePlusIToN) / (onePlusIToN - T.One));
        }

        public static T AnnualBasisExpr<T>(this Interest<T> interest, T years) where T : Expr
        {
            T onePlusIToN = (T)Expr.Parse("(1+"+ interest.yearlyInterestRate.ToString()+")^("+ years.ToString()+")");
            return (T)(interest.principal * ((interest.yearlyInterestRate * onePlusIToN) / (onePlusIToN - Expr.One)));
        }
    }
}
