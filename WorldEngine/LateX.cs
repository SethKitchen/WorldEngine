namespace WorldEngine
{
    public class WolframLatex
    {
        public static string Execute(string expr1)
        {
            string strExp = "TeXForm[" + expr1 + "]";

            var ans = WolframEngine.ExecuteToInputForm(strExp, 0);

            return ans;
        }

        public static Wolfram.NETLink.Expr GetWolframExpr(string expr1)
        {
            string strExp = "TeXForm[" + expr1 + "]";

            var ans = WolframEngine.TranslateToWolframExpr(strExp);

            return ans;
        }
    }
}
