using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSegment
{
    public int tile;
    public int top;
    public int bottom;
    public int left;
    public int right;
    public TileSegment(int tile, int top, int bottom, int left, int right)
    {
        this.tile = tile;
        this.top = top;
        this.bottom = bottom;
        this.left = left;
        this.right = right;
    }
}
