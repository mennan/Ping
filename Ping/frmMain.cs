using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;

namespace Ping
{
    public partial class frmMain : Form
    {
        public List<Printer> printers = new List<Printer>();
        private int i = 0;
        private delegate void Delege();
        private Action method = null;

        public frmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmAdd add = new frmAdd(printers, lbYazicilar);
            add.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;

            Timer t = new Timer() { Interval = 1000 };
            t.Start();

            t.Tick += delegate(object _s, EventArgs _e)
            {
                Delege d = new Delege(PingIt);
                d.BeginInvoke(null, null);
            };
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }

        private void PingIt()
        {
            System.Net.NetworkInformation.Ping p = new System.Net.NetworkInformation.Ping();
            Printer printer = printers[i];
            PingReply pr = p.Send(printer.IPAddress);

            if (pr.Status != IPStatus.Success)
                AddItemToListBox(string.Format("{0} -> {1} -> {2}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"), printer.Name, printer.IPAddress));

            if ((i + 1) == printers.Count)
                i = 0;
            else
                i++;
        }

        private void AddItemToListBox(string text)
        {
            if (lbHatalar.InvokeRequired)
            {
                method = delegate
                {
                    lbHatalar.Items.Add(text);
                };

                lbHatalar.Invoke(method);
            }

            notifyIcon1.BalloonTipText = text;
            notifyIcon1.BalloonTipTitle = "Yazıcıya ulaşılamadı.";
            notifyIcon1.ShowBalloonTip(1000);
        }

        public void RefreshPrinters()
        {
            foreach (var item in printers)
            {
                lbYazicilar.Items.Add(String.Format("{0}\t{1}", item.Name, item.IPAddress));
            }
        }
    }
}
