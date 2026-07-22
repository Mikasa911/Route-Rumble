using System.Collections.Generic;
using UnityEngine;
public class PathFinder
{
    public static List<Vector2> FindPath(Vector2 start, Vector2 target, LayerMask obstacles, LayerMask rockLayer, LayerMask interactables, LayerMask blankSpaceLayer, bool chestkey)
    {
        Node2D startNode = new Node2D(start);
        Node2D targetNode = new Node2D(target);
        startNode.parent = null;
        List<Node2D> openList = new List<Node2D>();
        List<Node2D> closedSet = new List<Node2D>();
        openList.Add(startNode);
        while(openList.Count>0)
        {
            Node2D currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].fCost < currentNode.fCost || openList[i].fCost == currentNode.fCost && openList[i].hCost < currentNode.hCost)
                {
                    currentNode = openList[i];
                }
            }
            openList.Remove(currentNode);
            closedSet.Add(currentNode);
            // Debug.Log(currentNode.parent.position);
            if (currentNode.position == targetNode.position)
            {
                return RetracePath(startNode, currentNode);
            }
            foreach (Node2D neighbor in GetNeighbors(currentNode, obstacles, rockLayer, interactables, blankSpaceLayer, chestkey))
            {
                if (closedSet.Contains(neighbor))
                {
                    continue;
                }
                float newCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newCostToNeighbor < neighbor.gCost || !openList.Contains(neighbor))
                {
                    neighbor.gCost = newCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;
                    neighbor.fCost = neighbor.gCost + neighbor.hCost;
                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }
            }           
        }
        return null; // No path found
    }
    static List<Vector2> RetracePath(Node2D startNode, Node2D endNode)
    {
        List<Node2D> path = new();
        List<Vector2> pathPos = new();
        while (startNode != endNode)
        {
            path.Add(endNode);
            endNode = endNode.parent;
        }
        pathPos.Add(startNode.position);
        path.Reverse();
        for (int i = 0; i < path.Count; i++)
        {
            pathPos.Add(path[i].position);
        }
        return pathPos;
    }
    static List<Node2D> GetNeighbors(Node2D node, LayerMask obstacles, LayerMask rockLayer, LayerMask interactables, LayerMask blankSpaceLayer, bool key)
    {
        List<Node2D> neighbors = new List<Node2D>();
        Vector2[] directions = new Vector2[]
        {
            new Vector2(0, 1),
            new Vector2(1, 0),
            new Vector2(0, -1),
            new Vector2(-1, 0)
        };
        foreach (Vector2 direction in directions)
        {
            Vector2 neighborPosition = node.position + direction;
            List<Collider2D> colliders = new(Physics2D.OverlapCircleAll(new Vector2(neighborPosition.x, neighborPosition.y), 0.1f, obstacles));
            if (!key)
            {
                colliders.AddRange(Physics2D.OverlapCircleAll(new Vector2(neighborPosition.x, neighborPosition.y), 0.1f, interactables));
            }
            if (Physics2D.OverlapCircle(new Vector2(neighborPosition.x, neighborPosition.y), 0.1f, blankSpaceLayer))
            {
                if (!(Physics2D.OverlapCircle(new Vector2(neighborPosition.x, neighborPosition.y), 0.1f, rockLayer)))
                {
                    colliders.AddRange(Physics2D.OverlapCircleAll(new Vector2(neighborPosition.x, neighborPosition.y), 0.1f, blankSpaceLayer));
                }
                else if((Physics2D.OverlapCircleAll(new Vector2(neighborPosition.x, neighborPosition.y), 0.1f, rockLayer)).Length>1)
                {
                    colliders.AddRange(Physics2D.OverlapCircleAll(new Vector2(neighborPosition.x, neighborPosition.y), 0.1f, rockLayer));
                }
            }
            else
            {
                colliders.AddRange(Physics2D.OverlapCircleAll(new Vector2(neighborPosition.x, neighborPosition.y), 0.1f, rockLayer));
            }
            bool hasObstacle = false;
            foreach (var collider in colliders)
            {
                hasObstacle = true;
                break;
            }
            if (!hasObstacle)
            {
                neighbors.Add(new Node2D(neighborPosition));
            }              
        }
        return neighbors;
    }
    static float GetDistance(Node2D nodeA, Node2D nodeB)
    {
        float dstX = Mathf.Abs(nodeA.position.x - nodeB.position.x);
        float dstY = Mathf.Abs(nodeA.position.y - nodeB.position.y);
        return dstX + dstY;
    }
    class Node2D
    {
        public Vector2 position;
        public float gCost;
        public float hCost;
        public Node2D parent;
        public float fCost;
        public Node2D(Vector2 position)
        {
            this.position = position;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            Node2D otherNode = (Node2D)obj;
            return position == otherNode.position;
        }
        public override int GetHashCode()
        {
            return position.GetHashCode();
        }
    }
}
