using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tibialyzer {
    public partial class TabBase : Form, TabInterface {
        public TabBase() {
            InitializeComponent();
            InitializeSettings();
            InitializeTooltips();
        }
        
        public void InitializeSettings() {

        }

        public void InitializeTooltips() {

        }
    }
}
