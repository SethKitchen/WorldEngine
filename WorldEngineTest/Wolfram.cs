namespace WorldEngineTest
{
    public class WolframTest
    {
        [Test]
        public void TwoPlusTwo()
        {
            var res = WorldEngine.WolframEngine.ExecuteToInputForm("2+2", 0);

            Assert.That(res.ToString(), Is.EqualTo("4"));
        }
    }
}