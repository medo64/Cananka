using System;
using System.Windows.Forms;

namespace CanStick {
    internal class ListViewEx : ListView {

        public ListViewEx() : base() {
            this.DoubleBuffered = true;
        }

    }
}
