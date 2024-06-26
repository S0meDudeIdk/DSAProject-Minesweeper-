/*  Name: Pham Vu Hoang Bao
    ID: ITCSIU22250
    Purpose: Manages the behavior of individual tiles in the Minesweeper game, 
            including handling clicks, flags, and displaying the correct state.
*/

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
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseOver() {
        if (gameManager.gameState != GameManager.GameState.Playing || SettingsManager.IsSettingsUIActive) {
            return;
        }

        if (active) {
            HandleTileClick();
        } else {
            HandleChording();
        }
    }

    private void OnMouseUp() {
        if (gameManager.gameState == GameManager.GameState.Playing) {
            gameManager.smileButton.SetSmileyDefault();
        }
    }

    private void HandleTileClick() {
        if (Input.GetMouseButtonDown(0)) {
            ClickedTile();
        } else if (Input.GetMouseButtonDown(1)) {
            ToggleFlag();
        }
    }

    private void HandleChording() {
        if ((Input.GetMouseButtonDown(0) && Input.GetMouseButtonDown(1)) || Input.GetMouseButtonDown(2)) {
            gameManager.Chording(this);
        }
    }

    private void ToggleFlag() {
        flagged = !flagged;
        spriteRenderer.sprite = flagged ? flaggedTile : unclickedTile;
        gameManager.UpdateFlagCount(flagged ? 1 : -1);
    }

    public void ClickedTile() {
        if (active && !flagged) {
            active = false;
            gameManager.SetFirstClickedTile(this);
            if (isMine) {
                spriteRenderer.sprite = mineHitTile;
                gameManager.GameOver();
            } else {
                spriteRenderer.sprite = clickedTile[mineCount];
                if (mineCount == 0) {
                    RevealAdjacentEmptyCells();
                }
                gameManager.CheckGameOver();
            }
        }
    }

    // Applying Depth First Search
    private void RevealAdjacentEmptyCells() {
        Stack<Tile> stack = new Stack<Tile>();
        stack.Push(this);

        while (stack.Count > 0) {
            Tile current = stack.Pop();
            current.ClickedTile();

            if (current.mineCount == 0) {
                foreach (Tile neighbor in gameManager.GetNeighbors(current)) {
                    if (neighbor.active && !neighbor.flagged 
                                        && !neighbor.isMine) {
                        stack.Push(neighbor);
                    }
                }
            }
        }
    }

    public void ShowGameOverState() {
        if (active) {
            active = false;
            spriteRenderer.sprite = isMine && !flagged ? mineTile : flagged && !isMine ? mineWrongTile : spriteRenderer.sprite;
        }
    }

    public void SetFlaggedIfMine() {
        if (isMine) {
            flagged = true;
            spriteRenderer.sprite = flaggedTile;
        }
    }
}
