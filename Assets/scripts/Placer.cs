using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Placer : MonoBehaviour
{
    public Tilemap tileMap; // x=-9/9 y= -5/5
    public Tile path, active, wrong, goal;
    public float fade = 2f;

    public int sizeX = 18, sizeY = 10;
    private float toFade;
    private TileType activeTile;

    private struct KeyXY
    {
        public readonly int x;
        public readonly int y;
        public KeyXY(int p1, int p2)
        {
            x = p1;
            y = p2;
        }
        // Equals and GetHashCode ommitted
    }

    private Dictionary<KeyXY, TileType> map;   // A = active; P = Path; W = wrong; G = goal
    private Dictionary<KeyXY, TileType> mapHidden;

    public float ToFade { get => toFade; set => toFade = value; }

    void Awake()
    {
        map = new Dictionary<KeyXY, TileType>();
        mapHidden = new Dictionary<KeyXY, TileType>();
    }

    public void SetActive(int x, int y)
    {

        foreach (KeyValuePair<KeyXY, TileType> entry in map)
        {
            if (entry.Value.Type == "A")
            {
                Tile temp = null;
                TileType tempTypeHidden = null;

                if (mapHidden.TryGetValue(new KeyXY(entry.Value.X, entry.Value.Y), out tempTypeHidden))
                {
                    if (tempTypeHidden.Type == "P")
                    {
                        temp = path;
                        entry.Value.Type = "P";
                    }
                    else if(tempTypeHidden.Type == "G")
                    {
                        temp = goal;
                        entry.Value.Type = "G";
                    }
                }

                if (temp == null)
                {
                    temp = wrong;
                    entry.Value.Type = "W";
                }

                tileMap.SetTile(new Vector3Int(entry.Value.X, entry.Value.Y, 0), temp);
                break;
            }
        }

        tileMap.SetTile(new Vector3Int(x, y, 0), active);
        map[new KeyXY(x, y)] = new TileType("A", x, y);
        activeTile = new TileType("A", x, y);
    }

    public TileType GetAt(int x, int y)
    {
        TileType temp;

        if(map.TryGetValue(new KeyXY(x,y),out temp))
        {
            return temp;
        }
        return null;
    }

    public bool CheckAt(int x, int y)
    {
        TileType temp;

        if (mapHidden.TryGetValue(new KeyXY(x, y), out temp))
        {
            return (temp.Type == "P" || temp.Type == "G");
        }
        return false;
    }

    public TileType GetActive()
    {
        return activeTile;
    }

    public void PlacePathHiddenAt(int x, int y)
    {
        
        mapHidden.Add(new KeyXY(x, y), new TileType("P", x, y));

        //tileMap.SetTile(new Vector3Int(x, y, 0), path);
    }

    public void PalceTile(int x, int y, string type)
    {
        Tile temp = null;
        switch (type)
        {
            case "A":
                temp = active;
                break;
            case "G":
                temp = goal;
                mapHidden[new KeyXY(x, y)] = new TileType(type, x, y);
                break;
            case "P":
                temp = path;
                break;
            case "W":
                temp = wrong;
                break;
        }

        if(type=="")
        {
            map.Remove(new KeyXY(x, y));
        }
        else
        {
            map[new KeyXY(x, y)] = new TileType(type,x,y);
        }

        tileMap.SetTile(new Vector3Int(x, y, 0), temp);
    }

    public bool CheckBound(int x, int y)
    {
        if (x < -sizeX / 2 || x > sizeX / 2 - 1 || y < -sizeY / 2 || y > sizeY / 2 - 1)
        {
            return false;
        }
        return true;
    }

    private void Update()
    {
        toFade -= Time.deltaTime;

        if(toFade<=0&& toFade >= -2)
        {
            Hide();
        }
    }

    public void Show()
    {
        toFade = fade;

        foreach (KeyValuePair<KeyXY, TileType> entry in mapHidden)
        {
            if (!map.ContainsKey(new KeyXY(entry.Value.X, entry.Value.Y)))
            {
                tileMap.SetTile(new Vector3Int(entry.Value.X, entry.Value.Y, 0), path);
            }
        }
    }

    private void Hide()
    {
        foreach (KeyValuePair<KeyXY, TileType> entry in mapHidden)
        {
            if (!map.ContainsKey(new KeyXY(entry.Value.X, entry.Value.Y)))
            {
                tileMap.SetTile(new Vector3Int(entry.Value.X, entry.Value.Y, 0), null);
            }
        }
    }
}

public class TileType
{
    private string type;
    private int x, y;

    public TileType(string type, int x, int y)
    {
        this.Type = type;
        this.X = x;
        this.Y = y;
    }

    public string Type { get => type; set => type = value; }
    public int X { get => x; set => x = value; }
    public int Y { get => y; set => y = value; }
}
