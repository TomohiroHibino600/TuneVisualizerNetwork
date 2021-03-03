using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace TuneVisualizer
{
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] AudioClip[] clips;
        [SerializeField] AudioSource source;

        public IMusicIndex musicIndex;
        private IntReactiveProperty reactIndex = new IntReactiveProperty();

        public IMusicSecond musicSecond;
        private FloatReactiveProperty reactSecond = new FloatReactiveProperty();

        // Start is called before the first frame update
        void Start()
        {
            musicIndex = GameObject.FindGameObjectWithTag("MusicIndex").GetComponent<IMusicIndex>();
            reactIndex.Value = musicIndex.GetValue();
            reactIndex.Subscribe(value => SetClipToSource(value)).AddTo(gameObject);

            musicSecond = GameObject.FindGameObjectWithTag("MusicSecond").GetComponent<IMusicSecond>();
            reactSecond.Value = musicSecond.Second;
            reactSecond.Subscribe(value => SetReactToNetworkedSec(value)).AddTo(gameObject);
        }

        private void SetClipToSource(int value)
        {
            source.clip = clips[value];
            SetAudioTimeToReact();
            source.Play();
        }

        public void Next()
        {
            musicIndex.AddIndex(clips.Length);
            reactIndex.Value = musicIndex.GetValue();
        }

        public void Previous()
        {
            musicIndex.SubtractIndex();
            reactIndex.Value = musicIndex.GetValue();
        }

        public void Play()
        {
            SetAudioTimeToReact();
            source.Play();
        }

        public void Stop()
        {
            SetAudioTimeToReact();
            source.Stop();
        }

        private void SetAudioTimeToReact()
        {
            reactSecond.Value = source.time;
        }

        private void SetReactToNetworkedSec(float value)
        {
            musicSecond.Second = value;
        }
    }
}
