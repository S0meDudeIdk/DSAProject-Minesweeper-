using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]

public class Tile : MonoBehaviour {
    [Header("Tile Sprites")]
    [SerializeField] private Sprite unclickedTile;
    [SerializeField] private Sprite flaggedTile;
    [SerializeField] private List<Sprite> clickedTile;
    [SerializeField] private Sprite mineTile;
    [SerializeField] private Sprite mineWrongTile;
    [SerializeField] private Sprite mineHitTile;

    [Header("GM set via code")]
    public GameManager gameManager;

    private SpriteRenderer spriteRenderer;
    public bool flagged = false;
    public bool active = true;
    public bool isMine = false;
    public int mineCount = 0;

    void Awake() {
        // Must exists
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseOver() {
        // If it hasn't already been pressed
        if (active) {
            if (Input.GetMouseButtonDown(0)) {
                // Left click = Reveal tile contents
                ClickedTile();
            } else if (Input.GetMouseButtonDown(1)) {
                // Right click = Toggle flag
                flagged = !flagged;
                if (flagged) { 
                    spriteRenderer.sprite = flaggedTile;
                } else {
                    spriteRenderer.sprite = unclickedTile;
                }
            }
        } else {
            // Pressing Left Mouse + Right Mouse (or Middle Mouse)
            if ((Input.GetMouseButtonDown(0) && Input.GetMouseButtonDown(1))
                || Input.GetMouseButtonDown(2)) {
                    gameManager.Chording(this);
            }
        }
    }

    public void ClickedTile() {
        // Flagged => Not allow left click
        if (active & !flagged) {
            // Makes sure that left click cannot click again
            active = false;
            gameManager.SetFirstClickedTile(this);
            if (isMine) {
                // "You are ded. Not big surprise :3" - Heavy from TF2
                spriteRenderer.sprite = mineHitTile;
                gameManager.GameOver();
            } else {
                // Safe click, set the correct sprite
                spriteRenderer.sprite = clickedTile[mineCount];
                if (mineCount == 0) {
                    // Register that the click should expand out to the neighbours.
                    gameManager.ClickNeighbours(this);
                }
                // Checking for Game Over state whenever we make a change in board
                gameManager.CheckGameOver();
            }
        }
    }

    // If the mine tile clicked -> Game over
    public void ShowGameOverState() {
        if (active) {
            active = false;
            if (isMine & !flagged) {
                // The not-flagged mine tiles will be shown
                spriteRenderer.sprite = mineTile;
            } else if (flagged & !isMine) {
                /* The incorrectly flagged mine tiles 
                    will be shown as crossthrough mine tile */
                spriteRenderer.sprite = mineWrongTile;
            }
        }  
    }

    // Method to flag the rest of the remaining mine on game completion
    public void SetFlaggedIfMine() {
        if (isMine) {
            flagged = true;
            spriteRenderer.sprite = flaggedTile;
        }
    }
}
