using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [Header("Assignables")]
    public Tilemap tilemap;
    public Tile grassTile; 
    public Tile dirtTile; 
    public Tile waterTile; 
    public Tile sandTile; 
    public Tile mountainTile; 
    [Space(10)]
    [Header("Map Settings")]
    public int width = 50; 
    public int height = 50;
    public float scale = 10f; 
    [Space(10)]
    [Header("Biome Thresholds")]
    [Range(0, 1)] public float grassThreshold = 0.5f; 
    [Range(0, 1)] public float dirtThreshold = 0.4f; 
    [Range(0, 1)] public float waterThreshold = 0.3f; 
    [Range(0, 1)] public float sandThreshold = 0.2f; 
    [Range(0, 1)] public float mountainThreshold = 0.7f; 
    [Space(10)]
    [Header("Perlin Noise Settings")]
    public int octaves = 1; 
    public float persistence = 0.5f; 
    public float lacunarity = 2.0f;
    public float frequency = 1.0f;
    public float amplitude = 1.0f;
    public float seed = 1.0f;
    
    void OnValidate()
    {
        GenerateMap();
        S_AStar aStar = new S_AStar(tilemap);
        
    }

    void GenerateMap()
    {
        tilemap.ClearAllTiles();
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float sample = GeneratePerlinNoise(x, y);
                
                
                if (sample > mountainThreshold)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), mountainTile);
                }
                else if (sample > grassThreshold)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), grassTile);
                }
                else if (sample > dirtThreshold)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), dirtTile);
                }
                else if (sample > sandThreshold)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), sandTile);
                }
                else if (sample > waterThreshold)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), waterTile);
                }
            }
        }
    }

    float GeneratePerlinNoise(int x, int y)
    {
        float total = 0;
        float _frequency = frequency;
        float _amplitude = amplitude;
        float maxValue = 0; 
        float step = 1 / scale;
        
        Vector3 worldPosition = new Vector3(x * step + transform.position.x + seed, 0, y * step + transform.position.z);
        
        for (int i = 0; i < octaves; i++)
        {
            float xCoord = (float)x / scale * _frequency + seed;
            float yCoord = (float)y / scale * _frequency + seed;
            total += Mathf.PerlinNoise(worldPosition.x * _frequency + seed, worldPosition.z * _frequency) * amplitude;

            maxValue += _amplitude;

            _amplitude *= persistence;
            _frequency /= lacunarity;
        }

        return total / maxValue; 
    }
}