using UnityEngine;

namespace Custom.Mechanics
{
    /// <summary>
    /// 
    /// VelocityMovement is used for basic movement action.
    /// It uses rigit body velocity.
    /// :NeverFeltAlive
    /// 
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class VelocityMovement : MonoBehaviour, IMovement
    {
        protected float speed = 1f;

        protected Vector3 _direction;

        protected Rigidbody2D body;

        public virtual Vector3 Direction 
        {
            get { return _direction; }
            set { _direction = value; }
        }



        protected virtual void Awake() =>
            body = GetComponent<Rigidbody2D>();

        protected virtual void FixedUpdate() =>
            Move();

        protected virtual void Move() =>
            body.velocity = _direction.normalized * speed;
    }
}
