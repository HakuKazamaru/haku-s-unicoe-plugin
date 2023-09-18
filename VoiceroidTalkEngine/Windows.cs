using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UniCoeEnginePlugin
{
    /// <summary>
    /// ウィンドウメッセージ制御関連クラス
    /// </summary>
    public class Window
    {
        public string ClassName;
        public string Title;
        public IntPtr hWnd;
        public int Style;

        /// <summary>
        /// マウス左ボタン押下メッセージ
        /// </summary>
        public const int WM_LBUTTONDOWN = 0x201;
        /// <summary>
        /// マウス左ボタン離しメッセージ
        /// </summary>
        public const int WM_LBUTTONUP = 0x202;
        /// <summary>
        /// 
        /// </summary>
        public const int MK_LBUTTON = 0x0001;
        /// <summary>
        /// 
        /// </summary>
        public static int GWL_STYLE = -16;
        /// <summary>
        /// 
        /// </summary>
        public const int WM_COPYDATA = 0x004A;

        /// <summary>
        /// ウィンドウメッセージ送信
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="Msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, uint wParam, uint lParam);

        /// <summary>
        /// ウィンドウメッセージ送信(文字列)
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="Msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, ref COPYDATASTRUCT lParam);


        /// <summary>
        /// ウィンドウハンドル検索
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="hwndChildAfter"></param>
        /// <param name="lpszClass"></param>
        /// <param name="lpszWindow"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr hWnd, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        /// <summary>
        /// ウィンドウハンドル取得
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        [DllImport("user32")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        /// <summary>
        /// ウィンドウクラス取得
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lpClassName"></param>
        /// <param name="nMaxCount"></param>
        /// <returns></returns>

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
        /// <summary>
        /// ウィンドウテキストの長さ取得
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowTextLength(IntPtr hWnd);
        /// <summary>
        /// ウィンドウテキスト取得
        /// </summary>
        /// <param name="hWnd">対象ウィンドウハンドル</param>
        /// <param name="lpString"></param>
        /// <param name="nMaxCount"></param>
        /// <returns></returns>

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        /// <summary>
        /// SendMessage用構造体
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpData;
        }

        /// <summary>
        /// ウィンドウへ文字列送信
        /// </summary>
        /// <param name="targetWindowHandle"></param>
        /// <param name="str"></param>
        public static void SendString(IntPtr targetWindowHandle, string str)
        {
            COPYDATASTRUCT cds = new COPYDATASTRUCT();
            cds.dwData = IntPtr.Zero;
            cds.lpData = str;
            cds.cbData = str.Length * sizeof(char);
            //受信側ではlpDataの文字列を(cbData/2)の長さでstring.Substring()する

            IntPtr myWindowHandle = Process.GetCurrentProcess().MainWindowHandle;
            SendMessage(targetWindowHandle, WM_COPYDATA, myWindowHandle, ref cds);
        }


        /// <summary>
        /// 子ハンドルをすべて取得
        /// </summary>
        /// <param name="parent">親ウィンドウ情報</param>
        /// <param name="dest">取得対象ウィンドウ情報</param>
        /// <returns></returns>
        public static List<Window> GetAllChildWindows(Window parent, List<Window> dest)
        {
            dest.Add(parent);
            EnumChildWindows(parent.hWnd).ToList().ForEach(x => GetAllChildWindows(x, dest));
            return dest;
        }

        /// <summary>
        /// 親ハンドルから子ハンドルの取得
        /// </summary>
        /// <param name="hParentWindow">親ウィンドウハンドル</param>
        /// <returns></returns>
        public static IEnumerable<Window> EnumChildWindows(IntPtr hParentWindow)
        {
            IntPtr hWnd = IntPtr.Zero;
            while ((hWnd = FindWindowEx(hParentWindow, hWnd, null, null)) != IntPtr.Zero) { yield return GetWindow(hWnd); }
        }

        /// <summary>
        /// ウィンドウ情報の取得
        /// </summary>
        /// <param name="hWnd">対象ウィンドウハンドル</param>
        /// <returns></returns>
        public static Window GetWindow(IntPtr hWnd)
        {
            int textLen = GetWindowTextLength(hWnd);
            string windowText = null;
            if (0 < textLen)
            {
                //ウィンドウのタイトルを取得する
                StringBuilder windowTextBuffer = new StringBuilder(textLen + 1);
                GetWindowText(hWnd, windowTextBuffer, windowTextBuffer.Capacity);
                windowText = windowTextBuffer.ToString();
            }

            //ウィンドウのクラス名を取得する
            StringBuilder classNameBuffer = new StringBuilder(256);
            GetClassName(hWnd, classNameBuffer, classNameBuffer.Capacity);

            // スタイルを取得する
            int style = GetWindowLong(hWnd, GWL_STYLE);
            return new Window() { hWnd = hWnd, Title = windowText, ClassName = classNameBuffer.ToString(), Style = style };
        }

    }
}
