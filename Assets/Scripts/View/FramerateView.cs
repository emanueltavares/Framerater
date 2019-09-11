using Framerater.Core;
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
            string framerate = Mathf.Round(_framerateComponent.NumFrames).ToString();
            _textComponent.text = string.Format("{0} FPS", framerate);
        }
    }

}