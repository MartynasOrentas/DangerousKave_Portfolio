using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
    static private TilemapCollider2D _tilemap2d;

    private void Awake()
    {
        Transform tilemap = transform.Find("Background/Grid/Tilemap");
        _tilemap2d = tilemap.gameObject.GetComponent<TilemapCollider2D>();
    }

    public static bool IsOverlapWithTilemap(Vector2 p)
    {
        if(_tilemap2d != null)
            return _tilemap2d.OverlapPoint(p);

        return false;
    }
}
