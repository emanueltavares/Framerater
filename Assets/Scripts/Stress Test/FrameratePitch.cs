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
        [SerializeField] private AbstractFramerate _framerater;
#pragma warning restore CS0649        

        protected virtual void OnEnable()
        {
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
