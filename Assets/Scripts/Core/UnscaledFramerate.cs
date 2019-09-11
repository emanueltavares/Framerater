using UnityEngine;

namespace Framerater.Core
{
    public class UnscaledFramerate : AbstractFramerate
    {
        protected override float GetDeltaTime()
        {
            return Time.unscaledDeltaTime;
        }
    }

}