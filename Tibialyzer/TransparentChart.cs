
// Copyright 2016 Mark Raasveldt
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace Tibialyzer {
    class TransparentChart : Chart {
        protected override void OnClick(EventArgs e) {
            base.OnClick(e);

            Control p = Parent;
            while (p != null) {
                // draw the background of the parent as background onto this 
                if (p is NotificationForm) {
                    NotificationForm c = (NotificationForm)p;
                    c.Controls.Remove(this);
                    break;
                }
                if (p.Parent == p) break;
                p = p.Parent;
            }
            MainForm.mainForm.CloseNotification();
        }
    }
}
