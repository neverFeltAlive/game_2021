using System;
using UnityEngine;

using Custom.Controlls;
using Custom.Utils;

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



        [SerializeField] private float bulletSpeed = 4f;
        [SerializeField] private string targetTag;
        [SerializeField] private Damage damage;
        [Space]
        [SerializeField] private Transform firePoint;
        [SerializeField] private GameObject bulletPrefab;



        public virtual void Shoot(Vector3 direction)
        {
            if (direction != Vector3.zero)
            {
                direction.Normalize();

                damage.orrigin = transform.position;

                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                bullet.GetComponent<BulletController>().damage = damage;
                bullet.GetComponent<BulletController>().targetTag = targetTag;
                bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
                bullet.transform.Rotate(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

                OnShoot?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
