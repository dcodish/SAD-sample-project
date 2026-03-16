using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Example_Project
{
    public partial class MoadAPanel : UserControl
    {
        public MoadAPanel()
        {
            InitializeComponent();
        }

        private void back_Click(object sender, EventArgs e)
        {
            mainForm.showPanel(new LoginPanel());
        }
    }
}
