using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TuneVisualizer
{
    /// <summary>
    /// 目の前の棒グラフを音量に合わせて管理する
    /// </summary>
    public class CubeFreq : MonoBehaviour
    {
        /// <summary>
        /// GUI指定した8分割した周波数の音量のindex
        /// </summary>
        [SerializeField] int band;

        /// <summary>
        /// GUI指定したこのCubeのサイズ調整値
        /// </summary>
        [SerializeField] float startScale, scaleMultiplier;

        /// <summary>
        /// 強調値を用いるか否か
        /// </summary>
        [SerializeField] bool useBuffer = true;

        /// <summary>
        /// GUI指定したこのCubeの色の初期値
        /// </summary>
        [SerializeField] Color myColor;

        private Material mat;

        // Use this for initialization
        void Start()
        {
            mat = GetComponent<MeshRenderer>().materials[0];
        }

        // Update is called once per frame
        void Update()
        {
            if (useBuffer == true)
            {
                //このPrefabの大きさを音量に合わせて調整
                transform.localScale = new Vector3(transform.localScale.x, (AudioViz.BandBuff[band] * scaleMultiplier) + startScale, transform.localScale.z);

                //このPrefabの色を音量に合わせて調整
                Color color = new Color(myColor.r * AudioViz.AudioBandBuffer[band], myColor.g * AudioViz.AudioBandBuffer[band], myColor.b * AudioViz.AudioBandBuffer[band]);
                mat.SetColor("_EmissionColor", color);
            }
            else if (useBuffer != true)
            {
                transform.localScale = new Vector3(transform.localScale.x, (AudioViz.FreqBand[band] * scaleMultiplier) + startScale, transform.localScale.z);
                Color color = new Color(myColor.r * AudioViz.AudioBand[band], myColor.g * AudioViz.AudioBand[band], myColor.b * AudioViz.AudioBand[band]);
                mat.SetColor("_EmissionColor", color);
            }
        }
    }
}