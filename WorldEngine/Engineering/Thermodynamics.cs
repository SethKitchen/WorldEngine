using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldEngine.Engineering
{
    /// <summary>
    /// No heat is lost to surroundings. All added heat is used to raise enthalpy of each component
    /// </summary>
    /// <param name="massFlowRateMDot">lbm/hr</param>
    /// <param name="temperatureT">F</param>
    /// <param name="specificHeatC">Btu/lbm-F</param>
    public class AdiabaticSystem(double massFlowRateMDot, double temperatureT, double specificHeatC)
    {
        public readonly double massFlowRateMDot = massFlowRateMDot;
        public readonly double temperatureT = temperatureT;
        public readonly double specificHeatC = specificHeatC;

        public double HeatNeededToMixAtTemperature(AdiabaticSystem two, double desiredMixtureTemperature)
        {
            return this.massFlowRateMDot * this.specificHeatC * (desiredMixtureTemperature - this.temperatureT) + two.massFlowRateMDot * two.specificHeatC * (desiredMixtureTemperature - two.temperatureT);
        }
    }
}
