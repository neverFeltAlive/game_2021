/// <remarks>
/// 
/// UtilsClass is used for containing multipurpose methods which are often used in the project
/// NeverFeltAlive
/// 
/// </remarks>


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.Utils
{
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
        // Create text in a given world position
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
        /// <summary>
        /// We instantiate a new object with Text Mesh component 
        /// and set it up according to given params
        /// </summary>
        

        public static void DrawCross(Vector3 center, Color color, float time)
        {
            Debug.DrawLine(new Vector3(center.x - .01f, center.y - .01f),
                    new Vector3(center.x + .01f, center.y + .01f), color, time);
            Debug.DrawLine(new Vector3(center.x - .01f, center.y + .01f),
                new Vector3(center.x + .01f, center.y - .01f), color, time);
        }
    }
}
