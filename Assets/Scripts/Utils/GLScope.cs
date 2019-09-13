using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framerater.Utils
{
    public class GLScope : System.IDisposable
    {
        public GLScope(int mode)
        {
            GL.Begin(mode);
        }

        public void Dispose()
        {
            GL.End();
        }
    }
}