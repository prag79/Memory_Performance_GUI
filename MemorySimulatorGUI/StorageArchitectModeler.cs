using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using PCIeGUI;
using DIMMGUI;
using DIMMDemoGUI;
using PCIeDemoGUI;
namespace MemorySimulatorGUI
{
    public partial class StorageArchitectModeler : System.Windows.Forms.Form
    {
       
        public StorageArchitectModeler()
        {
            InitializeComponent();
        }

        
      
        private void pictureBox6_Click(object sender, EventArgs e)
        {
            this.Visible = false;

            if (SAMDemoBox.Checked)
            {
                Form pciDemoGuiForm = new PCIeModeler();
                switch (pciDemoGuiForm.ShowDialog())
                {
                    case DialogResult.OK:
                        break;
                    case DialogResult.Cancel:
                        this.Visible = true;
                        break;
                }

            }
            else
            {

                Form pciGuiForm = new PCIeControllerGUI();
                switch (pciGuiForm.ShowDialog())
                {
                    case DialogResult.OK:
                        break;
                    case DialogResult.Cancel:
                        this.Visible = true;
                        break;
                }


            }
      
            
        }

      
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            
       
                this.Visible = false;
                if (SAMDemoBox.Checked)
                {
                    Form dimmForm = new DIMMDemoGui();
                    switch (dimmForm.ShowDialog())
                    {
                        case DialogResult.OK:
                            break;
                        case DialogResult.Cancel:
                            this.Visible = true;
                            break;
                    }
                }
                else
                {

                    Form dimmForm = new TeraSController();
                    switch (dimmForm.ShowDialog())
                    {
                        case DialogResult.OK:
                            break;
                        case DialogResult.Cancel:
                            this.Visible = true;
                            break;
                    }


                }
            
          
        }

      
 
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.linkLabel1.LinkVisited = true;

            // Navigate to a URL.
            System.Diagnostics.Process.Start("http://www.crossbar-inc.com");
        }

    
    }
}
