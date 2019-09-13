using Framerater.Core;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Framerater.View
{
    public class FramerateTextView : MonoBehaviour
    {
        /// <summary>
        /// Text component that will display the framerate
        /// </summary>
#pragma warning disable CS0649
        [SerializeField] private Text _textComponent;
        [SerializeField] private AbstractFramerate _framerate;
#pragma warning restore CS0649

        protected virtual void OnEnable()
        {
            StartCoroutine(UpdateFramerateText());
        }

        private IEnumerator UpdateFramerateText()
        {
            while (enabled)
            {
                yield return new WaitForEndOfFrame();

                string framerate = Format(_framerate.NumFrames);
                string avgDeltaTime = Format(_framerate.AverageDeltaTime * 1000);
                string minDeltaTime = Format(_framerate.MinDeltaTime * 1000);
                string maxDeltaTime = Format(_framerate.MaxDeltaTime * 1000);
                _textComponent.text = string.Format("{0} FPS (avg: {1}ms - min: {2}ms - max: {3}ms)", framerate, avgDeltaTime, minDeltaTime, maxDeltaTime);
            }
        }

        private string Format(float f)
        {
            double d = System.Convert.ToDouble(f);
            return d.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
        }
    }

}