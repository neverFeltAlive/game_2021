/// <remarks>
/// 
/// CameraController is used for controlling cameraTransform movement
/// NeverFeltAlive
/// 
/// </remarks>


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Platformer.Utils;

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
        // Fields

        #region Serialized Fields
        [Header("Links")]
        [SerializeField] [Tooltip("Transform component of the object to follow")] private Transform target;
        [SerializeField] [Tooltip("Transform component of the camera")] private Transform cameraTransform;
        [Space]

        [Header("Variables")]
        [SerializeField] [Range(0f, 2f)] [Tooltip("Time since the begining of player movement when camera starts following")] private float timeOffset = 0.25f;
        [SerializeField] [Tooltip("Required difference between coordinates on z axis")] private float _zOffset = -10f;
        [SerializeField] [Tooltip("Required difference between coordinates on x axis")] private float _xOffset = 0f;
        [SerializeField] [Tooltip("Required difference between coordinates on y axis")] private float _yOffset = 0f;
        #endregion

        #region Private Fields
        private float _maxX = 0f;
        private float _minX = 0f;
        private float _maxY = 0f;
        private float _minY = 0f;

        private Vector3 velocity = Vector3.zero;
        #endregion

        #region Properties
        [HideInInspector]
        public float XOffset { get { return _xOffset; } set { _xOffset = value; } }        
        [HideInInspector]
        public float YOffset { get { return _yOffset; } set { _yOffset = value; } }
        [HideInInspector]
        public float MaxX { get { return _maxX; } set { _maxX = value; } }
        [HideInInspector]
        public float MinX { get { return _minX; } set { _minX = value; } }
        [HideInInspector]
        public float MaxY { get { return _maxY; } set { _maxY = value; } }
        [HideInInspector]
        public float MinY { get { return _minY; } set { _minY = value; } }
        #endregion

        // Functions 

        #region MonoBehaviour Callbacks
        // When the script is enabled
        private void Start()
        {
            // Check required variables for assignment
            if (!target)
                target = GameObject.Find(Constants.CHARACTER_NAME).transform;
        }

        // Every fixed time period
        private void FixedUpdate()
        {
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, 0f);
            
            // Check for boarders
            if (target.position.x > MaxX)
                targetPosition.x = MaxX != 0 ? MaxX : targetPosition.x;
            if (target.position.x < MinX)
                targetPosition.x = MinX != 0 ? MinX : targetPosition.x;
            if (target.position.y > MaxY)
                targetPosition.y = MaxY != 0 ? MaxY : targetPosition.y;
            if (target.position.y < MinY)
                targetPosition.y = MinY != 0 ? MinY : targetPosition.y;

            // Check for offset 
            if (XOffset != 0)
                targetPosition.x += XOffset;
            if (YOffset != 0)
                targetPosition.y += YOffset;

            // Smooth following
            cameraTransform.position = Vector3.SmoothDamp(cameraTransform.position, targetPosition + new Vector3(0f, 0f, _zOffset), ref velocity, timeOffset);
        }
        #endregion
    }
}
