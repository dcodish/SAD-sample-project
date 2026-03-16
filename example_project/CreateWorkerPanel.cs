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
            foreach (Title t in Enum.GetValues(typeof(Title)))
            {
                comboBox1.Items.Add(TitleHelper.ToDisplayString(t));
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

            Title title = TitleHelper.FromDisplayString(comboBox1.Text);
            Worker W = new Worker(ID_textBox.Text, Name_textBox.Text, title, true);
            mainForm.showPanel(new CRUDPanel());
        }

        private void button_back_Click(object sender, EventArgs e)
        {
            mainForm.showPanel(new CRUDPanel());
        }
    }
}
