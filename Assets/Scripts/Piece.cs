using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceData
{
    public int gridX, gridZ;
    public PieceData()
    {
        gridX = 0;
        gridZ = 0;
    }
    public PieceData(int x, int z)
    {
        gridX = x;
        gridZ = z;
    }
}


public class Piece : MonoBehaviour
{
    public PieceData pieceData = new PieceData();
    private int x, z;
    public int gridX
    {
        get
        {
            return x;

        }
        set
        {
            pieceData.gridX = value;
            x = value;
        }
    }
    public int gridZ
    {
        get
        {
            return z;

        }
        set
        {
            pieceData.gridZ = value;
            z = value;
        }
    }

}
