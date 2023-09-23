using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HakuVoiceNarratorLibrary.Models
{
    /// <summary>
    /// 音声合成パラメーター管理モデル
    /// </summary>
    public class VoiceParameter
    {
        /// <summary>
        /// 話者ID
        /// </summary>
        public uint TalkerId { get; set; }

        /// <summary>
        /// 読み上げ文章(行単位の配列)
        /// </summary>
        public List<string> Texts { get; set; }

        /// <summary>
        /// 音量
        /// </summary>
        public float Volume { get; set; }

        /// <summary>
        /// 話速
        /// </summary>
        public float Speed { get; set; }

        /// <summary>
        /// 高さ
        /// </summary>
        public float Tone { get; set; }

        /// <summary>
        /// 抑揚
        /// </summary>
        public float Intonation { get; set; }

        /// <summary>
        /// 文中短ポーズ(80~500ms)
        /// </summary>
        public uint ShortPause { get; set; }

        /// <summary>
        /// 文中長ポーズ(100~200ms)
        /// </summary>
        public uint LongPause { get; set; }

        /// <summary>
        /// 文末ポーズ(200~10000ms)
        /// </summary>
        public uint TerminatePause { get; set; }

        /// <summary>
        /// かんたん!AI Talk使用フラグ
        /// </summary>
        public bool IsAiTalk { get; set; }

        /// <summary>
        /// かんたん!AI Talk キャラクターNO
        /// </summary>
        public int CharacterNo { get; set; }

    }
}
