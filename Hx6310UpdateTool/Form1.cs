using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hx6310UpdateTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ddcM2Reg.DDCSetVcp(VCP_INPUT_SRC, 3);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ddcM2Reg.DDCSetVcp(VCP_INPUT_SRC, 2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ddcM2Reg.DDCSetVcp(VCP_INPUT_SRC, 1);
        }
    }
}
