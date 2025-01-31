using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzlePoint : MonoBehaviour
{
    public PuzzleMainController PuzzleMainController;

    public bool EndMove = false;
    bool canMove;
    bool dragging;
    [SerializeField] private Collider2D collider;
    [SerializeField] private SpriteRenderer SpriteRenderer;

    [SerializeField] private float doubleClickTime = .2f; // Временной порог для двойного нажатия
    [SerializeField] private float lastClickTime; // Время последнего нажатия    
    [SerializeField] private float size; // Время последнего нажатия    
    void Start()
    {
        canMove = false;
        dragging = false;

        this.transform.rotation = PuzzleMainController.RotateStart.rotation;

        this.transform.localScale = new Vector2( PuzzleMainController.RotateStart.localScale.x* size, PuzzleMainController.RotateStart.localScale.y* size);

        this.transform.position = PuzzleMainController.RotateStart.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (dragging == false && EndMove == false)
        {
            if (collision.gameObject.name == gameObject.name)
            {
                EndPuzzle(collision);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (dragging == false && EndMove == false)
        {
            if (collision.gameObject.name == gameObject.name)
            {
                EndPuzzle(collision);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (dragging == false && EndMove == false)
        {
            if (collision.gameObject.name == gameObject.name)
            {
                EndPuzzle(collision);
            }
        }
    }


    void Update()
    {
        if (EndMove == false)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                if (collider == Physics2D.OverlapPoint(mousePos))
                {
                    canMove = true;
                }
                else
                {
                    canMove = false;
                }
                if (canMove)
                {
                    dragging = true;

                    this.transform.rotation = PuzzleMainController.RotateDrag.rotation;
                    this.transform.localScale = PuzzleMainController.RotateDrag.localScale;
                    this.transform.localScale = new Vector2(PuzzleMainController.RotateDrag.localScale.x * size, PuzzleMainController.RotateDrag.localScale.y * size);
                }
            }
            if (dragging)
            {
                this.transform.position = mousePos;
            }
            if (Input.GetMouseButtonUp(0))
            {
                canMove = false;
                dragging = false;
                this.transform.position = PuzzleMainController.RotateStart.position;
                this.transform.rotation = PuzzleMainController.RotateStart.rotation;
                this.transform.localScale = new Vector2(PuzzleMainController.RotateStart.localScale.x * size, PuzzleMainController.RotateStart.localScale.y * size);
            }
        }
    }

    private void EndPuzzle(Collider2D collision) {
        transform.position = collision.transform.position;

        this.transform.rotation = PuzzleMainController.RotateDrag.rotation;
        this.transform.localScale = new Vector2(PuzzleMainController.RotateDrag.localScale.x * size, PuzzleMainController.RotateDrag.localScale.y * size);

        EndMove = true;
        //collider.enabled = false;
        SpriteRenderer.sortingOrder = 10;
        PuzzleMainController.AddPuzzle();
    }

}