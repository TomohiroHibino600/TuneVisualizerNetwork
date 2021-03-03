using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TuneVisualizer
{
    /// <summary>
    /// ストロボライトが属する音域に合わせて、光の強度を変えるためのクラス
    /// </summary>
    public class AudioStrobeLight : MonoBehaviour
    {
        /// <summary>
        /// ストロボライトが属する音域の指定番号
        /// </summary>
        [SerializeField] int band;

        /// <summary>
        /// ストロボライトの強度の最小値と最大値
        /// </summary>
        [SerializeField] float minimumlight, maximumLight;

        private Light strober;

        // Use this for initialization
        void Start()
        {
            strober = GetComponent<Light>();
        }

        // Update is called once per frame
        void Update()
        {
            //属する音域の音量に合わせ、光量を調節する
            strober.intensity = (AudioViz.AudioBandBuffer[band] * (maximumLight - minimumlight)) + minimumlight;
        }
    }
}
