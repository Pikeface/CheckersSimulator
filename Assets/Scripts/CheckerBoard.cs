using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;


[XmlRoot("CheckerBoardData")]

public class CheckerBoardData
{
    public PieceData[] pieces;

    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(CheckerBoardData));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static CheckerBoardData Load(string path)
    {
        var serializer = new XmlSerializer(typeof(CheckerBoardData));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as CheckerBoardData;

        }
    }

}

public class CheckerBoard : MonoBehaviour
{
    public string fileName;
    public GameObject blackPiece;
    public GameObject whitePiece;

    public int boardX = 8, boardZ = 8;
    public float pieceRadius = 0.5f;

    public Piece[,] pieces;

    private int halfBoardX, halfBoardZ;
    private float pieceDiameter;
    private Vector3 bottomLeft;
    private CheckerBoardData data;




    // Use this for initialization
    void Start()
    {
        // calculate a few values
        halfBoardX = boardX / 2;
        halfBoardZ = boardZ / 2;
        pieceDiameter = pieceRadius * 2;
        bottomLeft = transform.position - Vector3.right * halfBoardX - Vector3.forward * halfBoardZ;

        string path = Application.persistentDataPath + "/" + fileName;
        // string path = Application.persistentDataPath + fileName + "/" + fileName;
        //data = CheckerBoardData.Load(path);
        
        CreateGrid();
        data = new CheckerBoardData();
        data.Save(path);        
    }

    void CreateGrid()
    {
        // initialise 2D array 
        pieces = new Piece[boardX, boardZ];

        #region Generate white pieces 
        // loop trhrough board columns and skip 2 each time 
        for (int x = 0; x < boardX; x += 2)
        {
            // loop through first 3 rows
            for (int z = 0; z < 3; z++)
            {
                // check even row - if its not even must be odd
                bool evenRow = z % 2 == 0;
                // generate piece 
                int gridX = evenRow ? x : x + 1;
                int gridZ = z;
                // generate piece
                GeneratePiece(whitePiece, gridX, gridZ);
            }

        }
        #endregion

        #region Generate Black Pieces
        for (int x = 0; x < boardX; x += 2)
        {
            // loop through first 3 rows
            for (int z = 5; z < 8; z++)
            {
                // check even row - if its not even must be odd
                bool evenRow = z % 2 == 0;
                // generate piece 
                int gridX = evenRow ? x : x + 1;
                int gridZ = z;
                // generate piece
                GeneratePiece(blackPiece, gridX, gridZ);

            }
        }
        #endregion

    }

    void GeneratePiece(GameObject piecePrefab, int x, int z)
    {

        // Crate instance of piece
        GameObject clone = Instantiate(piecePrefab);
        // set the parent to be this transform 
        clone.transform.SetParent(transform);
        // get the piece from the clone 
        Piece piece = clone.GetComponent<Piece>();
        // place the piece
        PlacePiece(piece, x, z);

    }

    void PlacePiece(Piece piece, int x, int z)
    {
        float xOffset = x * pieceDiameter + pieceRadius;
        float zOffset = z * pieceDiameter + pieceRadius;
        // set pieces new grid coordinates 
        piece.gridX = x;
        piece.gridZ = z;
        // move physical to board coordinates
        piece.transform.position = bottomLeft + Vector3.right * xOffset + Vector3.forward * zOffset;
        // set pieces array slot
        pieces[x, z] = piece;

    }

    public void PlacePiece(Piece piece, Vector3 position)
    {
        // translate position to coordinate array 
        float percentX = (position.x + halfBoardX) / boardX;
        float percentZ = (position.z + halfBoardZ) / boardZ;

        percentX = Mathf.Clamp01(percentX);
        percentZ = Mathf.Clamp01(percentZ);

        int x = Mathf.RoundToInt((boardX - 1) * percentX);
        int z = Mathf.RoundToInt((boardZ - 1) * percentZ);

        // get old piece from that coordinate
        Piece oldPiece = pieces[x, z];

        // if there is an old piece in slot currently then swap pieces 
        if (oldPiece != null)
        {
            // swap pieces 
            SwapPieces(piece, oldPiece);
        }
        else
        {
            // place the piece 
            int oldX = piece.gridX;
            int oldZ = piece.gridZ;
            // set old position to null 
            pieces[oldX, oldZ] = null;
            // set new position 
            PlacePiece(piece, x, z);

        }



    }
    void SwapPieces(Piece pieceA, Piece pieceB)
    {
        // check if pieceA OR pieceB is nuyll 
        // return 
        if (pieceA == null || pieceB == null)
            return; // exit the function

        // PieceA grid pos 
        int pAX = pieceA.gridX;
        int pAZ = pieceA.gridZ;

        // PieceB grid pos 
        int pBX = pieceB.gridX;
        int pBZ = pieceB.gridZ;


        // Swap pieces 
        PlacePiece(pieceA, pBX, pBZ);
        PlacePiece(pieceB, pAX, pAZ);




    }
}


