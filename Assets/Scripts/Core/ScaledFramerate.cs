using UnityEngine;

namespace Framerater.Core
{
    public class ScaledFramerate : AbstractFramerate
    {
        protected override float GetDeltaTime()
        {
            return Time.deltaTime;
        }
    }

}