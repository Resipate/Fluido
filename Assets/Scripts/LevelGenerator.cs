using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject[] tileElements;
    private int horizontal, vertical;
    int[,] levelMap =
    {
        {1,2,2,2,2,2,2,2,2,2,2,2,2,7},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
        {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,3},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
        {2,5,3,4,4,3,5,3,3,5,3,4,4,4},
        {2,5,3,4,4,3,5,4,4,5,3,4,4,3},
        {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
        {1,2,2,2,2,1,5,4,3,4,4,3,0,4},
        {0,0,0,0,0,2,5,4,3,4,4,3,0,3},
        {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
        {0,0,0,0,0,2,5,4,4,0,3,4,4,0},
        {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
        {0,0,0,0,0,0,5,0,0,0,4,0,0,0},
    };

    private void Start()
    {
        int childCount = transform.childCount;
        if(childCount > 0)
        {
            for(int i = childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        //Finding bounds of array for any given map
        vertical = levelMap.GetLength(0) - 1;
        horizontal = levelMap.GetLength(1) - 1;


        //Generating top left quarter of map as initial tiles
        GameObject Quad1 = new GameObject();
        Quad1.transform.parent = this.transform;
        Quad1.name = "Quadrant  1";
        for (int x = 0; x <= horizontal; x++)
        {
            for (int y = 0; y <= vertical; y++)
            {
                SetTile(y, x, Quad1.transform);
            }
        }
        
        //Generating top right quarter of map by manipulating a copy of first
        GameObject Quad2 = new GameObject();
        Quad2.transform.parent = this.transform;
        Quad2.name = "Quadrant  2";
        childCount = Quad1.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform iChild = Quad1.transform.GetChild(i);
            GameObject newChild = Instantiate(iChild.gameObject, 
                new Vector3((2*horizontal)-iChild.localPosition.x+1, iChild.localPosition.y, iChild.localPosition.z), 
                iChild.localRotation, 
                Quad2.transform);
            newChild.name = newChild.transform.position.x + ":" + newChild.transform.position.y;
            newChild.transform.localScale = new Vector3(-1, -1, 1);
            newChild.transform.eulerAngles = new Vector3(iChild.transform.eulerAngles.x+180, iChild.transform.eulerAngles.y, iChild.transform.eulerAngles.z);
        }

        //Generating bottom left quarter of map by manipulating a copy of first
        GameObject Quad3 = new GameObject();
        Quad3.transform.parent = this.transform;
        Quad3.name = "Quadrant  3";
        for (int i = 0; i < childCount; i++)
        {
            Transform iChild = Quad1.transform.GetChild(i);
            if (iChild.transform.position.y != 0)
            {
                GameObject newChild = Instantiate(iChild.gameObject,
                    new Vector3(iChild.localPosition.x, -iChild.localPosition.y, iChild.localPosition.z),
                    iChild.localRotation,
                    Quad3.transform);
                newChild.name = newChild.transform.position.x + ":" + newChild.transform.position.y;
                newChild.transform.localScale = new Vector3(-1, -1, 1);
                newChild.transform.eulerAngles = new Vector3(iChild.transform.eulerAngles.x, iChild.transform.eulerAngles.y + 180, iChild.transform.eulerAngles.z);
            }
        }

        //Combining manipulations from Q2 & Q3 to produce the manipulations for Q4 in bottom right
        GameObject Quad4 = new GameObject();
        Quad4.transform.parent = this.transform;
        Quad4.name = "Quadrant  4";
        for (int i = 0; i < childCount; i++)
        {
            Transform iChild = Quad1.transform.GetChild(i);
            if (iChild.transform.position.y != 0)
            {
                GameObject newChild = Instantiate(iChild.gameObject,
                    new Vector3((2 * horizontal) - iChild.localPosition.x + 1, -iChild.localPosition.y, iChild.localPosition.z),
                    iChild.localRotation,
                    Quad4.transform);
                newChild.name = newChild.transform.position.x + ":" + newChild.transform.position.y;
                newChild.transform.localScale = new Vector3(1, 1, 1);
                newChild.transform.eulerAngles = new Vector3(iChild.transform.eulerAngles.x + 180, iChild.transform.eulerAngles.y + 180, iChild.transform.eulerAngles.z);
            }
        }
    }

    /*
     * SetTile is just a simple switch statement to draw out the value and assign it to the correct piece
     *      - int y is vertical coordinate
     *      - int x is horizontal coordinate
     *      - Transform parent is the parent object/folder for the sprite to be housed in (useful for grouping)
     */
    private void SetTile(int y, int x, Transform parent)
    {
        switch (levelMap[y, x])
        {
            case 1:
                Set1(y, x, parent);
                break;
            case 2:
                Set2(y, x, parent);
                break;
            case 3:
                Set3(y, x, parent);
                break;
            case 4:
                Set4(y, x, parent);
                break;
            case 5:
                Set5(y, x, parent);
                break;
            case 6:
                Set6(y, x, parent);
                break;
            case 7:
                Set7(y, x, parent);
                break;
            default:
                break;
        }
    }

    /*
     * A truely mind-bending experience
     * below are 7 sets of rules for each tile to determine the correct rotation and to instantiate them correctly
     *      - Generates tile first in correct position but no rotation
     *      - TileSegment tS is a small object that holds adjacent tile information  in a cross formation
     *                             top
     *                   left     value    right
     *                            bottom
     *      - Each set has several if statements for each of the allowed connecting sprites in order to determine if their
     *        rotated in such a way that the new piece can seemlessly connect
     *      - Because the map is being generated in the order of top -> bottom then left -> right, I could not access the rotations
     *        of the bottom piece and the right piece, so I had the rules solely depend on the top and left pieces to construct a
     *        reasonable map
     */
    private void Set1(int y, int x, Transform parent)
    {
        GameObject newTile = Instantiate(tileElements[0], new Vector3(x, vertical - y, 0), new Quaternion(0, 0, 0, 0), parent);
        newTile.name = x + ":" + y;
        TileSegment tS = GetTileSegment(x, y);
        bool top = false;
        bool left = false;
        if(tS.GetTile("top") == 1)
        {
            GameObject topObj = GameObject.Find(x + ":" + (y - 1));
            if(topObj.transform.eulerAngles.z == 0 || topObj.transform.eulerAngles.z == 270)
            {
                top = true;
            }
        }
        else if(tS.GetTile("top") == 2)
        {
            GameObject topObj = GameObject.Find(x + ":" + (y - 1));
            if (topObj.transform.eulerAngles.z == 0)
            {
                top = true;
            }
        }
        else if (tS.GetTile("top") == 7)
        {
            GameObject topObj = GameObject.Find(x + ":" + (y - 1));
            if (topObj.transform.eulerAngles.z == 0)
            {
                top = true;
            }
        }

        if (tS.GetTile("left") == 1)
        {
            GameObject leftObj = GameObject.Find((x - 1) + ":" + y);
            if (leftObj.transform.eulerAngles.z == 0 || leftObj.transform.eulerAngles.z == 90)
            {
                left = true;
            }
        }
        else if(tS.GetTile("left") == 2)
        {
            GameObject leftObj = GameObject.Find((x - 1) + ":" + y);
            if (leftObj.transform.eulerAngles.z == 90)
            {
                left = true;
            }
        }
        if (tS.GetTile("left") == 7)
        {
            GameObject leftObj = GameObject.Find((x - 1) + ":" + y);
            if (leftObj.transform.eulerAngles.z == 270)
            {
                left = true;
            }
        }

        if (top && left) { newTile.transform.eulerAngles = new Vector3(0, 0, 180); }
        else if (top) { newTile.transform.eulerAngles = new Vector3(0, 0, 90); }
        else if (left) { newTile.transform.eulerAngles = new Vector3(0, 0, 270); }
    }

    private void Set2(int y, int x, Transform parent)
    {
        GameObject newTile = Instantiate(tileElements[1], new Vector3(x, vertical - y, 0), new Quaternion(0, 0, 0, 0), parent);
        newTile.name = x + ":" + y;
        TileSegment tS = GetTileSegment(x, y);
        bool top = false;
        bool left = false;
        if (tS.GetTile("top") == 1)
        {
            GameObject topObj = GameObject.Find(x + ":" + (y - 1));
            if (topObj.transform.eulerAngles.z == 0 || topObj.transform.eulerAngles.z == 270)
            {
                top = true;
            }
        }
        else if(tS.GetTile("top") == 2)
        {
            GameObject topObj = GameObject.Find(x + ":" + (y - 1));
            if (topObj.transform.eulerAngles.z == 0)
            {
                top = true;
            }
        }
        else if (tS.GetTile("top") == 7)
        {
            GameObject topObj = GameObject.Find(x + ":" + (y - 1));
            if (topObj.transform.eulerAngles.z == 0)
            {
                top = true;
            }
        }

        if (tS.GetTile("left") == 1)
        {
            GameObject leftObj = GameObject.Find((x - 1) + ":" + y);
            if(leftObj.transform.eulerAngles.z == 0 || leftObj.transform.eulerAngles.z == 90)
            {
                left = true;
            }
        }
        if(tS.GetTile("left") == 2)
        {
            GameObject leftObj = GameObject.Find((x - 1) + ":" + y);
            if (leftObj.transform.eulerAngles.z == 90)
            {
                left = true;
            }
        }
        if (tS.GetTile("left") == 7)
        {
            GameObject leftObj = GameObject.Find((x - 1) + ":" + y);
            if (leftObj.transform.eulerAngles.z == 270)
            {
                left = true;
            }
        }

        if (!top || left) { newTile.transform.eulerAngles = new Vector3(0, 0, 90); }
    }

    private void Set3(int y, int x, Transform parent)
    {
        GameObject newTile = Instantiate(tileElements[2], new Vector3(x, vertical - y, 0), new Quaternion(0, 0, 0, 0), parent);
        newTile.name = x + ":" + y;
        TileSegment tS = GetTileSegment(x, y);
        bool top = false;
        bool left = false;
        if (tS.GetTile("top") == 3)
        {
            GameObject topObj = GameObject.Find(x + ":" + (y - 1));
            if (topObj.transform.eulerAngles.z == 0 || topObj.transform.eulerAngles.z == 270)
            {
                top = true;
            }
        }
        else if (tS.GetTile("top") == 4)
        {
            GameObject topObj = GameObject.Find(x + ":" + (y - 1));
            if (topObj.transform.eulerAngles.z == 0)
            {
                top = true;
            }
        }
        else if (tS.GetTile("top") == 7)
        {
            GameObject topObj = GameObject.Find(x + ":" + (y - 1));
            if (topObj.transform.eulerAngles.z == 270)
            {
                top = true;
            }
        }

        if (tS.GetTile("left") == 3)
        {
            GameObject leftObj = GameObject.Find((x - 1) + ":" + y);
            if (leftObj.transform.eulerAngles.z == 0 || leftObj.transform.eulerAngles.z == 90)
            {
                left = true;
            }
        }
        else if (tS.GetTile("left") == 4)
        {
            GameObject leftObj = GameObject.Find((x - 1) + ":" + y);
            if (leftObj.transform.eulerAngles.z == 90)
            {
                left = true;
            }
        }
        else if (tS.GetTile("left") == 7)
        {
            GameObject leftObj = GameObject.Find((x - 1) + ":" + y);
            if (leftObj.transform.eulerAngles.z == 180)
            {
                left = true;
            }
        }

        if (top && left) { newTile.transform.eulerAngles = new Vector3(0, 0, 180); }
        else if (top) { newTile.transform.eulerAngles = new Vector3(0, 0, 90); }
        else if (left) { newTile.transform.eulerAngles = new Vector3(0, 0, 270); }
    }

    private void Set4(int y, int x, Transform parent)
    {
        GameObject newTile = Instantiate(tileElements[3], new Vector3(x, vertical - y, 0), new Quaternion(0, 0, 0, 0), parent);
        newTile.name = x + ":" + y;
        TileSegment tS = GetTileSegment(x, y);
        bool top = false;
        bool left = false;
        if (tS.GetTile("top") == 3)
        {
            GameObject topObj = GameObject.Find(x + ":" + (y - 1));
            if (topObj.transform.eulerAngles.z == 0 || topObj.transform.eulerAngles.z == 270)
            {
                top = true;
            }
        }
        else if (tS.GetTile("top") == 4)
        {
            GameObject topObj = GameObject.Find(x + ":" + (y - 1));
            if (topObj.transform.eulerAngles.z == 0)
            {
                top = true;
            }
        }
        else if(tS.GetTile("top") == 7)
        {
            GameObject topObj = GameObject.Find(x + ":" + (y - 1));
            if (topObj.transform.eulerAngles.z == 270)
            {
                top = true;
            }
        }

        if (tS.GetTile("left") == 3)
        {
            GameObject leftObj = GameObject.Find((x - 1) + ":" + y);
            if (leftObj.transform.eulerAngles.z == 0 || leftObj.transform.eulerAngles.z == 90)
            {
                left = true;
            }
        }
        if (tS.GetTile("left") == 4)
        {
            GameObject leftObj = GameObject.Find((x - 1) + ":" + y);
            if (leftObj.transform.eulerAngles.z == 90)
            {
                left = true;
            }
        }
        if (tS.GetTile("left") == 7)
        {
            GameObject leftObj = GameObject.Find((x - 1) + ":" + y);
            if (leftObj.transform.eulerAngles.z == 180)
            {
                left = true;
            }
        }

        if (!top || left) { newTile.transform.eulerAngles = new Vector3(0, 0, 90); }
    }

    private void Set5(int y, int x, Transform parent)
    {
        GameObject newTile = Instantiate(tileElements[4], new Vector3(x, vertical - y, 0), new Quaternion(0, 0, 0, 0), parent);
        newTile.name = x + ":" + y;
    }

    private void Set6(int y, int x, Transform parent)
    {
        GameObject newTile = Instantiate(tileElements[5], new Vector3(x, vertical - y, 0), new Quaternion(0, 0, 0, 0), parent);
        newTile.name = x + ":" + y;
    }

    private void Set7(int y, int x, Transform parent)
    {
        GameObject newTile = Instantiate(tileElements[6], new Vector3(x, vertical - y, 0), new Quaternion(0, 0, 0, 0), parent);
        newTile.name = x + ":" + y;
        TileSegment tS = GetTileSegment(x, y);
        bool iTop = false;
        bool oTop = false;
        bool iLeft = false;
        bool oLeft = false;
        if(tS.GetTile("top") == 1)
        {
            GameObject topObj = GameObject.Find(x + ":" + (y - 1));
            if(topObj.transform.eulerAngles.z == 0 || topObj.transform.eulerAngles.z == 270)
            {
                oTop = true;
            }
        }
        else if(tS.GetTile("top") == 2)
        {
            GameObject topObj = GameObject.Find(x + ":" + (y - 1));
            if (topObj.transform.eulerAngles.z == 0)
            {
                oTop = true;
            }
        }
        else if(tS.GetTile("top") == 3)
        {
            GameObject topObj = GameObject.Find(x + ":" + (y - 1));
            if (topObj.transform.eulerAngles.z == 0 || topObj.transform.eulerAngles.z == 270)
            {
                iTop = true;
            }
        }
        else if (tS.GetTile("top") == 4)
        {
            GameObject topObj = GameObject.Find(x + ":" + (y - 1));
            if (topObj.transform.eulerAngles.z == 0)
            {
                iTop = true;
            }
        }
        else if(tS.GetTile("top") == 7)
        {
            GameObject topObj = GameObject.Find(x + ":" + (y - 1));
            if(topObj.transform.eulerAngles.z == 0)
            {
                oTop = true;
            }
            else if(topObj.transform.eulerAngles.z == 270)
            {
                iTop = true;
            }
        }

        if (tS.GetTile("left") == 1)
        {
            GameObject leftObj = GameObject.Find((x - 1) + ":" + y);
            if (leftObj.transform.eulerAngles.z == 180 || leftObj.transform.eulerAngles.z == 270)
            {
                oLeft = true;
            }
        }
        else if (tS.GetTile("left") == 2)
        {
            GameObject leftObj = GameObject.Find((x - 1) + ":" + y);
            if (leftObj.transform.eulerAngles.z == 90)
            {
                oLeft = true;
            }
        }
        else if (tS.GetTile("left") == 3)
        {
            GameObject leftObj = GameObject.Find((x - 1) + ":" + y);
            if (leftObj.transform.eulerAngles.z == 180 || leftObj.transform.eulerAngles.z == 270)
            {
                iLeft = true;
            }
        }
        else if (tS.GetTile("left") == 4)
        {
            GameObject leftObj = GameObject.Find((x - 1) + ":" + y);
            if (leftObj.transform.eulerAngles.z == 90)
            {
                iLeft = true;
            }
        }
        else if (tS.GetTile("left") == 7)
        {
            GameObject leftObj = GameObject.Find((x - 1) + ":" + y);
            if (leftObj.transform.eulerAngles.z == 270)
            {
                oLeft = true;
            }
            else if (leftObj.transform.eulerAngles.z == 180)
            {
                iLeft = true;
            }
        }

        if(oTop && iLeft) { newTile.transform.eulerAngles = new Vector3(0, 0, 180); }
        else if(iTop && !oLeft) { newTile.transform.eulerAngles = new Vector3(0, 0, 90); }
        else if(oLeft && !iTop) { newTile.transform.eulerAngles = new Vector3(0, 0, 270); }
    }

    private TileSegment GetTileSegment(int x, int y)
    {
        int top, bottom, left, right;
        if (x > 0)
        {
            left = levelMap[y, x - 1];
        }
        else
        {
            left = -1;
        }

        if (x < horizontal)
        {
            right = levelMap[y, x + 1];
        }
        else
        {
            right = -1;
        }

        if (y > 0)
        {
            top = levelMap[y - 1, x];
        }
        else
        {
            top = -1;
        }

        if (y < vertical)
        {
            bottom = levelMap[y + 1, x];
        }
        else
        {
            bottom = -1;
        }
        TileSegment tS = new TileSegment(levelMap[y, x], top, bottom, left, right);
        return tS;
    }

    public TileSegment GetSurroundingTiles(int x, int y)
    {
        bool xFlip = false;
        if(x > horizontal)
        {
            xFlip = true;
            x = -x + horizontal*2;
        }
        bool yFlip = false;
        if (y < 0)
        {
            yFlip = true;
            y = -y;
        }
        TileSegment tS = GetTileSegment(x, y);
        if (xFlip) { int temp = tS.GetTile("left"); tS.SetTile("left", tS.GetTile("right")); tS.SetTile("right", temp); }
        if (yFlip) { int temp = tS.GetTile("up"); tS.SetTile("up", tS.GetTile("down")); tS.SetTile("down", temp); }
        return tS;
    }
}
