using UnityEngine;

namespace Custom.Controlls
{
    /// <summary>
    /// 
    /// TimeController is used for controlling time flow 
    /// :NeverFeltAlive
    /// 
    /// </summary>
    public class TimeController : MonoBehaviour
    {
        private float speedUpTime;
        private float targetTimeScale;
        private float _customDeltaTime;

        public float CustomDeltaTime 
        { 
            get { return _customDeltaTime; } 
            set 
            {
                if (value > 1f)
                    _customDeltaTime = 1f;
                else if (value < 0f)
                    _customDeltaTime = 0f;
                else
                    _customDeltaTime = value;
            } 
        }

        public static TimeController Instance { get; private set; }



        private void Awake() =>
            targetTimeScale = 1;

        private void OnEnable() =>
            Instance = this;

        private void Update()
        {
            if (Time.timeScale != targetTimeScale)
            {
                float tempTimeScale = Time.timeScale + (1f / speedUpTime) * Time.unscaledDeltaTime;
                ChangeTimeScale(tempTimeScale);
                Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, targetTimeScale);
            }
        }

        /// <summary>
        /// Changes the time scale and fixed delta time to a specific value
        /// </summary>
        public void ChangeTimeScale(float targetTimeScale)
        {
            if (targetTimeScale >= 0 && targetTimeScale <= 100)
            {
                Time.timeScale = targetTimeScale;
                Time.fixedDeltaTime = .02f * Time.timeScale;
            }
        }
        /// <summary>
        /// Triggeres gradual change of the time scale and fixed delta time 
        /// </summary>
        /// <param name="speedUpTime">In what amount of time the time scale reaches the target time scale</param>
        public void ChangeTimeScale(float targetTimeScale, float speedUpTime)
        {
            this.speedUpTime = speedUpTime;
            this.targetTimeScale = targetTimeScale;
        }

        public void ResetTimeScale() =>
            targetTimeScale = 1f;
    }
}
