using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TuneVisualizer
{
    public class CameraFirstTransform : MonoBehaviour
    {
        private void Awake()
        {
            transform.position = new Vector3(Random.Range(-7, 7), 546, -785 + Random.Range(-7, 7));
        }
    }
}
