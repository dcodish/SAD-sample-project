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
    /// <summary>
    /// הטופס הראשי — חלון יחיד שמחליף תוכן בתוכו.
    /// כל המסכים במערכת הם UserControl שנטענים לתוך panelMain.
    /// </summary>
    public partial class mainForm : Form
    {
        //הפניה סטטית לטופס הראשי — כדי שכל מסך יוכל לנווט
        private static mainForm instance;

        public mainForm()
        {
            InitializeComponent();
            instance = this;
            //הצגת מסך הכניסה בהפעלה
            showPanel(new LoginPanel());
        }

        /// <summary>
        /// החלפת המסך הנוכחי במסך חדש.
        /// זו הדרך היחידה לנווט בין מסכים במערכת.
        ///
        /// שימוש: mainForm.showPanel(new MyPanel());
        /// </summary>
        public static void showPanel(UserControl panel)
        {
            //ניקוי המסך הקודם
            instance.panelMain.Controls.Clear();
            //הוספת המסך החדש
            panel.Dock = DockStyle.Fill;
            instance.panelMain.Controls.Add(panel);
        }
    }
}
