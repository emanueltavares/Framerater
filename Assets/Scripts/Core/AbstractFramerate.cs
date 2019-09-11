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

        /// <summary>
        /// Average delta time of a collection of frames in a single second
        /// </summary>
        public float AverageDeltaTime { get; private set; }

        /// <summary>
        /// Min delta time of a collection of frames in a single second
        /// </summary>
        public float MinDeltaTime { get; private set; }
        
        /// <summary>
        /// Max delta time of a collection of frames in a single second
        /// </summary>
        public float MaxDeltaTime { get; private set; }

        protected virtual void OnEnable()
        {
            _frameCount = 0f;
            _elapsedTime = 0f;
            NumFrames = 0f;
            AverageDeltaTime = 0f;
            MinDeltaTime = 1f;
            MaxDeltaTime = 0f;

            StartCoroutine(UpdateFramerate());
        }

        private IEnumerator UpdateFramerate()
        {
            float maxDeltaTime = MaxDeltaTime;
            float minDeltaTime = MinDeltaTime;
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

                    // Update average delta time
                    AverageDeltaTime = _elapsedTime / NumFrames;
                   
                    // Update Minimum delta time
                    MinDeltaTime = minDeltaTime;
                    minDeltaTime = 100f;
                    
                    // Update maximum delta time
                    MaxDeltaTime = maxDeltaTime;
                    maxDeltaTime = 0f;

                    // Update elapsed time
                    _elapsedTime -= 1f;

                    // Reset frame count
                    _frameCount = 1f - frameValue;
                }
                else
                {
                    _frameCount += 1;
                }

                // Update Delta Time
                float deltaTime = GetDeltaTime();
                if (deltaTime > maxDeltaTime)
                {
                    maxDeltaTime = deltaTime;
                }
                if (deltaTime < minDeltaTime)
                {
                    minDeltaTime = deltaTime;
                }
                _elapsedTime += deltaTime;
            }
        }

        protected abstract float GetDeltaTime();
    }
}