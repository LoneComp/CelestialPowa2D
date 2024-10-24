using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class S_AStar
{
    private Tilemap tilemap;
    private List<S_Node> openSet;
    private HashSet<S_Node> closedSet;

    public S_AStar(Tilemap tilemap)
    {
        this.tilemap = tilemap;
    }

    public List<Vector3Int> FindPath(Vector3Int start, Vector3Int end)
    {
        openSet = new List<S_Node>();
        closedSet = new HashSet<S_Node>();

        S_Node startNode = new S_Node(start);
        S_Node endNode = new S_Node(end);
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            S_Node currentNode = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentNode.FCost || 
                    (openSet[i].FCost == currentNode.FCost && openSet[i].HCost < currentNode.HCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);
            
            if (currentNode.Position == endNode.Position) // Arrived at destination
            {
                return RetracePath(startNode, currentNode);
            }

            foreach (S_Node neighbor in GetNeighbors(currentNode))
            {
                if (closedSet.Contains(neighbor)) continue;

                float newCostToNeighbor = currentNode.GCost + GetDistance(currentNode, neighbor);
                if (newCostToNeighbor < neighbor.GCost || !openSet.Contains(neighbor))
                {
                    neighbor.GCost = newCostToNeighbor;
                    neighbor.HCost = GetDistance(neighbor, endNode);
                    neighbor.Parent = currentNode;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        return null; 
    }

    private List<S_Node> GetNeighbors(S_Node node)
    {
        List<S_Node> neighbors = new List<S_Node>();

        Vector3Int[] directions = {
            new Vector3Int(1, 0, 0),  // Droite
            new Vector3Int(-1, 0, 0), // Gauche
            new Vector3Int(0, 1, 0),  // Haut
            new Vector3Int(0, -1, 0)  // Bas
        };

        foreach (var direction in directions)
        {
            Vector3Int neighborPosition = node.Position + direction;
            
            if (IsWalkable(neighborPosition))
            {
                neighbors.Add(new S_Node(neighborPosition));
            }
        }

        return neighbors;
    }

    private bool IsWalkable(Vector3Int position)
    {
        TileBase tile = tilemap.GetTile(position);
        return tile != null && tile != tilemap.GetTile(new Vector3Int(0, 0, 0)); 
    }

    private float GetDistance(S_Node a, S_Node b)
    {
        int dstX = Mathf.Abs(a.Position.x - b.Position.x);
        int dstY = Mathf.Abs(a.Position.y - b.Position.y);
        return dstX + dstY; 
    }

    private List<Vector3Int> RetracePath(S_Node startNode, S_Node endNode)
    {
        List<Vector3Int> path = new List<Vector3Int>();
        S_Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode.Position);
            currentNode = currentNode.Parent;
        }

        path.Reverse();
        return path;
    }
}
