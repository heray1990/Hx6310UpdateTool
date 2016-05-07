using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace Hx6310UpdateTool
{
    class LogPrinter
    {
        public static void ShowLog(TextBox txtBoxLog, string log)
        {
            // Clear the textBoxLog if there are more than 100 lines in it.
            if (txtBoxLog.GetLineFromCharIndex(txtBoxLog.Text.Length) > 100)
                txtBoxLog.Text = "";

            txtBoxLog.AppendText("> " + log);
            txtBoxLog.AppendText(Environment.NewLine);
            txtBoxLog.ScrollToCaret();

            SaveLogInFile(log);
        }

        public static void SaveLogInFile(string log)
        {
            string logPath = Path.GetDirectoryName(Application.ExecutablePath);

            System.IO.Directory.CreateDirectory(logPath + @"\Logs");
            System.IO.StreamWriter sw = System.IO.File.AppendText(logPath
                + @"\Logs\" + DateTime.Now.ToString("yyyy-MM-dd") + @".log");
            sw.WriteLine(DateTime.Now.ToString(String.Format("[{0}]HH:mm:ss > ", Version())) + log);
            sw.Close();
            sw.Dispose();
        }

        public static string Version()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(asm.Location);
            return String.Format("{0}.{1}.{2}", fvi.ProductMajorPart, fvi.ProductMinorPart, fvi.ProductBuildPart);
        }
    }
}
