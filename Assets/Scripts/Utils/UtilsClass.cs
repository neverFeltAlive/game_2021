using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

using Custom.Controlls;

namespace Custom.Utils
{
    /// <summary>
    /// 
    /// UtilsClass is used for containing multipurpose methods which are often used in the project
    /// :NeverFeltAlive
    /// 
    /// </summary>
    public static class UtilsClass
    /* DEBUG statements for this document 
     * 
     * Debug.Log("UtilsClass --> Start: ");
     * Debug.Log("<size=13><i><b> UtilsClass --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> UtilsClass --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> UtilsClass --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> UtilsClass --> </b></i><color=green> Function: </color></size>");
     * 
     */
    {
        /// <summary>
        /// Create text in a given world position
        /// </summary>
        /// <remarks>
        /// Instantiates a new object with Text Mesh component 
        /// and sets it up according to given params
        /// </remarks>
        public static TextMesh CreateWorldText(string text, 
            Color color, Transform parent, 
            Vector3 localPosition, int fontSize = 2,
            TextAnchor textAnchor = TextAnchor.MiddleCenter, 
            TextAlignment textAlignment = TextAlignment.Center,
            int sortingOrder = 0)
        {
            GameObject gameObject = new GameObject("Grid Text", typeof(TextMesh));
            Transform transform = gameObject.transform;

            transform.SetParent(parent, false);
            transform.localPosition = localPosition;

            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;

            return textMesh;
        }

        public static void DrawCross(Vector3 center, Color color, float time)
        {
            Debug.DrawLine(new Vector3(center.x - .01f, center.y - .01f),
                    new Vector3(center.x + .01f, center.y + .01f), color, time);
            Debug.DrawLine(new Vector3(center.x - .01f, center.y + .01f),
                new Vector3(center.x + .01f, center.y - .01f), color, time);
        }

        public static void StartCamShake(CinemachineVirtualCamera virtualCam, float intensity = .3f, float frequency = 1f)
        {
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
            cinemachineBasicMultiChannelPerlin.m_FrequencyGain = frequency;
        }

        public static void StopCamShake(CinemachineVirtualCamera virtualCam)
        {
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
            cinemachineBasicMultiChannelPerlin.m_FrequencyGain = 0;
        }
    }
}
