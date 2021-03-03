using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkedVar;

namespace TuneVisualizer
{
    public class MusicSecond : NetworkedBehaviour, IMusicSecond
    {
        private NetworkedVarFloat second = new NetworkedVarFloat(new NetworkedVarSettings
        {
            ReadPermission = NetworkedVarPermission.Everyone,
            WritePermission = NetworkedVarPermission.Everyone
        }, 0);

        public float Second { get { return second.Value; } set { value = second.Value; } }

        public void ResetSecond()
        {
            second.Value = 0;
        }

        void ListenChanges()
        {
            second.OnValueChanged += valueChanged;
        }

        void valueChanged(float prevF, float newF)
        {
            Debug.Log("second went from " + prevF + " to " + newF);
        }
    }
}
