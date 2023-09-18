using HakuVoiceNarratorLibrary;
using NAudio.Wave;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Yomiage.SDK;
using Yomiage.SDK.Config;
using Yomiage.SDK.Talk;
using Yomiage.SDK.VoiceEffects;

namespace VoiceroidTalkEngine
{
    public class VoiceEngine : VoiceEngineBase
    {
        /// <summary>
        /// waveファイルの保存先管理
        /// </summary>
        private string wavePath => Path.Combine(DllDirectory, "output.wav");

        /// <summary>
        /// 音声合成メソッド
        /// </summary>
        /// <param name="mainVoice"></param>
        /// <param name="subVoice"></param>
        /// <param name="talkScript"></param>
        /// <param name="masterEffect"></param>
        /// <param name="setSamplingRate_Hz"></param>
        /// <param name="submitWavePart"></param>
        /// <returns></returns>

        public override async Task<double[]> Play(VoiceConfig mainVoice, VoiceConfig subVoice, TalkScript talkScript, MasterEffectValue masterEffect, Action<int> setSamplingRate_Hz, Action<double[]> submitWavePart)
        {
            double[] returnData;

            (int returnVal, int sampleRate, double[] wave) = await Voiceroid.TakeVoiceroid(
                wavePath,
                talkScript.OriginalText,
                (float)talkScript.Volume,
                (float)talkScript.Speed,
                (float)talkScript.Pitch,
                (float)1.0,
                talkScript.Sections.First().Pause.Span_ms);

            if (returnVal == 0)
            {
                setSamplingRate_Hz(sampleRate);
                returnData = wave;
            }
            else
            {
                returnData = new double[0];
            }

            return returnData;
        }
    }
}
