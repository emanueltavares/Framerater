﻿using Framerater.Core;
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
        [SerializeField] private RectOffset _borderOffset;
        [Range(0, 0.025f)][SerializeField] private float _linePadding;
        private IFramerate _framerate;
#pragma warning restore CS0649

        private List<float> _frameCaches = new List<float>();
        private int _maxFrameCaches = 60;

        protected virtual void OnEnable()
        {
            if (Application.isPlaying)
            {
                _frameCaches.Clear();
                for (int i = 0; i < _maxFrameCaches; i++)
                {
                    _frameCaches.Add(0f);
                }

                if (_framerate == null)
                {
                    _framerate = GetComponent<IFramerate>();
                }

                StartCoroutine(UpdateFrameCaches());
            }            
        }

        protected virtual void OnPostRender()
        {            
            // Draw border
            float left = _borderOffset.left / (float)Screen.height;
            float right = (Screen.height - _borderOffset.right) / (float)Screen.height;
            float top = (Screen.width - _borderOffset.top) / (float)Screen.width;
            float bottom = _borderOffset.bottom / (float)Screen.width;

            using (new GLMatrixScope())
            {
                _material.SetPass(0);
                GL.LoadOrtho();
                using (new GLScope(GL.LINE_STRIP))
                {
                    GL.Vertex3(left, top, 0f);
                    GL.Vertex3(right, top, 0f);
                    GL.Vertex3(right, bottom, 0f);
                    GL.Vertex3(left, bottom, 0f);
                    GL.Vertex3(left, top, 0f);
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
                        GL.Color(Color.red);

                        for (int i = 0; i < _frameCaches.Count; i++)
                        {
                            Vector3 dot = Vector3.zero;
                            dot.x = Mathf.Lerp(left + _linePadding, right - _linePadding, i / (_frameCaches.Count - 1f));
                            dot.y = Mathf.Lerp(bottom + _linePadding, top - _linePadding, Mathf.Min(_frameCaches[i] / 60f, 1f));
                            GL.Vertex(dot);
                        }
                    }
                }
            }
        }

        private IEnumerator UpdateFrameCaches()
        {
            while (enabled)
            {
                yield return new WaitForEndOfFrame();

                _frameCaches.RemoveAt(0);
                _frameCaches.Add(_framerate.NumFrames);
            }
        }
    }
}