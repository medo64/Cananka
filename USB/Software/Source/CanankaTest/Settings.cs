using System;
using System.ComponentModel;
using Medo.Configuration;

namespace CanankaTest {
    internal class Settings {

        public static Settings Current = new Settings();


        [Category("Appearance")]
        [DisplayName("Scale Boost")]
        [Description("Amount of boost given to each icon in addition to DPI size increases.")]
        [DefaultValue(0.00)]
        public double ScaleBoost {
            get { return Config.Read("ScaleBoost", LimitBetween(0.00, -1.00, 4.00)); }
            set { Config.Write("ScaleBoost", LimitBetween(value, -1.00, 4.00)); }
        }



        #region Helper

        private static double LimitBetween(double value, double minValue, double maxValue) {
            if (value < minValue) { return minValue; }
            if (value > maxValue) { return maxValue; }
            return value;
        }

        #endregion Helper

    }
}
