using UnityEngine;

namespace Custom.Mechanics
{
    /// <summary>
    /// 
    /// CharacterMovement is used for extending movement logic.
    /// It inherits from VelocityMovement and allows to dinamically modify speed and disable default movement
    /// :NeverFeltAlive
    /// 
    /// </summary>
    public class CharacterMovement : VelocityMovement, IDisablableMovement
    {
        [SerializeField] protected CharacterMovementStats movementStats;

        protected bool isDisabled;

        private float? timer = null;

        public float MaxSpeed {
            get { return movementStats.maxSpeed; }
        }



        #region MonoBehaviour Callbacks
        protected override void FixedUpdate()
        {
            if (isDisabled)
                return;

            speed = Mathf.Clamp(_direction.magnitude, 0, movementStats.maxSpeed);
            base.FixedUpdate();
        }

        protected virtual void Update()
        {
            if (timer != null)
                CheckTimer();
        }
        #endregion

        #region Functions
        private void CheckTimer()
        {
            if (timer - Time.deltaTime <= 0)
            {
                EnableMovement();
                timer = null;
            }
            else
                timer -= Time.deltaTime;
        }

        public void DisableMovement()
        {
            body.velocity = Vector2.zero;
            isDisabled = true;
        }
        public void DisableMovement(float time)
        {
            timer = time;
            DisableMovement();
        }

        public void EnableMovement() =>
            isDisabled = false;
        #endregion

        public bool IsMoving()
        {
            if (body.velocity == Vector2.zero)
                return false;
            else
                return true;
        }
    }
}
