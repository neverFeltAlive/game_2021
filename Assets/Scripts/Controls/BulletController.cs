using UnityEngine;

using Custom.Utils;
using Custom.Mechanics;

namespace Custom.Controlls
{
    /// <summary>
    /// 
    /// BulletController is used for controlling bullet behaciour
    /// :NeverFeltAlive
    /// 
    /// </summary>
    public class BulletController : MonoBehaviour
    {
        [HideInInspector] public string targetTag;
        [HideInInspector] public Damage damage;




        private void Update()
        {
            if (Vector3.Distance(damage.orrigin, transform.position) > 2f)
                Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.tag == targetTag)
            {
                IDamagable<Damage> enemy = collision.transform.GetComponent<IDamagable<Damage>>();

                if (enemy != null)
                    enemy.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
