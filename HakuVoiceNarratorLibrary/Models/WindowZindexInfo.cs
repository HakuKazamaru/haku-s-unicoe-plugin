using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HakuVoiceNarratorLibrary.Models
{
    /// <summary>
    /// Z-Index情報管理クラス
    /// </summary>
    public class WindowZindexInfo
    {
        /// <summary>
        /// テキスト
        /// </summary>
        public List<uint> TextBot {  get; set; }
        /// <summary>
        /// 再生ボタン
        /// </summary>
        public List<uint> PlayButton { get; set; }
        /// <summary>
        /// 保存ボタン
        /// </summary>
        public List<uint> SaveButton { get; set; }
        /// <summary>
        /// 音声効果タブ
        /// </summary>
        public List<uint> VoiceParameterTab { get; set; }
        /// <summary>
        /// 音量
        /// </summary>
        public List<uint> Volume { get; set; }
        /// <summary>
        /// 話速
        /// </summary>
        public List<uint> Speed { get; set; }
        /// <summary>
        /// 高さ
        /// </summary>
        public List<uint> Tone { get; set; }
        /// <summary>
        /// 抑揚
        /// </summary>
        public List<uint> Intonation { get; set; }
    }
}
