using Framerater.Core;
using Framerater.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framerater.View
{
    [ExecuteAlways]
    public class FramerateGraphView : MonoBehaviour
    {        
#pragma warning disable CS0649        
        [SerializeField] private Material _material;
        [SerializeField] private RectOffsetNormal _borderOffset;
        [Range(0, 0.025f)] [SerializeField] private float _linePadding;
        [Range(30, 240)] [SerializeField] private int _maxFramerate = 60;
        [SerializeField] private AbstractFramerate _framerate;
        [SerializeField] private Color _borderColor = Color.white;
        [SerializeField] private Color _graphColor = Color.red;
#pragma warning restore CS0649

        private List<float> _frameCaches = new List<float>();
        private int _maxFrameCaches = 60;

        protected virtual void OnValidate()
        {
            _borderOffset.right = Mathf.Max(_borderOffset.left, _borderOffset.right);
            _borderOffset.bottom = Mathf.Min(_borderOffset.top, _borderOffset.bottom);
        }

        protected virtual void OnEnable()
        {
            if (Application.isPlaying)
            {
                _frameCaches.Clear();
                for (int i = 0; i < _maxFrameCaches; i++)
                {
                    _frameCaches.Add(0f);
                }

                StartCoroutine(UpdateFrameCaches());
            }
        }

        protected virtual void OnPostRender()
        {
            // Draw border
            //float left = _borderOffset.left / (float)Screen.height;
            //float right = (Screen.height - _borderOffset.right) / (float)Screen.height;
            //float top = (Screen.width - _borderOffset.top) / (float)Screen.width;
            //float bottom = _borderOffset.bottom / (float)Screen.width;

            

            using (new GLMatrixScope())
            {
                _material.SetPass(0);
                GL.LoadOrtho();                
                using (new GLScope(GL.LINE_STRIP))
                {
                    GL.Color(_borderColor);
                    GL.Vertex3(_borderOffset.left, _borderOffset.top, 0f);
                    GL.Vertex3(_borderOffset.right, _borderOffset.top, 0f);
                    GL.Vertex3(_borderOffset.right, _borderOffset.bottom, 0f);
                    GL.Vertex3(_borderOffset.left, _borderOffset.bottom, 0f);
                    GL.Vertex3(_borderOffset.left, _borderOffset.top, 0f);
                }
            }

            if (Application.isPlaying)
            {
                // Draw fps
                using (new GLMatrixScope())
                {
                    _material.SetPass(0);
                    GL.LoadOrtho();
                    using (new GLScope(GL.LINE_STRIP))
                    {
                        GL.Color(_graphColor);

                        float height = _borderOffset.top - _borderOffset.bottom;
                        float width = _borderOffset.right - _borderOffset.left;
                        if (height > _linePadding * 2f && width > _linePadding)
                        {
                            for (int i = 0; i < _frameCaches.Count; i++)
                            {
                                Vector3 dot = Vector3.zero;
                                dot.x = Mathf.Lerp(_borderOffset.left + _linePadding, _borderOffset.right - _linePadding, i / (_frameCaches.Count - 1f));
                                dot.y = Mathf.Lerp(_borderOffset.bottom + _linePadding, _borderOffset.top - _linePadding, Mathf.Min(_frameCaches[i] / _maxFramerate, 1f));
                                GL.Vertex(dot);
                            }
                        }
                    }
                }
            }
        }

        private IEnumerator UpdateFrameCaches()
        {
            float elapsedTime = 0f;
            while (enabled)
            {
                while (elapsedTime <= 1f)
                {                    
                    yield return new WaitForEndOfFrame();
                    elapsedTime += Time.unscaledDeltaTime;
                }

                elapsedTime -= 1f;

                _frameCaches.RemoveAt(0);
                _frameCaches.Add(_framerate.NumFrames);
            }
        }
    }
}