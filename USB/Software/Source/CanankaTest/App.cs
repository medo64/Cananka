using System;
using System.Threading;
using System.Windows.Forms;
using Medo.Configuration;

namespace CanankaTest {
    static class App {
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Medo.Application.UnhandledCatch.ThreadException += new EventHandler<ThreadExceptionEventArgs>(UnhandledCatch_ThreadException);
            Medo.Application.UnhandledCatch.Attach();

            Medo.Windows.Forms.State.ReadState += delegate (object sender, Medo.Windows.Forms.StateReadEventArgs e) {
                e.Value = Config.Read("State!" + e.Name.Replace("CanankaTest.", ""), e.DefaultValue);
            };
            Medo.Windows.Forms.State.WriteState += delegate (object sender, Medo.Windows.Forms.StateWriteEventArgs e) {
                Config.Write("State!" + e.Name.Replace("CanankaTest.", ""), e.Value);
            };

            Application.Run(new MainForm());
        }

        private static void UnhandledCatch_ThreadException(object sender, ThreadExceptionEventArgs e) {
#if !DEBUG
            Medo.Diagnostics.ErrorReport.ShowDialog(null, e.Exception, new Uri("https://medo64.com/feedback/"));
#else
            throw e.Exception;
#endif
        }

    }
}
