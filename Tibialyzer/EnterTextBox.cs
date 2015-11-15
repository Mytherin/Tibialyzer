using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Tibialyzer {
    class EnterTextBox : TextBox {
        protected override bool IsInputKey(Keys keyData) {
            if (keyData == Keys.Return)
                return true;
            return base.IsInputKey(keyData);
        }
    }
}
