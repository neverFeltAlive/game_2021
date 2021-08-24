/// <remarks>
/// 
/// RollVisualsController is used for controlling roll animation
/// NeverFeltAlive
/// 
/// </remarks>


using UnityEngine;

using Custom.Mechanics;

namespace Custom.Visuals
{
    [RequireComponent(typeof(Roll))]
    public class RollVisualsController : AnimationTrigger<Roll>
    {
    }
}
