using Ong.Friendly.FormsStandardControls.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HakuVoiceNarratorLibrary.Models
{
    /// <summary>
    /// VOICEROID情報管理モデル
    /// </summary>
    public class VoiceroidInfo
    {
        /// <summary>
        /// かんたん！AI Takeキャラクター情報構造体
        /// </summary>
        public struct AiTalkCharacterInfo
        {
            /// <summary>
            /// キャラクター番号
            /// </summary>
            public int No { get; set; }
            /// <summary>
            /// キャラクター名
            /// </summary>
            public string Name { get; set; }
        }

        /// <summary>
        /// 話者ID
        /// </summary>
        public uint TalkerId { get; set; }

        /// <summary>
        /// 話者名
        /// </summary>
        public string TalkerName { get; set; }

        /// <summary>
        /// プロセス名(拡張子なし)
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// インストール先
        /// </summary>
        public string InstallPath { get; set; }

        /// <summary>
        /// かんたん！AI Takeフラグ
        /// </summary>
        public bool IsAiTalk { get; set; }

        /// <summary>
        /// かんたん！AI Take一覧
        /// </summary>
        public List<AiTalkCharacterInfo> CharacterList { get; set; }
    }
}
