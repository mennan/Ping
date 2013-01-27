using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ping
{
    public partial class frmAdd : Form
    {
        public List<Printer> printers { get; set; }
        public ListBox list { get; set; }

        public frmAdd(List<Printer> printers, ListBox list)
        {
            InitializeComponent();
            this.printers = printers;
            this.list = list;

            foreach (var item in printers)
            {
                lbPrinters.Items.Add(String.Format("{0}\t{1}", item.Name, item.IPAddress));
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string message = "";

            if (txtName.Text == "")
                message += "Yazıcı adını yazınız.\n";

            if (txtIpAddress.Text == "")
                message += "IP adresini yazınız.\n";

            if (message != "")
                MessageBox.Show(message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                var printer = printers.FirstOrDefault(x => x.Name == txtName.Text || x.IPAddress == txtIpAddress.Text);

                if (printer == null)
                {
                    printers.Add(new Printer
                    {
                        Name = txtName.Text,
                        IPAddress = txtIpAddress.Text
                    });

                    string printerText = String.Format("{0}\t{1}", txtName.Text, txtIpAddress.Text);

                    lbPrinters.Items.Add(printerText);
                    list.Items.Add(String.Format("{0} ({1})", txtName.Text, txtIpAddress.Text));
                }
                else
                    MessageBox.Show("Yazıcı daha önceden listeye eklenmiş.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtName.Text = "";
                txtIpAddress.Text = "";
            }
        }
    }
}
