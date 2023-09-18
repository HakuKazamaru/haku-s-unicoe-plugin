using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HakuVoiceNarratorLibrary
{
    /// <summary>
    /// ウィンドウメッセージ制御関連クラス
    /// </summary>
    public class Window
    {
        /// <summary>
        /// NLogロガー
        /// </summary>
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        #region ウィンドウコントロール情報
        /// <summary>
        /// コントロールのクラス名
        /// </summary>
        public string ClassName;
        /// <summary>
        /// コントロールのキャプション
        /// </summary>
        public string Title;
        /// <summary>
        /// コントロールのウィンドウハンドル
        /// </summary>
        public IntPtr hWnd;
        /// <summary>
        /// コントロールのスタイル
        /// </summary>
        public int Style;
        #endregion

        #region ウィンドウメッセージ定数
        /// <summary>
        /// マウス左ボタン押下メッセージ
        /// </summary>
        public const int WM_LBUTTONDOWN = 0x201;
        /// <summary>
        /// マウス左ボタン離しメッセージ
        /// </summary>
        public const int WM_LBUTTONUP = 0x202;
        /// <summary>
        /// マウス右ボタン
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
        /// 文字列送信メッセージ
        /// </summary>
        public const uint WM_SETTEXT = 0x000c;
        #endregion

        #region Win32Apiのデリゲート
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
        /// ウィンドウメッセージ送信(文字列)
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="Msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, string lParam);

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
        #endregion

        /// <summary>
        /// SendMessage用構造体
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct COPYDATASTRUCT
        {
            /// <summary>
            /// 送信データ(32ビット)
            /// </summary>
            public IntPtr dwData;
            /// <summary>
            /// 送信データポインターのバイト数
            /// </summary>
            public UInt32 cbData;
            /// <summary>
            /// 送信データのポインター
            /// </summary>
            public IntPtr lpData;
        }

        /// <summary>
        /// ウィンドウへ文字列送信
        /// </summary>
        /// <param name="targetWindowHandle"></param>
        /// <param name="str"></param>
        public static void SendString(IntPtr targetWindowHandle, string str)
        {
            logger.Trace("==============================  Start   ==============================");

            /*
            COPYDATASTRUCT cds = new COPYDATASTRUCT();
            Byte[] data = Encoding.Unicode.GetBytes(str);

            cds.dwData = IntPtr.Zero;
            cds.lpData = Marshal.AllocHGlobal(data.Length); ;
            cds.cbData = (uint)data.Length;
            Marshal.Copy(data, 0, cds.lpData, data.Length);

            IntPtr myWindowHandle = Process.GetCurrentProcess().MainWindowHandle;
            SendMessage(targetWindowHandle, WM_COPYDATA, myWindowHandle, ref cds);

            Marshal.FreeHGlobal(cds.lpData);
            */

            SendMessage(targetWindowHandle, WM_SETTEXT, 0, str);

            // ログ出力
            logger.Trace("送信対象ハンドルID　：{0}", targetWindowHandle);
            logger.Trace("ウィンドウメッセージ：{0}", WM_COPYDATA);
            logger.Trace("送信文字列　　　　　：{0}", str);

            logger.Trace("==============================   End    ==============================");
        }


        /// <summary>
        /// 子ハンドルをすべて取得
        /// </summary>
        /// <param name="parent">親ウィンドウ情報</param>
        /// <param name="dest">取得対象ウィンドウ情報</param>
        /// <returns></returns>
        public static List<Window> GetAllChildWindows(Window parent, List<Window> dest)
        {
            logger.Trace("==============================  Start   ==============================");

            dest.Add(parent);
            EnumChildWindows(parent.hWnd).ToList().ForEach(x => GetAllChildWindows(x, dest));

            // ログ出力
            logger.Trace("親ハンドルID：{0}", parent.hWnd);
            logger.Trace("子ハンドル数：{0}", dest.Count - 1);

            logger.Trace("==============================   End    ==============================");
            return dest;
        }

        /// <summary>
        /// 親ハンドルから子ハンドルの取得
        /// </summary>
        /// <param name="hParentWindow">親ウィンドウハンドル</param>
        /// <returns></returns>
        public static IEnumerable<Window> EnumChildWindows(IntPtr hParentWindow)
        {
            logger.Trace("==============================  Start   ==============================");
            IntPtr hWnd = IntPtr.Zero;
            while ((hWnd = FindWindowEx(hParentWindow, hWnd, null, null)) != IntPtr.Zero) { yield return GetWindow(hWnd); }
            logger.Trace("==============================   End    ==============================");
        }

        /// <summary>
        /// ウィンドウ情報の取得
        /// </summary>
        /// <param name="hWnd">対象ウィンドウハンドル</param>
        /// <returns></returns>
        public static Window GetWindow(IntPtr hWnd)
        {
            logger.Trace("==============================  Start   ==============================");
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

            // ログ出力
            logger.Trace("ハンドルID　　　　　　：{0}", hWnd);
            logger.Trace("ウィンドウキャプション：{0}", windowText);
            logger.Trace("ウィンドウクラス　　　：{0}", classNameBuffer.ToString());
            logger.Trace("ウィンドウスタイル　　：{0}", style);

            logger.Trace("==============================   End    ==============================");
            return new Window() { hWnd = hWnd, Title = windowText, ClassName = classNameBuffer.ToString(), Style = style };
        }

    }
}
