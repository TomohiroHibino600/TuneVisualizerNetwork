using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkedVar;

namespace TuneVisualizer
{
    public class MusicIndex : NetworkedBehaviour, IMusicIndex
    {
        private NetworkedVarInt index = new NetworkedVarInt(new NetworkedVarSettings
        {
            ReadPermission = NetworkedVarPermission.Everyone,
            WritePermission = NetworkedVarPermission.Everyone
        }, 0);

        public int GetValue() { return index.Value; }

        public void AddIndex(int max)
        {
            if (index.Value >= max - 1 )
            {
                return;
            }

            index.Value += 1;
        }

        public void SubtractIndex()
        {
            if (index.Value <= 0)
            {
                return;
            }

            index.Value -= 1;
        }

        void ListenChanges()
        {
            index.OnValueChanged += valueChanged;
        }

        void valueChanged(int prevF, int newF)
        {
            Debug.Log("index went from " + prevF + " to " + newF);
        }
    }
}
