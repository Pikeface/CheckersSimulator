using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckerBoard : MonoBehaviour
{

    public GameObject blackPiece;
    public GameObject whitePiece;

    public int boardX = 8, boardZ = 8;
    public float pieceRadius = 0.5f;

    public Piece[,] pieces;

    private int halfBoardX, halfBoradZ;
    private float pieceDiameter;
    private Vector3 bottomLeft;



    // Use this for initialization
    void Start()
    {
        // calculate a few values
        halfBoardX = boardX / 2;
        halfBoradZ = boardZ / 2;
        pieceDiameter = pieceRadius * 2;
        bottomLeft = transform.position - Vector3.right * halfBoardX - Vector3.forward * halfBoradZ;

        CreateGrid();

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
        // move physicall to board coordinates
        piece.transform.position = bottomLeft + Vector3.right * xOffset + Vector3.forward * zOffset;
        // set pieces array slot
        pieces[x, z] = piece;

    }


}
