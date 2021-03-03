using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TuneVisualizer
{
    /// <summary>
    /// 落下Prefabを生成する
    /// </summary>
    public class CubeDropper : MonoBehaviour
    {
        /// <summary>
        /// 最大音量を出した音域の番号を保持
        /// </summary>
        private int topBand = 0;

        /// <summary>
        /// 落下物生成を待つ秒数
        /// </summary>
        [SerializeField] int instantWait = 5;

        /// <summary>
        /// 落下物生成の頻度
        /// </summary>
        [SerializeField] float instantRate = 0.5f;

        /// <summary>
        /// GUIで指定した落下物のPrefab
        /// </summary>
        [SerializeField] GameObject[] dropperObject = new GameObject[8];

        void Start()
        {
            //数秒後にObjectDrop()を呼び出し、一定頻度で繰り返す
            InvokeRepeating("ObjectDrop", instantWait, instantRate);
        }

        /// <summary>
        /// 落とすPrefabを生成する
        /// </summary>
        void ObjectDrop()
        {
            BestBandGetter();

            //最大音量を出した音域に合った落下Prefabを生成
            Instantiate(dropperObject[topBand], new Vector3(Random.Range(-5000f, 5000), Random.Range(100, 1000), Random.Range(0f, 10000f)), transform.rotation);
        }

        /// <summary>
        /// 8区分の中で最大音量を出した音域の番号を得る
        /// </summary>
        void BestBandGetter()
        {
            for (int i = 0; i < 8; i++)
            {
                if (i == 0)
                {
                    topBand = 0;
                }
                else if (AudioViz.AudioBand[topBand] < AudioViz.AudioBand[i])
                {
                    topBand = i;
                }
            }
        }
    }
}
