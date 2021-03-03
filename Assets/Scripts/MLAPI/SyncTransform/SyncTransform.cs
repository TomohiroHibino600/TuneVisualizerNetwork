using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

namespace TuneVisualizer
{
    public class SyncTransform : NetworkedBehaviour
    {
        private Transform parent;
        public string parentTag;
        public Vector3 positionOffset;

        public void Start()
        {
            if (!IsServer & IsOwner) {
                transform.parent = GameObject.FindGameObjectWithTag(parentTag).transform;
                transform.position = transform.parent.position + positionOffset;
            }
        }
    }
}
