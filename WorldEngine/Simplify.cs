using Expr = MathNet.Symbolics.SymbolicExpression;

namespace WorldEngine
{
    public class Simplify
    {
        public static string Execute(Expr expr1)
        {
            string strExp = "FullSimplify[" + expr1.ToString() + "]";

            var ans = WolframEngine.ExecuteToInputForm(strExp, 0);

            return ans;
        }

        public static Wolfram.NETLink.Expr GetWolframExpr(Expr expr1)
        {
            string strExp = "FullSimplify[" + expr1.ToString() + "]";

            var ans = WolframEngine.TranslateToWolframExpr(strExp);

            return ans;
        }
    }
}
