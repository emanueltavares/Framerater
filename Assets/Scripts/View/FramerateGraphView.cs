using Framerater.Core;
using Framerater.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framerater.View
{
    public class FramerateGraphView : MonoBehaviour
    {
#pragma warning disable CS0649        
        [SerializeField] private Material _material;
        [SerializeField] private RectOffset _rectOffset;
        private IFramerate _framerate;
#pragma warning restore CS0649

        private List<float> _frameCaches = new List<float>();
        private int _maxFrameCaches = 60;

        protected virtual void OnEnable()
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

        protected virtual void OnPostRender()
        {
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
                        dot.x = (float) i / _frameCaches.Count;
                        dot.y = Mathf.Min(_frameCaches[i] / 60f, 1f);
                        GL.Vertex(dot);
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