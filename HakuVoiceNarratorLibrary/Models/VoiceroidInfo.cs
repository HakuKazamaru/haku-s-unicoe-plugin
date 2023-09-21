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

    }
}
