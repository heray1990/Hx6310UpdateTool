using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Hx6310UpdateTool
{
    class Lptio
    {
        // I2C Tool Device Constant
        public const int DEVICE_LPT = 0;
        public const int DEVICE_U2C = 1;
        public const int DEVICE_WT = 2;
        public const int DEVICE_UART = 3;
        public const int DEVICE_USBTCON = 4;
        public const int DEVICE_FTDI = 8;
        public const int DEVICE_AARDVARK = 16;

        // I2C Protocol Constant
        public const int M2REG_DEVICE_I2C = 0;
        public const int M2REG_DEVICE_DDCCI = 1;
        public const int M2REG_DEVICE_DDCCI_STANDBY = 2;
        public const int M2REG_DEVICE_I2C_INDIRECT = 3;
        public const int M2REG_DEVICE_I2C_DIRECT = 4;

        // ASIC Type Constant
        public const int ASIC_TYPE_MG4 = 0;  // Magic-4
        public const int ASIC_TYPE_MG5 = 1;  // Magic-5
        public const int ASIC_TYPE_M2I = 2;  // M2I
        public const int ASIC_TYPE_M2K = 3;  // M2K
        public const int ASIC_TYPE_NUM = 4;  // Total number of ASIC type

        // ASIC Sub Type Constant
        public const int ASIC_SUB_TYPE_FPGA_VIRTEX_4 = 0;
        public const int ASIC_SUB_TYPE_FPGA_VIRTEX_5 = 1;
        public const int ASIC_SUB_TYPE_SHUTTLE = 2;
        public const int ASIC_SUB_TYPE_ECO_1 = 3;
        public const int ASIC_SUB_TYPE_ECO_2 = 4;
        public const int ASIC_SUB_TYPE_ECO_3 = 5;
        public const int ASIC_SUB_TYPE_MP = 6;

        [DllImport("lptio.dll")]
        public static extern void I2c_Start();
        [DllImport("lptio.dll")]
        public static extern void I2c_RStart();
        [DllImport("lptio.dll")]
        public static extern void I2c_Stop();
        [DllImport("lptio.dll")]
        public static extern int I2c_Tx(long portaddr);
        [DllImport("lptio.dll")]
        public static extern int I2c_Rx(ref long portaddr, int ack);
        [DllImport("lptio.dll")]
        // sz: # hex bytes of str, str: read cmd string
        public static extern long I2c_ReadString(byte sz, string str);
        [DllImport("lptio.dll")]
        // sz: # hex bytes of str, str: write cmd string
        public static extern long I2c_WriteString(byte sz, string str);
        [DllImport("lptio.dll")]
        public static extern int Lpt_Output(long dat);
        [DllImport("lptio.dll")]
        public static extern byte LptioSetDevice(long device);
        [DllImport("lptio.dll")]
        public static extern long Pll_Clock(long f, long base_f, ref int m, ref int r, ref int n);
        [DllImport("lptio.dll")]
        public static extern long Pll_Nandu(long f, long base_f, ref int h, ref int m, ref int l);
        [DllImport("lptio.dll")]
        public static extern long Pll_Write(byte dev, byte addr, long m, long r, long n);
        [DllImport("lptio.dll")]
        public static extern byte uart_connect(byte port, long baudrate);
        [DllImport("lptio.dll")]
        public static extern void uart_disconnect();
        [DllImport("lptio.dll")]
        public static extern byte I2cWriteData(byte device, ref byte cmd, long cmdlen, ref byte wdat, long wdatlen);
        [DllImport("lptio.dll")]
        public static extern byte I2cReadData(byte device, ref byte cmd, long cmdlen, ref byte rdat, long rdatlen);
        [DllImport("lptio.dll")]
        public static extern void I2cSetClockRate(long rate);
        [DllImport("lptio.dll")]
        public static extern void SetPortVal(byte addr, byte val);
        [DllImport("lptio.dll")]
        public static extern int GetDeviceNumber();

        // Andes ISP
        [DllImport("lptio.dll")]
        public static extern void AndesPauseMcu();
        [DllImport("lptio.dll")]
        public static extern void AndesWriteSpiSr(byte val);
        [DllImport("lptio.dll")]
        public static extern void AndesBurstMode();
        [DllImport("lptio.dll")]
        public static extern void AndesWriteSpiData(long addr, ref byte buf, long leng);
        [DllImport("lptio.dll")]
        public static extern void AndesNormalMode();
        [DllImport("lptio.dll")]
        public static extern byte AndesReadCrc(long addr, long leng);
        [DllImport("lptio.dll")]
        public static extern void AndesStartMcu();
        [DllImport("lptio.dll")]
        public static extern long AndesIsp(ref byte buf, long leng);
        [DllImport("lptio.dll")]
        public static extern byte CRC8(byte dat, byte crc);
    }
}
