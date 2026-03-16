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
    public partial class UpdateDeletePanel : UserControl
    {
        private Worker exist_Worker;
        public UpdateDeletePanel()
        {
            InitializeComponent();
            textBox_name.Enabled = false;
            comboBox_title.Enabled = false;
            button_delete.Hide();
            button_update.Hide();
        }

        private void button_search_Click(object sender, EventArgs e)
        {
            exist_Worker = Worker.seekWorker(textBox_id.Text);
            if (exist_Worker == null)
            {
                MessageBox.Show("עובד לא נמצא", "שגיאה", MessageBoxButtons.OK);
                return;
            }
            button_delete.Show();
            button_update.Show();
            textBox_name.Text = exist_Worker.getName();
            textBox_name.Enabled = true;
            comboBox_title.Enabled = true;
            //מילוי הקומבובוקס מרשימת התפקידים (נטענה מה-DB)
            comboBox_title.Items.Clear();
            foreach (Title t in Program.Titles)
            {
                comboBox_title.Items.Add(t);
            }
            comboBox_title.SelectedItem = exist_Worker.getTitle();
        }

        private void button_update_Click(object sender, EventArgs e)
        {
            exist_Worker.setName(textBox_name.Text);
            exist_Worker.setTitle((Title)comboBox_title.SelectedItem);
            exist_Worker.updateWorker();
            mainForm.showPanel(new CRUDPanel());
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            exist_Worker.deleteWorker();
            mainForm.showPanel(new CRUDPanel());
        }

        private void button_back_Click(object sender, EventArgs e)
        {
            mainForm.showPanel(new CRUDPanel());
        }
    }
}
