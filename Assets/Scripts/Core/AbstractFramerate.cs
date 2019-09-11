using System;
using System.Collections;
using UnityEngine;

namespace Framerater.Core
{
    public abstract class AbstractFramerate : MonoBehaviour, IFramerate
    {        
        /// <summary>
        /// Elapsed time until we reach Max Seconds
        /// </summary>
        private float _elapsedTime = 0f;

        /// <summary>
        /// Total of frames obtained until we reach Max Seconds;
        /// </summary>
        private float _frameCount = 0f;

        /// <summary>
        /// Average value of the frame, per second
        /// </summary>
        public float NumFrames { get; private set; }

        protected virtual void OnEnable()
        {
            _frameCount = 0f;
            _elapsedTime = 0f;
            NumFrames = 0f;

            StartCoroutine(UpdateFramerate());
        }

        private IEnumerator UpdateFramerate()
        {
            while (enabled)
            {
                yield return new WaitForEndOfFrame();

                if (_elapsedTime >= 1f)
                {
                    //float exceedingElapsedTime = (_elapsedTime - _maxSeconds) / _maxSeconds;
                    float frameValue = 1 / _elapsedTime;
                    _frameCount += frameValue;

                    // Update value
                    NumFrames = _frameCount;

                    // Update elapsed time
                    _elapsedTime -= 1f;

                    // Reset frame count
                    _frameCount = 1f - frameValue;
                }
                else
                {
                    _frameCount += 1;
                }

                _elapsedTime += GetDeltaTime();
            }
        }

        protected abstract float GetDeltaTime();
    }
}