using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldEngine.Engineering
{
    public class Pump
    {
        public readonly double densityRho = .997; // kg / L;
        public readonly double gravitationalAccelerationG = 9.807; // m/s^2
        public readonly double head; // m
        public readonly double flowQ; // L / s

        /// <summary>
        /// Initialize with water density and Earth graviational constant
        /// </summary>
        /// <param name="head">>The height or energy level that a pump can raise a fluid (m)</param>
        /// <param name="flowQ">Flow rate in L/s</param>
        public Pump(double head, double flowQ)
        {
            this.head = head;
            this.flowQ = flowQ;
        }

        public Pump(double densityRho, double gravitationalAccelerationG, double head, double flowQ)
        {
            this.densityRho = densityRho;
            this.gravitationalAccelerationG = gravitationalAccelerationG;
            this.head = head;
            this.flowQ = flowQ;
        }

        public double FluidPowerWdot()
        {
            return densityRho * gravitationalAccelerationG * head * flowQ;
        }

        public Pump AddInSeries(Pump two)
        {
            if(this.densityRho != two.densityRho || this.gravitationalAccelerationG !=two.gravitationalAccelerationG)
            {
                throw new Exception("Must have same density and gravity constraints");
            }
            double newHead = this.head + two.head;
            double newFlow = this.flowQ;
            return new Pump(this.densityRho, this.gravitationalAccelerationG, newHead, newFlow);
        }

        public Pump AddInParallel(Pump two)
        {
            if (this.densityRho != two.densityRho || this.gravitationalAccelerationG != two.gravitationalAccelerationG)
            {
                throw new Exception("Must have same density and gravity constraints");
            }
            double newHead = this.head;
            double newFlow = this.flowQ + two.flowQ;
            return new Pump(this.densityRho, this.gravitationalAccelerationG, newHead, newFlow);
        }
    }
}
