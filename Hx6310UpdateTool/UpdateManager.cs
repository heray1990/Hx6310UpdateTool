using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Hx6310UpdateTool
{
    class UpdateManager
    {
        public static string result;
        public delegate void dUpdateProgress(int total, int current);
        public event dUpdateProgress onUpdateProgress;

        public bool Update()
        {
            int i = 0;
            int fragNum = 0, baseNum = 0, fileLength = 0;
            long sectorSize = 0;
            string fileName = Form1.fileName;
            byte crcCheck = 0, hwCrc = 0;

            if (!File.Exists(fileName))
            {
                LogPrinter.SaveLogInFile("Open " + fileName + " fail!!!");
                return false;
            }

            FileInfo fi = new FileInfo(fileName);
            fileLength = (int)fi.Length;
            byte[] buf = new byte[fileLength];

            FileStream fs = fi.OpenRead();
            fs.Read(buf, 0, Convert.ToInt32(fs.Length));
            fs.Close();

            for (i = 0; i < fileLength; i++)
            {
                crcCheck = Lptio.CRC8(buf[i], crcCheck);
            }
            Lptio.AndesPauseMcu();
            Lptio.AndesWriteSpiSr(0);
            Lptio.AndesBurstMode();

            fragNum = (fileLength - 1) / 256;
            sectorSize = 0x1000;

            LogPrinter.SaveLogInFile("Byte Length: " + fileLength.ToString());
            LogPrinter.SaveLogInFile("Frag number: " + fragNum.ToString());
            LogPrinter.SaveLogInFile("CRC Check: " + crcCheck.ToString());

            for (i = 0; i <= fragNum; i++)
            {
                double progressValue = i * 100 / fragNum;

                if (onUpdateProgress != null)
                    onUpdateProgress(100, (int)Math.Round(progressValue, 0));                

                if (((baseNum + i * 256) % sectorSize) == 0)
                {
                    Lptio.AndesWriteSpiData((baseNum + i * 256), ref buf[i * 256], 4);
                    Lptio.AndesWriteSpiData((baseNum + i * 256 + 4), ref buf[i * 256 + 4], 252);
                }
                else
                {
                    if (i == fragNum)
                    {
                        Lptio.AndesWriteSpiData((baseNum + fileLength - 1), ref buf[fileLength - 1], (int)(fileLength - 1 - (i - 1) * 256));
                    }
                    else
                    {
                        Lptio.AndesWriteSpiData((baseNum + i * 256), ref buf[i * 256], 256);
                    }
                }

                System.Threading.Thread.Sleep(100);
            }

            Lptio.AndesNormalMode();
            hwCrc = Lptio.AndesReadCrc(0, fileLength);
            Lptio.AndesWriteSpiSr(0x8C);
            Lptio.AndesStartMcu();
            LogPrinter.SaveLogInFile("Hardware CRC: " + hwCrc.ToString());

            if (hwCrc == crcCheck)
            {
                LogPrinter.SaveLogInFile("CRC PASS...");
                result = "CRC PASS...";                
            }
            else
            {
                LogPrinter.SaveLogInFile("CRC NG...");
                result = "CRC NG...";
            }
            return true;
        }
    }
}
