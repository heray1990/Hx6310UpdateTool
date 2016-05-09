using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace Hx6310UpdateTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            progressValue = 0;
        }

        private static int progressValue;
        public static string fileName;

        #region constANT_VALUES
        public const byte VCP_BRIGHTNESS = 0x10;
        public const byte VCP_CONTRAST = 0x12;
        public const byte VCP_SELECT_COLOR_PRESET = 0x14;
        public const byte VCP_RGAIN = 0x16;
        public const byte VCP_GGAIN = 0x18;
        public const byte VCP_BGAIN = 0x1A;
        public const byte VCP_READ_HX6310_VER_L = 0x31;
        public const byte VCP_READ_NOV324_VER_L = 0x32;
        public const byte VCP_READ_SIL9777_VER_L = 0x33;
        public const byte VCP_READ_PS9115_VER_L = 0x34;
        public const byte VCP_READ_HX6310_VER_H = 0x35;
        public const byte VCP_READ_NOV324_VER_H = 0x36;
        public const byte VCP_READ_SIL9777_VER_H = 0x37;
        public const byte VCP_READ_PS9115_VER_H = 0x38;

        public const byte VCP_INPUT_SRC = 0x60;
        public const byte VCP_RBLACKLEVEL = 0x6C;
        public const byte VCP_GBLACKLEVEL = 0x6E;
        public const byte VCP_BBLACKLEVEL = 0x70;

        public const byte VCP_DEBUG = 0xFE;
        #endregion

        private DDCCIM6Reg ddcM2Reg = new DDCCIM6Reg();

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            int setDeviceSts = Lptio.LptioSetDevice(Lptio.DEVICE_FTDI);
            LogPrinter.ShowLog(textBoxLog, "LptioSetDevice return value: " + setDeviceSts);

            Lptio.I2cSetClockRate(50);
            ddcM2Reg.DDCSetVcp(VCP_DEBUG, 1);
            
            buttonUpdate.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string binPath = Path.GetDirectoryName(Application.ExecutablePath);
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = binPath;
            openFileDialog1.Filter = "bin files (*.bin)|*.bin|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
                fileName = textBox1.Text;
            }

        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(StartUpdate)).Start();
        }

        public void StartUpdate()
        {
            bool isDone = false;
            UpdateManager updateManager = new UpdateManager();
            updateManager.onUpdateProgress += new UpdateManager.dUpdateProgress(updateManager_onUpdateProgress);
            isDone = updateManager.Update();
            if (isDone)
                MessageBox.Show(UpdateManager.result);
        }

        private void updateManager_onUpdateProgress(int total, int current)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new UpdateManager.dUpdateProgress(updateManager_onUpdateProgress), new object[] { total, current });
            }
            else
            {
                this.progressBarUpdate.Maximum = total;
                this.progressBarUpdate.Value = current;
                int value = (int)(((double)current / (double)total) * 100);

                if (value != progressValue)
                {
                    progressValue = value;
                    LogPrinter.ShowLog(textBoxLog, progressValue.ToString() + "%");
                }                
            }
        }
    }
}
