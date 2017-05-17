using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GGL;


public class PiecePicker : MonoBehaviour
{

    public float pieceHeight = 3f;
    public float rayDistance = 100f;
    public LayerMask selectionIgnoreLayer;

    private Piece selectedPiece;
    private CheckerBoard board;
    private Vector3 hitPoint;


    // Use this for initialization
    void Start()
    {
        // find the checkerboard in the scene
        board = FindObjectOfType<CheckerBoard>();

    }


    void FixedUpdate()
    {

        CheckSelection();
        MoveSelection();

    }

    void MoveSelection()
    {
        // check if we have a piece seleeted
        // if not = 0 we have one selected
        if (selectedPiece != null)
        {
            // create new ray from camera 
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            GizmosGL.AddLine(ray.origin, ray.origin + ray.direction * rayDistance, 0.1f, 0.1f, Color.yellow, Color.yellow);

            RaycastHit hit;
            // Raycast to only hit objects that arent pieces 
            if (Physics.Raycast(ray, out hit, rayDistance, ~selectionIgnoreLayer))

            {
                
                // GizmosGL.color = Color.red;
                GizmosGL.AddSphere(hit.point, 0.5f);
                // move the piece to position
                Vector3 piecePos = hit.point + Vector3.up * pieceHeight;

                selectedPiece.transform.position = piecePos;

                // obtain hit point
                hitPoint = hit.point;
                
            }
            // check if mouse button was released 
            if (Input.GetMouseButtonUp(0))
            {
                // Move piece to hit point 
                Piece piece = selectedPiece.GetComponent<Piece>();
                board.PlacePiece(piece, hitPoint);


                // deselect piece 
                selectedPiece = null;

            }
        }
    }



    void CheckSelection()
    {
        // if there is a lready a selected piece
        if (selectedPiece != null)
            return; // Exit the function


        // create a ray from camera mouse position to world
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        GizmosGL.AddLine(ray.origin, ray.origin + ray.direction * rayDistance);

        RaycastHit hit;
        // Check if the player hits the mouse button
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray 
            if (Physics.Raycast(ray, out hit, rayDistance))
            {
                // set the selected piece to be the hit object
                selectedPiece = hit.collider.GetComponent<Piece>();
                // check if the user did not hit a piece
                if (selectedPiece == null)
                {
                    // display message 
                    Debug.Log("cannot pick up Object: " + hit.collider.name);
                }

            }
        }
    }
}
