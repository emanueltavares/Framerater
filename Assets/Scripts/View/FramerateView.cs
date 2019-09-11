﻿using Framerater.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Framerater.View
{
    public class FramerateView : MonoBehaviour
    {
        /// <summary>
        /// Text component that will display the framerate
        /// </summary>
        [SerializeField] private Text _textComponent;

        /// <summary>
        /// Framerate component that will calculate the framerate
        /// </summary>
        private IFramerate _framerateComponent;

        protected virtual void OnEnable()
        {
            if (_framerateComponent == null)
            {
                _framerateComponent = GetComponent<IFramerate>();
            }
        }

        protected virtual void Update()
        {
            string framerate = Format(_framerateComponent.NumFrames);
            string avgDeltaTime = Format(_framerateComponent.AverageDeltaTime * 1000);
            _textComponent.text = string.Format("{0} FPS ({1}ms)", framerate, avgDeltaTime);
        }

        private string Format(float f)
        {
            double d = System.Convert.ToDouble(f);
            return d.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
        }
    }

}