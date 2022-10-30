using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSegment
{
    private int tile;
    private int top;
    private int bottom;
    private int left;
    private int right;
    public TileSegment(int tile, int top, int bottom, int left, int right)
    {
        this.tile = tile;
        this.top = top;
        this.bottom = bottom;
        this.left = left;
        this.right = right;
    }

    public int GetTile(string direction)
    {
        switch (direction)
        {
            case "top": return top;
            case "bottom": return bottom;
            case "left": return left;
            case "right": return right;
            default: return tile;
        }
    }
}
