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
            get { return LimitBetween(Config.Read("ScaleBoost", 0.00), -1.00, 4.00); }
            set { Config.Write("ScaleBoost", LimitBetween(value, -1.00, 4.00)); }
        }


        [Category("History")]
        [DisplayName("ID")]
        [Description("Last ID for the message.")]
        public int LastID {
            get { return LimitBetween(Config.Read("LastID", 0), 0x00000000, 0x1FFFFFFF); }
            set { Config.Write("LastID", value); }
        }

        [Category("History")]
        [DisplayName("Length")]
        [Description("Last length for the message.")]
        public int LastLength {
            get { return LimitBetween(Config.Read("LastLength", 0), 0, 8); }
            set { Config.Write("LastLength", value); }
        }

        [Category("History")]
        [DisplayName("Remote Request")]
        [Description("Last state of the remote request for the message.")]
        public bool LastRemoteRequest {
            get { return Config.Read("LastRemoteRequest", false); }
            set { Config.Write("LastRemoteRequest", value); }
        }

        [Category("History")]
        [DisplayName("Data")]
        [Description("Last data for the message.")]
        public string LastData {
            get { return Config.Read("LastData", "00"); }
            set { Config.Write("LastData", ""); }
        }


        #region Helper

        private static int LimitBetween(int value, int minValue, int maxValue) {
            if (value < minValue) { return minValue; }
            if (value > maxValue) { return maxValue; }
            return value;
        }

        private static double LimitBetween(double value, double minValue, double maxValue) {
            if (value < minValue) { return minValue; }
            if (value > maxValue) { return maxValue; }
            return value;
        }

        #endregion Helper

    }
}
