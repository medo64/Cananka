using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CanStick {
    static class App {
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
