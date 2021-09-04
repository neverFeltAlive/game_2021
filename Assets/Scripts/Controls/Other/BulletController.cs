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
        private Rigidbody2D body;
        private Vector3 lastVelocity;



        private void Awake() =>
            body = GetComponent<Rigidbody2D>();

        private void Update()
        {
            if (Vector3.Distance(damage.orrigin, transform.position) > 2f)
                Destroy(gameObject);

            lastVelocity = body.velocity;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            float currentSpeed = lastVelocity.magnitude;
            Vector3 direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
            transform.right = lastVelocity.normalized;
            //transform.Rotate(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            //transform.rotation = Quaternion.LookRotation(direction);
            body.velocity = direction * currentSpeed;

/*            if (collision.transform.tag == targetTag)
            {
                IDamagable<Damage> enemy = collision.transform.GetComponent<IDamagable<Damage>>();

                if (enemy != null)
                    enemy.TakeDamage(damage);
            }

            Destroy(gameObject);*/
        }
    }
}
