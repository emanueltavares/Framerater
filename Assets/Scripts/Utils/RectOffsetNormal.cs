using UnityEngine;

namespace Framerater.Utils
{
    [System.Serializable]
    public struct RectOffsetNormal
    {
        [Range(0f, 1f)] public float left;
        [Range(0f, 1f)] public float right;
        [Range(0f, 1f)] public float top;
        [Range(0f, 1f)] public float bottom;
    }
}