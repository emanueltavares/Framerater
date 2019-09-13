using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framerater.Utils
{
    public class GLMatrixScope : System.IDisposable
    {
        public GLMatrixScope()
        {
            GL.PushMatrix();
        }

        public void Dispose()
        {
            GL.PopMatrix();
        }
    }
}