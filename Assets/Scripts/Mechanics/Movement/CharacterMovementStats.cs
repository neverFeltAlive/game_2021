using UnityEngine;

namespace Custom.Mechanics
{
    [CreateAssetMenu(fileName = "CharacterMovementStats", menuName = "Character Movement Stats")]
    public class CharacterMovementStats : ScriptableObject
    {
        public bool isScaled;
        public float maxSpeed;
    }
}
