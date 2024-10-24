using UnityEngine;

public class S_Node
{
    public Vector3Int Position; // Position du node
    public float GCost; // Coût du chemin depuis le départ
    public float HCost; // Coût estimé jusqu'à l'arrivée
    public float FCost => GCost + HCost; // Coût total
    public S_Node Parent; // Node parent pour retracer le chemin

    public S_Node(Vector3Int _position)
    {
        Position = _position;
    }
}
