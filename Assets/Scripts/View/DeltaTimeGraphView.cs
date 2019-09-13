using Framerater.Core;
using Framerater.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framerater.View
{
    [ExecuteAlways]
    public class DeltaTimeGraphView : MonoBehaviour
    {
#pragma warning disable CS0649        
        [SerializeField] private Material _material;
        [SerializeField] private RectOffsetNormal _borderOffset;
        [Range(0, 0.025f)] [SerializeField] private float _linePadding;
        [Range(0.025f, 0.075f)][SerializeField] private float _maxDeltaTime = 0.01f;
        [SerializeField] private AbstractFramerate _framerate;
        [SerializeField] private Color _borderColor = Color.white;
        [SerializeField] private Color _minDeltaTimeGraphColor = Color.green;
        [SerializeField] private Color _avgDeltaTimeGraphColor = Color.yellow;
        [SerializeField] private Color _maxDeltaTimeGraphColor = Color.red;
#pragma warning restore CS0649

        private List<float> _minDeltaTimeCaches = new List<float>();
        private List<float> _avgDeltaTimeCaches = new List<float>();
        private List<float> _maxDeltaTimeCaches = new List<float>();
        private int _numDeltaTimeCaches = 60;

        protected virtual void OnValidate()
        {
            _borderOffset.right = Mathf.Max(_borderOffset.left, _borderOffset.right);
            _borderOffset.bottom = Mathf.Min(_borderOffset.top, _borderOffset.bottom);
        }

        protected virtual void OnEnable()
        {
            if (Application.isPlaying)
            {
                _minDeltaTimeCaches.Clear();
                _avgDeltaTimeCaches.Clear();
                _maxDeltaTimeCaches.Clear();
                for (int i = 0; i < _numDeltaTimeCaches; i++)
                {
                    _minDeltaTimeCaches.Add(0f);
                    _avgDeltaTimeCaches.Add(0f);
                    _maxDeltaTimeCaches.Add(0f);
                }

                StartCoroutine(UpdateDeltaTimeCaches());
            }
        }

        protected virtual void OnPostRender()
        {
            // Draw border
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
                using (new GLMatrixScope())
                {
                    _material.SetPass(0);
                    GL.LoadOrtho();

                    float height = _borderOffset.top - _borderOffset.bottom;
                    float width = _borderOffset.right - _borderOffset.left;
                    if (height > _linePadding * 2f && width > _linePadding)
                    {
                        // Draw min delta time
                        DrawLine(_minDeltaTimeCaches, _minDeltaTimeGraphColor, _borderOffset.top, _borderOffset.left, _borderOffset.bottom, _borderOffset.right);
                        // Draw avg delta time
                        DrawLine(_avgDeltaTimeCaches, _avgDeltaTimeGraphColor, _borderOffset.top, _borderOffset.left, _borderOffset.bottom, _borderOffset.right);
                        // Draw max delta time
                        DrawLine(_maxDeltaTimeCaches, _maxDeltaTimeGraphColor, _borderOffset.top, _borderOffset.left, _borderOffset.bottom, _borderOffset.right);
                    }                    
                }
            }
        }

        private void DrawLine(List<float> deltaTimeCaches, Color color, float top, float left, float bottom, float right)
        {
            using (new GLScope(GL.LINE_STRIP))
            {
                GL.Color(color);

                for (int i = 0; i < deltaTimeCaches.Count; i++)
                {
                    Vector3 dot = Vector3.zero;
                    dot.x = Mathf.Lerp(left + _linePadding, right - _linePadding, i / (deltaTimeCaches.Count - 1f));
                    dot.y = Mathf.Lerp(bottom + _linePadding, top - _linePadding, Mathf.Min(deltaTimeCaches[i] / _maxDeltaTime, 1f));
                    GL.Vertex(dot);
                }
            }                
        }

        private IEnumerator UpdateDeltaTimeCaches()
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

                _minDeltaTimeCaches.RemoveAt(0);
                _minDeltaTimeCaches.Add(_framerate.MinDeltaTime);

                _avgDeltaTimeCaches.RemoveAt(0);
                _avgDeltaTimeCaches.Add(_framerate.AverageDeltaTime);

                _maxDeltaTimeCaches.RemoveAt(0);
                _maxDeltaTimeCaches.Add(_framerate.MaxDeltaTime);
            }
        }
    }
}