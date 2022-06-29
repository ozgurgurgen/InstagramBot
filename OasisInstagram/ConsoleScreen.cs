using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OasisInstagram
{
    public partial class ConsoleScreen : Form
    {
        public ConsoleScreen()
        {
            InitializeComponent();
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var dataList = new Data.Transfer().ToConsole();
            var output = "";

            if (dataList != null)
            {

                for (int i = consoleList.Items.Count; i <= dataList.Count; i++)
                {
                    if (consoleList.Items.Count < dataList.Count)
                    {
                        var satir = new ListViewItem(dataList[i]);
                        consoleList.Items.Add(satir);
                    }
                }
            }
            consoleList.Items[consoleList.Items.Count - 1].EnsureVisible();
        }

        private void ConsoleScreen_Load(object sender, EventArgs e)
        {
            consoleList.View = View.Details;
            consoleList.FullRowSelect = true;
            consoleList.Columns.Add("Output");
            consoleList.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
        }
    }
}
