using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TuneVisualizer
{
    /// <summary>
    /// 音楽の周波数ごとの音量と8分割したときの音量を可視化し、補正する
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class AudioViz : MonoBehaviour
    {
        /// <summary>
        /// 音楽のAudioSource
        /// </summary>
        [SerializeField] AudioSource AS;

        /// <summary>
        /// FFTの結果
        /// </summary>
        public static float[] Samples = new float[512];

        /// <summary>
        /// FFTの結果を8分割したときの各区域の平均音量
        /// </summary>
        public static float[] FreqBand = new float[8];

        /// <summary>
        /// FFTの結果を8分割したときの各区域の平均音量について、増減を強調した音量
        /// </summary>
        public static float[] BandBuff = new float[8];

        /// <summary>
        /// FFTの結果を8分割したときの各区域の平均音量について、増減の強調の調整値
        /// </summary>
        float[] bufferDecrease = new float[8];

        /// <summary>
        /// FFTの結果を8分割したとき、全ての区域の平均音量のうちの最大値
        /// </summary>
        float[] freqBandHighest = new float[8];

        /// <summary>
        /// 各区分の音量を最大音量で相対化した出力音量値
        /// </summary>
        public static float[] AudioBand = new float[8];

        /// <summary>
        /// 各区分の音量を最大音量で相対化した出力音量値に補正を加えたもの
        /// </summary>
        public static float[] AudioBandBuffer = new float[8];

        void Start()
        {
            //AS = GetComponent<AudioSource>();
        }

        void Update()
        {
            GetAudioSpec();
            MakeFreqBands();
            MakeBandBuff();
            MakeAudioBands();
        }

        /// <summary>
        /// 音声データをFFTし、その結果をSamplesに代入
        /// </summary>
        void GetAudioSpec()
        {
            AS.GetSpectrumData(Samples, 0, FFTWindow.Blackman);
        }

        /// <summary>
        /// Samplesを8分割し、各区分の平均音量を示す配列FreqBandを計算
        /// </summary>
        void MakeFreqBands()
        {
            int count = 0;

            for (int i = 0; i < 8; i++)
            {
                //平均音量を毎区分ごとに初期化
                float Average = 0;

                //1区分を求めるための計算
                int SampleCounter = (int)Mathf.Pow(2, i + 1);
                if (i == 7)
                {
                    SampleCounter = +2;
                }

                /**①countの推移
                    i=0のとき      count → 0~1
                    i=1~6のとき    count → count[i-1] ~ count[i-1] + 2のi+1乗(=各iのSampleCounterの量)
                    i=7のとき      count → 254, 255
                    ※iごとのcountの各区分を数字で表すと、0~1, 2~5, 6~13, 14~29, 30~61, 62~125, 126~253, 254~255

                音楽の音域が周波数の低い音域に偏っているため、低い音域の音量を重点的に計測している

                /**②各区分の平均音量Average = k-最小周波数~最大周波数Σ{Samples[k]*(k+1)} / 最大周波数
                 * 　※MakeAudioBands()で全区分中の最大音量で割ることで、値が相対化され、棒グラフにしたときに高低差が少なくなる
                 */
                for (int j = 0; j < SampleCounter; j++)
                {
                    Average += Samples[count] * (count + 1);
                    count++;
                }
                Average /= count;

                FreqBand[i] = Average * 10;
            }
        }

        /// <summary>
        /// 音量の増減が実際より強調されるように補正し、補正後の音量を配列BandBuffに代入
        /// </summary>
        void MakeBandBuff()
        {
            //全区分の補正音量配列BandBuffに関して
            for (int i = 0; i < 8; i++)
            {
                //現フレームの音量Freqband[i]が、1フレーム前に示した補正音量BandBaff[i]より大きい場合
                if (FreqBand[i] > BandBuff[i])
                {

                    //補正音量を現在の音量に合わせる
                    BandBuff[i] = FreqBand[i];

                    //音量差分値に0.005fを代入
                    bufferDecrease[i] = 0.005f;

                    //現フレームの音量Freqband[i]が、1フレーム前に示した補正音量BandBaff[i]より小さい場合
                }
                else if (BandBuff[i] > FreqBand[i])
                {

                    //補正音量から音量差分値を引く
                    BandBuff[i] -= bufferDecrease[i];

                    //小さくなっている間中ずっと、音量差分値を1.1倍し続けることで、補正音量の値が下がる割合を大きくしていく
                    bufferDecrease[i] *= 1.1f;
                }
            }
        }

        /// <summary>
        /// 全区分の中の最大音量で音量配列と補正音量配列を相対化し、出力配列audiobandとaudiobandBufferを求める
        /// </summary>
        void MakeAudioBands()
        {
            for (int i = 0; i < 8; i++)
            {
                //全区分の中の最大音量を求める
                if (FreqBand[i] > freqBandHighest[i])
                {
                    freqBandHighest[i] = FreqBand[i];
                }

                //各区分の音量と補正音量について、全区分中の最大音量で相対化
                AudioBand[i] = (FreqBand[i] / freqBandHighest[i]);
                AudioBandBuffer[i] = (BandBuff[i] / freqBandHighest[i]);
            }
        }
    }
}
