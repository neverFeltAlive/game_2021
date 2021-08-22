/// <remarks>
/// 
/// Grid is used for creating grid-like coordinate system.
/// This class is used in pathfinding logics of Enemy AI
/// NeverFeltAlive
/// 
/// </remarks>


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.Utils
{
    public class Grid<TGridObject>
    /* DEBUG statements for this document 
     * 
     * Debug.Log("Grid --> Start: ");
     * Debug.Log("<size=13><i><b> Grid --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> Grid --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> Grid --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> Grid --> </b></i><color=green> Function: </color></size>");
     * 
     */
    /* TODO
     * 
     */
    {
        public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
        public class OnGridObjectChangedEventArgs : EventArgs
        {
            public int x;
            public int y;
        }
        /// <remarks>
        /// We need this event to display changed values
        /// </remarks>



        private TGridObject[,] gridArray;

        public Vector3 originPosition;

        #region Properties
        public int Width { get; }
        public int Height { get; }
        public float CellSize { get; private set; }
        #endregion

        private bool showDebug = false;
        /// <remarks>
        /// Set to true if visual is needed
        /// </remarks>



        public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<int, int, Grid<TGridObject>, TGridObject> createGridObject)
        {
            this.Width = width;
            this.Height = height;
            this.CellSize = cellSize;

            this.originPosition = originPosition;

            gridArray = new TGridObject[width, height];

            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    gridArray[x, y] = createGridObject(x, y, this);
                }
            }

            if (showDebug)
            {
                TextMesh[,] debugArray = new TextMesh[width, height];

                for (int x = 0; x < gridArray.GetLength(0); x++)
                {
                    for (int y = 0; y < gridArray.GetLength(1); y++)
                    {
                        // Print the grid's verticles 
                        //debugArray[x, y] = UtilsClass.CreateWorldText(gridArray[x, y]?.ToString(), Color.white, null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f);

                        // Print the grid's lines
                        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.black, 100f);
                        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.black, 100f);
                    }
                }

                // Print the grid's boarder
                Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.black, 100f);
                Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.black, 100f);

                // Subscribe to event
                OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) =>
                {
                    debugArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y]?.ToString();
                };
            }

        }



        #region Public Functions
        // Set value to a certain grid sell 
        public void SetGridObject(int x, int y, TGridObject obj)
        {
            // Check for invalid values
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                gridArray[x, y] = obj;
                if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y});
            }
        }
        public void SetGridObject(Vector3 worldPosition, TGridObject obj)
        {
            int x, y;
            GetGridPosition(worldPosition, out x, out y);

            SetGridObject(x, y, obj);
        }

        // Trigger OnGridObjectChanged event from the outside of the script
        public void TriggerGridObjectChanged(int x, int y)
        {
            if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y});
        }

        // Converts world position coordinates into grid coordinates
        public void GetGridPosition(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt((worldPosition - originPosition).x / CellSize);
            y = Mathf.FloorToInt((worldPosition - originPosition).y / CellSize);
        }
        #endregion

        #region Methods
        // Converts grid coordinates into a world position
        public Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, y) * CellSize + originPosition;
        }

        // Returns the value of a grid node 
        public TGridObject GetGridObject(int x, int y)
        {
            // Check for invalid values
            if (x >= 0 && x < Width && y >= 0 && y < Height)
                return gridArray[x, y];
            else
                return default(TGridObject);

        }
        public TGridObject GetGridObject(Vector3 worldPosition)
        {
            int x, y;
            GetGridPosition(worldPosition, out x, out y);

            return GetGridObject(x, y);
        }
        #endregion
    }
}
