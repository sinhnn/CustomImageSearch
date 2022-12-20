using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string[] ret =      UseImageSearch     (@"C:\Users\sinhnn\source\repos\Libraries\ImageSearchDLL\ImageSearchDLL\WindowsFormsApp1\sample.png", "30");
            System.Diagnostics.Debug.WriteLine(ret);
            string[] retByBin = UseImageSearchByBin(@"C:\Users\sinhnn\source\repos\Libraries\ImageSearchDLL\ImageSearchDLL\WindowsFormsApp1\sample.png", "30");
            System.Diagnostics.Debug.WriteLine(retByBin);

            //Application.Run(new Form1());
        }

        [DllImport(@"C:\Users\sinhnn\source\repos\Libraries\ImageSearchDLL\ImageSearchDLL\Release\ImageSearchDLL.dll")]
        public static extern IntPtr ImageSearch(int x, int y, int right, int bottom, [MarshalAs(UnmanagedType.LPStr)] string imagePath);

        //[DllImport(@"C:\Users\sinhnn\source\repos\Libraries\ImageSearchDLL\ImageSearchDLL\Release\ImageSearchDLL.dll")]
        //public static extern IntPtr ImageSearchByBin(int aLeft, int aTop, int aRight, int aBottom ,int nWidth, int nHeight, uint nPlanes, uint nBitCounts, byte[] data);

        [DllImport(@"C:\Users\sinhnn\source\repos\Libraries\ImageSearchDLL\ImageSearchDLL\Release\ImageSearchDLL.dll")]
        public static extern IntPtr ImageSearchByBin(int aLeft, int aTop, int aRight, int aBottom, int nWidth, int nHeight, int aVariation, uint nBitCounts, IntPtr hbitdata);


        public static string[] UseImageSearchByBin(string imgPath, string tolerance)
        {
            int right = Screen.PrimaryScreen.WorkingArea.Right;
            int bottom = Screen.PrimaryScreen.WorkingArea.Bottom;
            //System.Diagnostics.Debug.WriteLine(imgPath);
            //imgPath = "*" + tolerance + " " + imgPath;
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(imgPath);
            //byte[] rgbValues = new byte[bitmap.Width * bitmap.Height * 4];

            System.Diagnostics.Debug.WriteLine($"UseImageSearchByBin {bitmap.PixelFormat}");
            IntPtr result = ImageSearchByBin(0, 0,
                Screen.PrimaryScreen.WorkingArea.Width,
                Screen.PrimaryScreen.WorkingArea.Height,
                bitmap.Width,
                bitmap.Height,
                4,
                32,
                bitmap.GetHbitmap());


            string res = Marshal.PtrToStringAnsi(result);
            System.Diagnostics.Debug.WriteLine("UseImageSearchByBin: " + res + " on " + right.ToString() + " " + bottom.ToString());

            if (res[0] == '0') return null;

            string[] data = res.Split('|');

            int x; int y;
            int.TryParse(data[1], out x);
            int.TryParse(data[2], out y);

            return data;
        }

        public static string[] UseImageSearch(string imgPath, string tolerance)
        {
            int right = Screen.PrimaryScreen.WorkingArea.Right;
            int bottom = Screen.PrimaryScreen.WorkingArea.Bottom;
            System.Diagnostics.Debug.WriteLine(imgPath);
            imgPath = "*" + tolerance + " " + imgPath;

            IntPtr result = ImageSearch(0, 0, right, bottom, imgPath);
            string res = Marshal.PtrToStringAnsi(result);
            System.Diagnostics.Debug.WriteLine(res + " on " + right.ToString() + " " + bottom.ToString());

            if (res[0] == '0') return null;

            string[] data = res.Split('|');

            int x; int y;
            int.TryParse(data[1], out x);
            int.TryParse(data[2], out y);

            return data;
        }
    }



}
