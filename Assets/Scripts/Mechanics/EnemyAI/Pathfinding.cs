/// <remarks>
/// 
/// Pathfinding is used for detecting the best path to the destination.
/// It immplements A * pathfinding algorythms
/// NeverFeltAlive
/// 
/// </remarks>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Platformer.Utils;

namespace Platformer.Mechanics.EnemyAI
{
    public class Pathfinding : MonoBehaviour
    /* DEBUG statements for this document 
     * 
     * Debug.Log("Pathfinding --> Start: ");
     * Debug.Log("<size=13><i><b> Pathfinding --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> Pathfinding --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> Pathfinding --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> Pathfinding --> </b></i><color=green> Function: </color></size>");
     * 
     */
    {
        #region Private Fields
        private const int STRAIGHT_COST = 10;
        private const int DIAGONAL_COST = 14;

        private Grid<PathNode> grid;

        private List<PathNode> openList;
        private List<PathNode> closedList;
        #endregion

        public static Pathfinding Instance { get; private set; }




        public Pathfinding(int gridWidth, int gridHeight, Vector3 gridOriginPosition, float gridCellsize = .5f)
        {
            Instance = this;

            // Create a grid of path nodes
            grid = new Grid<PathNode>(gridWidth, gridHeight, gridCellsize, gridOriginPosition, (int x, int y, Grid<PathNode> g) => new PathNode(x, y, g));
        }



        #region Public Functions
        public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition)
        {
            grid.GetGridPosition(startWorldPosition, out int startX, out int startY);
            grid.GetGridPosition(endWorldPosition, out int endX, out int endY);

            List<PathNode> path = FindPath(startX, startY, endX, endY);

            if (path == null)
                return null;
            else
            {
                List<Vector3> vectorPath = new List<Vector3>();

                foreach (PathNode node in path)
                {

                    vectorPath.Add(grid.GetWorldPosition(node.x, node.y));
                    Debug.Log("<size=13><i><b> Pathfinding --> </b></i><color=green> FindPath: </color></size> " + node + "     " +
                        (grid.GetWorldPosition(node.x, node.y)));
                }
                /// <summary>
                /// To convert grid coordinates to vectors we multimply them by grid cell size and offset by origin position of the grid
                /// </summary>
                
                return vectorPath;
            }
        }
        public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
        {
            PathNode startNode = GetNode(startX, startY);
            PathNode endNode = GetNode(endX, endY);

            openList = new List<PathNode> { startNode };
            closedList = new List<PathNode>();

            // Initialize all nodes and add them to the array
            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    PathNode pathNode = GetNode(x, y);

                    pathNode.gCost = int.MaxValue;
                    pathNode.hCost = CalculateHCost(pathNode, endNode);
                    pathNode.CalculateFCost();
                    /// <remarks>
                    /// We assign gCost to int max value to avoid extra conditions
                    /// as firther in the algorythm we change gCost for smaller values
                    /// </remarks>
                    
                    pathNode.cameFromNode = null;
                }
            }

            startNode.gCost = 0;
            startNode.hCost = CalculateHCost(startNode, endNode);
            startNode.CalculateFCost();

            // Check all nodes
            while (openList.Count > 0)
            {
                PathNode currentNode = GetLowestFCostNode(openList);

                // Check if current node is the final node
                if (currentNode == endNode)
                    return CalculatePath(currentNode);

                // Remove the current node from the list
                openList.Remove(currentNode);
                closedList.Add(currentNode);

                // Check all neighbours and update their costs
                foreach (PathNode neighbourNode in GetNeighboursList(currentNode))
                {
                    // Check if neighbour is already checked
                    if (closedList.Contains(neighbourNode))
                        continue;

                    // Check if neighbouring node is walkable
                    if (!neighbourNode.isWalkable)
                    {
                        closedList.Add(neighbourNode);
                        continue;
                    }

                    // Check if node has to be updated to lower costs
                    int tentativeGCost = currentNode.gCost + CalculateHCost(currentNode, neighbourNode);
                    if (tentativeGCost < neighbourNode.gCost)
                    {
                        neighbourNode.cameFromNode = currentNode;
                        neighbourNode.gCost = tentativeGCost;
                        neighbourNode.hCost = CalculateHCost(neighbourNode, endNode);
                        neighbourNode.CalculateFCost();


                        // Add a to open list for further check
                        if (!openList.Contains(neighbourNode))
                            openList.Add(neighbourNode);
                    }
                }
            }
            /// <summary>
            /// We check every node starting with the first node and add all its neighbours to the open list.
            /// After that we check the nodes with the lowest f cost the simmilar way
            /// </summary>

            return null;
        }
        #endregion

        #region Methods
        // Calculates the h cost of a node by finding the shortest distnce and multiplying it with cost
        private int CalculateHCost(PathNode currentNode, PathNode endNode)
        {
            int xDistance = Mathf.Abs(currentNode.x - endNode.x);
            int yDistance = Mathf.Abs(currentNode.y - endNode.y);

            int straigntDistance = Mathf.Abs(xDistance - yDistance);

            return DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + STRAIGHT_COST * straigntDistance;
        }
        /// <summary>
        /// This function finds the shortest possible path by moving streight as much as possible before moving diagonally
        /// </summary>

        // Finds the node with the lowest fCost and returns its index
        private PathNode GetLowestFCostNode(List<PathNode> pathNodesList)
        {
            PathNode lowestFCostNode = pathNodesList[0];

            for (int i = 0; i < pathNodesList.Count; i++)
            {
                if (pathNodesList[i].fCost < lowestFCostNode.fCost)
                    lowestFCostNode = pathNodesList[i];
            }

            return lowestFCostNode;
        }

        // Calculates the path from path nodes array using came from index
        private List<PathNode> CalculatePath(PathNode endNode)
        {
            List<PathNode> path = new List<PathNode> { endNode };

            // Add all nodes with came from index to the path
            PathNode currentNode = endNode;
            while (currentNode.cameFromNode != null)
            {
                path.Add(currentNode.cameFromNode);
                currentNode = currentNode.cameFromNode;
            }

            path.Reverse();
            return path;
        }

        // Checks all neighbouring nodes and validate them
        private List<PathNode> GetNeighboursList(PathNode currendNode)
        {
            List<PathNode> neighboursList = new List<PathNode>();

            if (currendNode.x - 1 >= 0)
            {
                neighboursList.Add(GetNode(currendNode.x - 1, currendNode.y));                                              // left
                if (currendNode.y - 1 >= 0) neighboursList.Add(GetNode(currendNode.x - 1, currendNode.y - 1));              // left bottom
                if (currendNode.y + 1 < grid.Height) neighboursList.Add(GetNode(currendNode.x - 1, currendNode.y + 1));     // left top
            }
            if (currendNode.x + 1 < grid.Width)
            {
                neighboursList.Add(GetNode(currendNode.x + 1, currendNode.y));                                              // right
                if (currendNode.y - 1 >= 0) neighboursList.Add(GetNode(currendNode.x + 1, currendNode.y - 1));              // left bottom
                if (currendNode.y + 1 < grid.Height) neighboursList.Add(GetNode(currendNode.x + 1, currendNode.y + 1));     // left top
            }
            if (currendNode.y - 1 >= 0) neighboursList.Add(GetNode(currendNode.x, currendNode.y - 1));                      // bottom
            if (currendNode.y + 1 < grid.Height) neighboursList.Add(GetNode(currendNode.x, currendNode.y + 1));             // top

            return neighboursList;
        }

        private PathNode GetNode(int x, int y)
        {
            return grid.GetGridObject(x, y);
        }
        #endregion
    }

    public class PathNode
    {
        private Grid<PathNode> grid;

        #region Pubic Fields
        // Position in grid
        public int x;
        public int y;

        // A * pathfinding values
        public int gCost;                           // distance from the start node to the current node
        public int hCost;                           // estimate distance from the current node to the final node
        public int fCost;                           // hCost + gCost

        public bool isWalkable;

        public PathNode cameFromNode;               // the previous node on the path (node we come from to current node)
        #endregion



        public PathNode(int x, int y, Grid<PathNode> grid)
        {
            this.x = x;
            this.y = y;
            this.grid = grid;

            isWalkable = true;
        }



        #region Public Functions
        public override string ToString()
        {
            return x + "," + y;
        }
        public void CalculateFCost() =>
                    this.fCost = this.hCost + this.gCost;
        #endregion
    }
    /// <summary>
    /// This class contains all the neccessary fields for the A* pathfinding algorythm
    /// </summary>
}
