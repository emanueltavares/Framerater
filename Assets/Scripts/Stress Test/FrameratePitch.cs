using Framerater.Core;
using System.Collections;
using UnityEngine;

namespace Framerater.StressTest
{
    public class FrameratePitch : MonoBehaviour
    {
#pragma warning disable CS0649
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private float _targetFramerate = 60f;
#pragma warning restore CS0649

        private IFramerate _framerater;

        protected virtual void OnEnable()
        {
            if (_framerater == null)
            {
                _framerater = GetComponent<IFramerate>();
            }

            StartCoroutine(UpdatePitch());
        }

        private IEnumerator UpdatePitch()
        {
            while (enabled)
            {
                yield return new WaitForEndOfFrame();

                float pitch = 1f;
                if (_framerater.NumFrames <= _targetFramerate)
                {
                    pitch = Mathf.InverseLerp(0f, _targetFramerate, _framerater.NumFrames);
                }
                _audioSource.pitch = pitch;
            }
        }
    }
}
