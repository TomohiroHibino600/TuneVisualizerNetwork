using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TuneVisualizer
{
    /// <summary>
    /// 落下Prefabの大きさや色の変化を音量に合わせて管理する
    /// </summary>
    public class DroppedObjectMinipuklator : MonoBehaviour
    {

        /// <summary>
        /// GUI指定した8分割した周波数の音量のindex
        /// </summary>
        [SerializeField] int band;

        /// <summary>
        /// GUI指定したこのPrefabのサイズ調整値
        /// </summary>
        [SerializeField] float startScale, scaleMultiplier;

        /// <summary>
        /// 強調値を用いるか否か
        /// </summary>
        [SerializeField] bool useBuffer = true;

        /// <summary>
        /// GUI指定したこのPrefabの色の初期値
        /// </summary>
        [SerializeField] Color myColor;

        /// <summary>
        /// このPrefabの破壊を待つ秒数
        /// </summary>
        [SerializeField] float destroyWait;

        /// <summary>
        /// 消去用のカウント
        /// </summary>
        private int count = 0;

        private Material mat;

        void Start()
        {
            mat = GetComponent<MeshRenderer>().materials[0];

            //数秒後にDestroyThis()を呼び出し、一定頻度で繰り返す
            Invoke("DestroyThis", destroyWait);
        }

        // Update is called once per frame
        void Update()
        {
            if (useBuffer == true)
            {
                //このPrefabの大きさを音量に合わせて調整
                transform.localScale = new Vector3((AudioViz.BandBuff[band] * scaleMultiplier) + startScale, (AudioViz.BandBuff[band] * scaleMultiplier) + startScale, (AudioViz.BandBuff[band] * scaleMultiplier) + startScale);

                //このPrefabの色を音量に合わせて調整
                Color color = new Color(myColor.r * AudioViz.AudioBandBuffer[band], myColor.g * AudioViz.AudioBandBuffer[band], myColor.b * AudioViz.AudioBandBuffer[band]);
                mat.SetColor("_EmissionColor", color);
            }
            else if (useBuffer != true)
            {
                transform.localScale = new Vector3((AudioViz.FreqBand[band] * scaleMultiplier) + startScale, (AudioViz.FreqBand[band] * scaleMultiplier) + startScale, (AudioViz.FreqBand[band] * scaleMultiplier) + startScale);
                Color color = new Color(myColor.r * AudioViz.AudioBand[band], myColor.g * AudioViz.AudioBand[band], myColor.b * AudioViz.AudioBand[band]);
                mat.SetColor("_EmissionColor", color);
            }

            //もし目立たないほど小さかったら
            if (transform.localScale.x <= 1.0F)
            {
                count++;

                //150フレーム目立たなかったら、このPrefabを破壊
                if (count == 150)
                {
                    Destroy(this.gameObject);
                }
            }
            else
            {
                //目立つほど大きくなったら初期化
                count = 0;
            }
        }

        private void DestroyThis()
        {
            Destroy(gameObject);
        }
    }
}
