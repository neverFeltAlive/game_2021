/// <remarks>
/// 
/// CameraController is used for controlling cameraTransform movement
/// NeverFeltAlive
/// 
/// </remarks>


using System;
using System.Collections;
using UnityEngine;

using Custom.Utils;
using Platformer.Mechanics.Player;

namespace Platformer.Camera
{
    public class CameraController : MonoBehaviour
    /* DEBUG statements for this document 
     * 
     * Debug.Log("CameraController --> Start:");
     * Debug.Log("<size=13><i><b> CameraController --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> CameraController --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> CameraController --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> CameraController --> </b></i><color=green> Function: </color></size>");
     * 
     */
    /* TODO
     * 
     */
    {
        public enum CameraState
        {
            Normal,
            Shaking
        }



        #region Serialized Fields
        [Header("Links")]
        [SerializeField] [Tooltip("Transform component of the object to follow")] private Transform target;
        [SerializeField] [Tooltip("Transform component of the camera")] private Transform cameraTransform;
        [Space]

        [Header("Variables")]
        [SerializeField] [Range(0f, 2f)] [Tooltip("Time since the begining of player movement when camera starts following")] private float timeOffset = 0.25f;
        #endregion

        #region Private Fields
        private float currentTimeOffset;
        private float currentXOffset;
        private float currentYOffset;
        private float currentZOffset;

        private CameraState state;

        private Vector3 velocity = Vector3.zero;
        #endregion

        #region Public Fields
        public float maxX = 0f;
        public float minX = 0f;
        public float maxY = 0f;
        public float minY = 0f;
        public float zOffset = -10f;
        public float xOffset = 0f;
        public float yOffset = 0f;
        #endregion



        #region MonoBehaviour Callbacks
        private void Start()
        {
            if (!target)
                target = GameObject.Find(Constants.CHARACTER_NAME).transform;

            DashController.OnDashStateChanged += DashHandler;
            PlayerFightController.OnShoot += ShootHandler;

            ResetValues();
        }

        private void FixedUpdate()
        {
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, 0f);
            
            // Check for boarders
            if (target.position.x > maxX)
                targetPosition.x = maxX != 0 ? maxX : targetPosition.x;
            if (target.position.x < minX)
                targetPosition.x = minX != 0 ? minX : targetPosition.x;
            if (target.position.y > maxY)
                targetPosition.y = maxY != 0 ? maxY : targetPosition.y;
            if (target.position.y < minY)
                targetPosition.y = minY != 0 ? minY : targetPosition.y;

            // Check for offset 
            if (currentXOffset != 0)
                targetPosition.x += currentXOffset;
            if (currentYOffset != 0)
                targetPosition.y += currentYOffset;

            // Smooth following
            cameraTransform.position = Vector3.SmoothDamp(cameraTransform.position, targetPosition + new Vector3(0f, 0f, currentZOffset), ref velocity, currentTimeOffset);
        }
        #endregion

        #region Functions
        private void ResetValues()
        {
            currentTimeOffset = timeOffset;
            currentXOffset = xOffset;
            currentYOffset = yOffset;
            currentZOffset = zOffset;
            state = CameraState.Normal;
        }

        private void ShakeCam(float duration = .3f, float magnitude = .005f)
        {
            if (state == CameraState.Shaking)
                StopAllCoroutines();

            StartCoroutine(Shake(duration, magnitude));

        }

        #region Event Handlers
        private void DashHandler(object sender, DashController.OnDashStateChangedEventArgs args)
        {
            if (args.state == DashController.DashState.Active)
            {
                if (args.isPower)
                    ShakeCam(.5f, .05f);
                else
                    ShakeCam();
            }
        }

        private void ShootHandler(object sender, EventArgs args)
        {
            ShakeCam(.1f, .01f);
        }
        #endregion
        #endregion



        #region Coroutines
        IEnumerator Shake(float duration = .3f, float magnitude = .005f)
        {
            state = CameraState.Shaking;
            currentTimeOffset = 0f;

            while (duration > 0f)
            {
                duration -= Time.fixedDeltaTime;

                currentXOffset = UnityEngine.Random.Range(-1f, 1f) * magnitude;
                currentYOffset = UnityEngine.Random.Range(-1f, 1f) * magnitude;

                yield return new WaitForFixedUpdate();
            }

            ResetValues();
        }
        #endregion
    }
}
