using System;
using UnityEngine;

using Custom.Controlls;

namespace Custom.Mechanics
{
    /// <summary>
    /// 
    /// ShootProjectiles is used for shooting projectiles 
    /// :NeverFeltAlive
    /// 
    /// </summary>
    public class ShootProjectiles : MonoBehaviour, IRangeAttack
    {
        public event EventHandler OnShoot;



        [SerializeField] private ShootPeojectilesStats shootProjectilesStats;
        [SerializeField] private Transform firePoint;
        [SerializeField] private LayerMask obstacleMask;

        private const float BULLET_SPEED = 4f;



        public virtual void Shoot(Vector3 direction)
        {
            if (direction != Vector3.zero)
            {
                direction.Normalize();

                shootProjectilesStats.damage.orrigin = transform.position;

                float distance = Vector3.Distance(transform.position, firePoint.position);
                RaycastHit2D hit = Physics2D.Raycast(transform.position, firePoint.position, distance, obstacleMask);
                if (hit.collider == null)
                {
                    GameObject bullet = Instantiate(shootProjectilesStats.bulletPrefab, firePoint.position, Quaternion.identity);
                    bullet.GetComponent<BulletController>().damage = shootProjectilesStats.damage;
                    bullet.GetComponent<Rigidbody2D>().velocity = direction * BULLET_SPEED;
                    bullet.transform.Rotate(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
                }

                OnShoot?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
