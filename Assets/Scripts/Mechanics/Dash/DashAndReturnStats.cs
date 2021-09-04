using UnityEngine;

namespace Custom.Mechanics
{
    [CreateAssetMenu(fileName = "DashAndReturnStats", menuName = "Dash And Return Stats")]
    public class DashAndReturnStats : ScriptableObject
    {
        [Range(1, 5)] [Tooltip("Max number of dashed available at once")] public int maxNumberOfDashes = 5;
        [Space] [Header("Cooldown settings")]
        [Range(1f, 10f)] [Tooltip("Dash cooldown time in seconds")] public float cooldownTime = 3f;
        [Range(1f, 10f)] [Tooltip("Dash cooldown time after every return in seconds")] public float returnCooldownTime = 2f;
        [Range(0f, 10f)] [Tooltip("Dash cooldown time after last return in seconds")] public float lastReturnCooldownTime = .5f;
        [Range(.1f, 5f)] [Tooltip("Time  after one dash untill coldown is triggered")] public float cooldownTriggeringTime = .8f;
    }
}
