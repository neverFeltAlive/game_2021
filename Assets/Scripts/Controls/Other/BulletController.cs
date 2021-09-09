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
        private int bounceCount;

        private Vector3 lastVelocity;

        private Rigidbody2D body;

        [HideInInspector] public string targetTag;
        [HideInInspector] public Damage damage;



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
            if (collision.gameObject.tag == Constants.WALL_TAG)
                Bounce(collision);
            else if (collision.gameObject.tag == Constants.PLAYER_TAG || collision.gameObject.tag == Constants.ENEMY_TAG)
                Damage(collision);
        }

        private void Damage(Collision2D collision)
        {
            IDamagable<Damage> enemy = collision.transform.GetComponent<IDamagable<Damage>>();

            if (enemy != null)
                enemy.TakeDamage(damage);

            Destroy(gameObject);
        }

        private void Bounce(Collision2D collision)
        {
            int maxBounceCount = 3;

            bounceCount++;
            if (bounceCount > maxBounceCount)
                Destroy(gameObject);
            else
            {
                float currentSpeed = lastVelocity.magnitude;
                Vector3 direction = Vector3.Reflect(lastVelocity, collision.GetContact(0).normal);
                body.velocity = direction.normalized * currentSpeed;
                transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            }
        }
    }
}
