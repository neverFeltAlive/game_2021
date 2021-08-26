using System;
using UnityEngine;

namespace Custom.Mechanics
{
    /// <summary>
    /// 
    /// ShootProjectiles is used for shooting projectiles 
    /// :NeverFeltAlive
    /// 
    /// </summary>
    public class ShootProjectiles : MonoBehaviour
    {
        public event EventHandler OnShoot;



        [SerializeField] private Transform firePoint;
        [SerializeField] private GameObject bulletPrefab;



        public virtual void Shoot(Vector3 direction)
        {
            float bulletSpeed = 2f;

            direction.Normalize();

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
            bullet.transform.Rotate(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

            OnShoot?.Invoke(this, EventArgs.Empty);
        }
    }
}
