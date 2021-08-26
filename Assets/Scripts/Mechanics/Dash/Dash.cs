using System;
using UnityEngine;


namespace Custom.Mechanics
{
    /// <summary>
    /// 
    /// Dash is used for dashing character
    /// :NeverFeltAlive
    /// 
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class Dash : MonoBehaviour
    {
        public event EventHandler<OnDashEventArgs> OnDash;
        public class OnDashEventArgs : EventArgs
        {
            public Vector3 direction;
        }

        

        #region Fields
        [SerializeField] protected float force = .45f;

        private Rigidbody2D body;
        #endregion



        protected virtual void Awake() =>
            body = GetComponent<Rigidbody2D>();

        public virtual void PerformDash(Vector3 direction)
        {
            if (direction != Vector3.zero)
            {
                direction = ModifyDirection(direction);
                body.MovePosition(transform.position + direction * force);
                OnDash?.Invoke(this, new OnDashEventArgs { direction = direction });
            }
        }

        /// <summary>
        /// Modifies vector to be have specific magnitude
        /// </summary>
        /// <param name="vector">Vector to be modified</param>
        /// <returns></returns>
        private Vector3 ModifyDirection(Vector3 vector)
        {
            float minMagnitude = .45f;

            if (vector.magnitude > minMagnitude)
                return vector;
            else 
                return vector * minMagnitude / vector.magnitude;
        }
    }
}
