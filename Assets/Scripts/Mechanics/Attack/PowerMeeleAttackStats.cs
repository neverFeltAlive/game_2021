using UnityEngine;

using Custom.Utils;

namespace Custom.Mechanics
{
    [CreateAssetMenu(fileName = "PowerMeeleAttackStats", menuName = "Power Meele Attack Stats")]
    public class PowerMeeleAttackStats : ScriptableObject
    {
        [Space] [Header("Power Attack Stats")]
        public float powerAttackDashRange;
        public float powerAttackRange;
        public Damage powerAttackDamage;
    }
}
