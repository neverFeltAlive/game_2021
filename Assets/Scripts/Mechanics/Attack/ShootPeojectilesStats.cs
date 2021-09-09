using UnityEngine;

using Custom.Utils;

namespace Custom.Mechanics
{
    [CreateAssetMenu(fileName = "ShootProjectilesStats", menuName = "Shoot Projectiles Stats")]
    public class ShootPeojectilesStats : ScriptableObject
    {
        public Damage damage;
        [Space] 
        public GameObject bulletPrefab;
    }
}
