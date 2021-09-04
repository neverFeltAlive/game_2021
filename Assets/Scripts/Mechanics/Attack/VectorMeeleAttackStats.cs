using UnityEngine;

using Custom.Utils;

namespace Custom.Mechanics
{
    [CreateAssetMenu(fileName = "VectorMeeleAttackStats", menuName = "Vector Meele Attack Stats")]
    public class VectorMeeleAttackStats : ScriptableObject
    {
        public string targetTag;

        public Damage damage;
    }
}
