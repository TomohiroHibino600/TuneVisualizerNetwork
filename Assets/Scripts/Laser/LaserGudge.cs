using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crosstales.Radio;

namespace TuneVisualizer
{
    public class LaserGudge : MonoBehaviour
    {
        [SerializeField] Transform rayOrigin = null;
        [SerializeField] LaserPointer laserPointer = null;
        [SerializeField] AudioPlayer audioPlayer = null;
        [SerializeField] float maxRayDistance = 100.0f;

        private void Start()
        {
            if (ReferenceEquals(audioPlayer, null))
            {
                audioPlayer = GameObject.FindGameObjectWithTag("AudioPlayer").GetComponent<AudioPlayer>();
            }
        }

        void Update()
        {
            // 右手のコントローラの位置と向いている方向からRayを作成
            Ray ray = new Ray(rayOrigin.position, laserPointer._forward);

            // 作成したRay上にColliderがあるか判定
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxRayDistance))
            {
                if (Input.GetKey(KeyCode.Space) | OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
                {
                    if (hit.collider.gameObject.tag == "Play")
                    {
                        audioPlayer.Play();
                    }

                    if (hit.collider.gameObject.tag == "Stop")
                    {
                        audioPlayer.Stop();
                    }

                    if (hit.collider.gameObject.tag == "Next")
                    {
                         audioPlayer.Next();
                    }

                    if (hit.collider.gameObject.tag == "Previous")
                    {
                        audioPlayer.Previous();
                    }
                }
            }
        }
    }
}
