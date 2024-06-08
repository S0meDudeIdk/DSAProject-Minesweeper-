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
        }
    }

    public void ClickedTile() {
        // Flagged => Not allow left click
        if (active & !flagged) {
            // Makes sure that left click cannot click again
            active = false;
            if (isMine) {
                // "You are ded. Not big surprise :3" - Heavy from TF2
                spriteRenderer.sprite = mineHitTile;
            } else {
                // Safe click, set the correct sprite
                spriteRenderer.sprite = clickedTile[mineCount];
                if (mineCount == 0) {
                    // Register that the click should expand out to the neighbours.
                    gameManager.ClickNeighbours(this);
                }
            }
        }
    }
}
