using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx6310UpdateTool
{
    class DDCCIM6Reg
    {
        #region CONSTANT_VALUES
        public const int I2C_BURST = 1;
        public const double DDCCI_TIMEOUT = (0.07);  // 70ms unit for 1 byte
        public const byte REG_DATA_CMD = 0x00;
        public const int FRAG_SIZE = 32;

        public const byte REG_WRITE = 0xF7;
        public const byte REG_WRITE_REPLY = 0xF9;
        public const byte REG_READ = 0xF2;
        public const byte REG_READ_REPLY = 0xF4;
        public const byte PLL_SET = 0xD7;
        public const byte PLL_SET_REPLY = 0xD9;

        public const int GET_VCP_FEATURE = 1;
        public const int VCP_FEATURE_REPLY = 2;
        public const int SET_VCP_FEATURE = 3;
        public const byte PLATFORMATTR_WRITE = 0x8D;
        public const byte PANELPARA_WRITE = 0x8F;
        public const byte PLATFORMATTR_WRITE_REPLY = 0x8E;
        public const byte PANELPARA_WRITE_REPLY = 0x90;
        public const byte PLATFORMATTR_READ = 0x91;
        public const byte PLATFORMATTR_READ_REPLY = 0x92;        

        public const byte TIMEOUT_VALUE = 0xFF;
        #endregion

        private byte CheckSum(ref byte[] buf, int length)
        {
            byte result = 0;
            int i = 0;

            for (i = 0; i <= (length - 1); i++)
            {
                result = (byte) (result ^ buf[i]);
            }

            return result;
        }

        private bool I2cLibWrite(int slaveAddrSft, ref byte[] cmd, int cmdLen, ref byte[] dat, int dataLen)
        {
            int i = 0;
            int ack = 0;

            if (I2C_BURST == 0)
            {                
                // Slave acknowledged if ack==0
                Lptio.I2c_Start();
                if ((slaveAddrSft / 2048) == 0x1E)  // 10 bit addr
                {                    
                    long addr = Common.DisBit((slaveAddrSft / 256), 0);
                    ack = Lptio.I2c_Tx(addr);
                    addr = slaveAddrSft & 0xFF;
                    if (ack == 1)
                    {
                        ack = Lptio.I2c_Tx(addr);
                    }
                }
                else  // 7 bit addr
                {
                    long addr = Common.DisBit(slaveAddrSft, 0);
                    ack = Lptio.I2c_Tx(addr);
                }

                for (i = 0; i <= (cmdLen - 1); i++)
                {
                    if (ack == 1)
                        break;

                    ack = Lptio.I2c_Tx(cmd[i]);
                }

                for (i = 0; i <= (dataLen - 1); i++)
                {
                    if (ack == 1)
                        break;

                    ack = Lptio.I2c_Tx(dat[i]);
                }

                Lptio.I2c_Stop();

                if (ack == 0)
                    return true;
                else
                    return false;
            }
            else
            {
                if ((cmdLen == 0) && (dataLen == 0))
                {
                    Lptio.I2c_Start();
                    ack = Lptio.I2c_Tx(slaveAddrSft);
                    Lptio.I2c_Stop();

                    if (ack == 0)
                        return true;
                    else
                        return false;
                }

                if (Lptio.I2cWriteData((byte)slaveAddrSft, ref cmd[0], cmdLen, ref dat[0], dataLen) == 1)
                    return true;
                else
                    return false;
            }
        }

        public void DDCSetVcp(byte vcp, int data)
        {
            byte[] buf = new byte[7];
            byte[] dummy = new byte[1];

            buf[0] = 0x51;
            buf[1] = 0x84;
            buf[2] = SET_VCP_FEATURE;
            buf[3] = (byte)vcp;
            buf[4] = (byte)(data / 256);
            buf[5] = (byte)(data % 256);
            buf[6] = (byte)(CheckSum(ref buf, 6) ^ 0x6E);

            if (!I2cLibWrite(0x6E, ref dummy, 0, ref buf, buf.Length))
            {
                LogPrinter.SaveLogInFile("[SetDebugMode] I2cLibWrite Error");
            }
        }
    }
}
