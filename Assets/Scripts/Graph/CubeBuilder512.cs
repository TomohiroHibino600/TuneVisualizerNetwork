using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TuneVisualizer
{
    /// <summary>
    /// 各周波数の音量を示すグラフを周囲に置く
    /// </summary>
    public class CubeBuilder512 : MonoBehaviour
    {

        /// <summary>
        /// 各周波数の音量を示すために用いるCubeのプレハブ
        /// </summary>
        [SerializeField] GameObject circleCube;

        /// <summary>
        /// 大きさを調整するための変数
        /// </summary>
        [SerializeField] float scaleParam = 100000;

        /// <summary>
        /// 各周波数の音量を示すために用いるCubeのインスタンス
        /// </summary>
        private GameObject circleCubeIns;

        /// <summary>
        /// 各周波数の音量を示すために用いるCubeの配列
        /// </summary>
        private GameObject[] circleCubes = new GameObject[512];

        /// <summary>
        /// 360度を512等分した角度。各CircleCubeをこの角度ごとに配置する
        /// </summary>
        private float circleRad;

        void Start()
        {
            circleRad = 360f / 512f;

            for (int i = 0; i < 512; i++)
            {
                //生成
                circleCubeIns = Instantiate(circleCube);

                //子要素として、CircleCubeをこのオブジェクトと同位置に配置
                circleCubeIns.transform.position = this.transform.position;
                circleCubeIns.transform.parent = this.transform;
                circleCubeIns.name = "CircleCube" + i;

                //円状に配置するために、半径一定で、親要素の角度を一定間隔ずつ変える
                this.transform.eulerAngles = new Vector3(0, -circleRad * i, 0);
                circleCubeIns.transform.position = Vector3.forward * 1000;

                //配列に収める
                circleCubes[i] = circleCubeIns;
            }
        }

        void Update()
        {
            if (circleCubes[511] == null)
            {
                return;
            }

            for (int i = 0; i < 512; i++)
            {
                //各周波数の音量に合わせて、cubeの高さを変える
                circleCubes[i].transform.localScale = new Vector3(10, (AudioViz.Samples[i] * scaleParam) + 2, 10);
            }
        }
    }
}
