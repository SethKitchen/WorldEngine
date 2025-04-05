using MathNet.Spatial.Euclidean;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolfram.NETLink;
using WorldEngine.Engineering;

namespace WorldEngineTest.Engineering
{
    public class PumpsTest
    {
        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// A pump draws from an ambient reservoir to provide 200 L/min of water at room temperature and
        /// 10 m of head. The pump efficiency is 55%, and the motor efficiency is 80%. If the pump runs
        /// continuously each day all year long and electricity costs $0.15/kWh, the annual electricity
        /// cost for the pump is most nearly:
        /// </summary>
        [Test]
        public void PE_AG_BIO_1()
        {
            Pump p = new(10, 200.0 / 60);
            var wdot = p.FluidPowerWdot();
            var powerConsideringEfficiency = wdot / .55 / .8;
            var time = 24 * 365;
            var totalWork = powerConsideringEfficiency * time / 1000; // kWh/ yr

            var cost = totalWork * 0.15;

            Assert.Multiple(() =>
            {
                Assert.That(System.Math.Round(powerConsideringEfficiency, 1), Is.EqualTo(740.7));
                Assert.That(time, Is.EqualTo(8760));
                Assert.That(System.Math.Round(totalWork, 1), Is.EqualTo(6488.8));
                Assert.That(System.Math.Round(cost, 0), Is.EqualTo(973));
            });
        }

        /// <summary>
        /// Two identical pumps are installed in parallel. Each pump discharges 20 gpm at 150 ft of head.
        /// The discharge head (ft) from the two pumps is most nearly:
        /// </summary>
        [Test]
        public void PE_AG_BIO_2()
        {
            Pump p = new(150.0 / 3.281, 20 / 15.85);

            Pump ans = p.AddInParallel(p);

            Assert.That(ans.head * 3.281, Is.EqualTo(150.0));
        }
    }
}
