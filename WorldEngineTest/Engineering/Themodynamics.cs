using WorldEngine.Engineering;

namespace WorldEngineTest.Engineering
{
    public class ThermoTest
    {
        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// A processor wishes to mix 1 ton/hr of dry ground corn with 2 tons/hr of well water
        /// in a wellmixed, adiabatic tank to produce a mash. The corn enters at 50°F, and the
        /// well water enters at 40°F. The desired mash exit temperature is 90°F. Assume the
        /// specific heat of corn is 0.2 Btu/lbm-°F. The rate (Btu/hr) of energy addition required
        /// to heat the mash is most nearly:
        /// </summary>
        [Test]
        public void PE_AG_BIO_3()
        {
            AdiabaticSystem corn = new(1 * 2000, 50, 0.2);
            AdiabaticSystem water = new(2 * 2000, 40, 1);
            var ans = corn.HeatNeededToMixAtTemperature(water, 90);

            Assert.That(System.Math.Round(ans, 0), Is.EqualTo(216000));
        }
    }
}
