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
    public partial class CreateWorkerPanel : UserControl
    {
        public CreateWorkerPanel()
        {
            InitializeComponent();
            //מילוי הקומבובוקס מרשימת התפקידים (נטענה מה-DB)
            foreach (Title t in Program.Titles)
            {
                comboBox1.Items.Add(t);
            }
            comboBox1.SelectedIndex = 0;
        }

        private void Add_button_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ID_textBox.Text))
            {
                MessageBox.Show("יש להזין תעודת זהות", "שגיאה", MessageBoxButtons.OK);
                return;
            }
            if (string.IsNullOrWhiteSpace(Name_textBox.Text))
            {
                MessageBox.Show("יש להזין שם", "שגיאה", MessageBoxButtons.OK);
                return;
            }

            Title title = (Title)comboBox1.SelectedItem;
            Worker W = new Worker(ID_textBox.Text, Name_textBox.Text, title, true);
            mainForm.showPanel(new CRUDPanel());
        }

        private void button_back_Click(object sender, EventArgs e)
        {
            mainForm.showPanel(new CRUDPanel());
        }
    }
}
