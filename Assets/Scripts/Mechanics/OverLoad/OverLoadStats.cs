using UnityEngine;

namespace Custom.Mechanics
{
    [CreateAssetMenu(fileName = "OverLoadStats", menuName = "OverLoad Stats")]
    public class OverLoadStats : ScriptableObject
    {
        public float overLoadTime = 3f;
        public float cooldownTime = 100f;
    }
}
