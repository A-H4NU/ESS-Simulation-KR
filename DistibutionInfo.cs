using System;
using System.Collections.Generic;

namespace ESS_Simulation
{
    internal class DistibutionInfo<T>
    {
        private readonly Dictionary<T, double> _distribution;
        private double _sum;

        public DistibutionInfo() => this._distribution = new Dictionary<T, double>();

        public void Add(T @object, double value)
        {
            double p = 0.0;
            if (this._distribution.ContainsKey(@object))
            {
                p = this._distribution[@object];
                this._distribution.Remove(@object);
            }
            this._distribution.Add(@object, value + p);
            this._sum += value;
        }

        public double GetRate(T @object) => this._distribution[@object] / this._sum;

        public int GetDistributeOf(T @object, int total) => (int)Math.Round(this.GetRate(@object) * total);
    }
}
