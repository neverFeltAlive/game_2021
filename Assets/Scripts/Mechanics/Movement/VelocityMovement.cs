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
    /* DEBUG statements for this document 
     * 
     * Debug.Log("VelocityMovement --> Start: ");
     * Debug.Log("<size=13><i><b> VelocityMovement --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> VelocityMovement --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> VelocityMovement --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> VelocityMovement --> </b></i><color=green> Function: </color></size>");
     * 
     */
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
            Move(_direction, speed);

        public virtual void Move(Vector3 direction, float speed) =>
            body.velocity = direction.normalized * speed;
    }
}
