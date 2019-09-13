using Framerater.Core;
using System.Collections;
using UnityEngine;

namespace Framerater.StressTest
{
    public class FrameratePitch : MonoBehaviour
    {
#pragma warning disable CS0649
        [SerializeField] private AudioSource _audioSource;
        [Range(30f, 240f)][SerializeField] private float _targetFramerate = 60f;
        [SerializeField] private AbstractFramerate _framerater;
        [Range(-2f, 2f)][SerializeField] private float _defaultPitch = 1f;
#pragma warning restore CS0649        

        protected virtual void OnEnable()
        {
            StartCoroutine(UpdatePitch());
        }

        private IEnumerator UpdatePitch()
        {
            _audioSource.pitch = _defaultPitch;
            while (enabled)
            {
                yield return new WaitForEndOfFrame();

                float pitch = _defaultPitch;
                if (_framerater.NumFrames <= _targetFramerate)
                {
                    pitch = Mathf.InverseLerp(0f, _targetFramerate, _framerater.NumFrames);
                }
                _audioSource.pitch = pitch;
            }
        }

        protected virtual void OnDisable()
        {
            _audioSource.pitch = _defaultPitch;
        }
    }
}
