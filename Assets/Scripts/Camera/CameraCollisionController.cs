/// <remarks>
/// 
/// CameraCollisionController is used for passing new variables to camera controller
/// NeverFeltAlive
/// 
/// </remarks>


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Platformer.Utils;

namespace Platformer.Camera
{
    public class CameraCollisionController : MonoBehaviour
    /* DEBUG statements for this document 
     * 
     * Debug.Log("CameraCollisionController --> Start: ");
     * Debug.Log("<size=13><i><b> CameraCollisionController --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> CameraCollisionController --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> CameraCollisionController --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> CameraCollisionController --> </b></i><color=green> Function: </color></size>");
     * 
     */
    /* TODO
     * 
     */
    {
        // Fields

        #region Serialized Fields
        [Header("Components")]
        [Space]

        // Variables
        [Header("Variables")]
        [SerializeField] [Tooltip("Right boarder x value")] private float maxX = 0f;
        [SerializeField] [Tooltip("Left boarder x value")] private float minX = 0f;
        [SerializeField] [Tooltip("Right boarder y value")] private float maxY = 0f;
        [SerializeField] [Tooltip("Left boarder y value")] private float minY = 0f;
        [Space]
        [SerializeField] [Range(-30f, 30f)] [Tooltip("Offset value for x")] private float xOffset = 0f;
        [SerializeField] [Range(-30f, 30f)] [Tooltip("Offset value for y")] private float yOffset = 0f;
        #endregion

        #region Private Fields
        private float savedXOffset = 0f;
        private float savedYOffset = 0f;
        #endregion

        // Functions 

        #region Event Handlers
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.name == Constants.CHARACTER_NAME)
            {
                CameraController camera = collision.GetComponent<CameraController>();

                // Changing boarders if needed
                if (maxX != 0)
                    camera.MaxX = maxX;
                if (minX != 0)
                    camera.MinX = minX;
                if (maxY != 0)
                    camera.MaxY = maxY;
                if (minY != 0)
                    camera.MinY = minY;

                // Change offset if needed
                if (xOffset != 0)
                {
                    savedXOffset = camera.XOffset;

                    // Make sure only works one way
                    if (xOffset > 0 && gameObject.transform.position.x > collision.transform.position.x)
                        camera.XOffset = xOffset;
                    if (xOffset < 0 && gameObject.transform.position.x < collision.transform.position.x)
                        camera.XOffset = xOffset;
                }
                if (yOffset != 0)
                {
                    savedYOffset = camera.XOffset;

                    // Make sure only works one way
                    if (yOffset > 0 && gameObject.transform.position.y > collision.transform.position.y)
                        camera.YOffset = yOffset;

                    if (yOffset < 0 && gameObject.transform.position.y < collision.transform.position.y)
                        camera.YOffset = yOffset;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.name == Constants.CHARACTER_NAME)
            {
                CameraController camera = collision.GetComponent<CameraController>();

                // Restore offset
                camera.XOffset = savedXOffset;
                camera.YOffset = savedYOffset;
            }
        }
        #endregion
    }
}
