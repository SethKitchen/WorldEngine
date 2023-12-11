using Wolfram.NETLink;

namespace WorldEngine
{
    public class WolframEngine
    {

        public static Expr TranslateToWolframExpr(string expression)
        {
            string[] mlArgs = { "-linkmode", "launch", "-linkname", "C:\\Program Files\\Wolfram Research\\Wolfram Engine\\13.2\\wolfram.exe" };
            // This launches the Mathematica kernel:
            IKernelLink ml = MathLinkFactory.CreateKernelLink(mlArgs);

            // Discard the initial InputNamePacket the kernel will send when launched.
            ml.WaitAndDiscardAnswer();

            ml.Evaluate(expression);
            ml.WaitForAnswer();
            Expr? ans1 = ml.GetExpr();

            // Always Close link when done:
            ml.Close();

            return ans1;
        }

        public static string ExecuteToOutput(string expression, int pageWidth)
        {
            string[] mlArgs = { "-linkmode", "launch", "-linkname", "C:\\Program Files\\Wolfram Research\\Wolfram Engine\\13.2\\wolfram.exe" };
            // This launches the Mathematica kernel:
            IKernelLink ml = MathLinkFactory.CreateKernelLink(mlArgs);

            // Discard the initial InputNamePacket the kernel will send when launched.
            ml.WaitAndDiscardAnswer();

            var ans = ml.EvaluateToOutputForm(expression, pageWidth);

            // Always Close link when done:
            ml.Close();

            return ans;
        }

        public static string ExecuteToInputForm(string expression, int pageWidth)
        {
            string[] mlArgs = { "-linkmode", "launch", "-linkname", "C:\\Program Files\\Wolfram Research\\Wolfram Engine\\13.2\\wolfram.exe" };
            // This launches the Mathematica kernel:
            IKernelLink ml = MathLinkFactory.CreateKernelLink(mlArgs);

            // Discard the initial InputNamePacket the kernel will send when launched.
            ml.WaitAndDiscardAnswer();

            var ans = ml.EvaluateToInputForm(expression, pageWidth);

            // Always Close link when done:
            ml.Close();

            return ans;
        }

        public static string ExecuteStr(string expression)
        {
            string[] mlArgs = { "-linkmode", "launch", "-linkname", "C:\\Program Files\\Wolfram Research\\Wolfram Engine\\13.2\\wolfram.exe" };
            // This launches the Mathematica kernel:
            IKernelLink ml = MathLinkFactory.CreateKernelLink(mlArgs);

            // Discard the initial InputNamePacket the kernel will send when launched.
            ml.WaitAndDiscardAnswer();

            ml.Evaluate(expression);
            ml.WaitForAnswer();
            string ans1 = ml.GetString();

            // Always Close link when done:
            ml.Close();

            return ans1;
        }
    }
}