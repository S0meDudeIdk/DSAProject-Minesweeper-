/*  Name: Pham Vu Hoang Bao
    ID: ITCSIU22250
    Purpose: Manages the Minesweeper game, 
            including game state, tile creation, and game logic.
*/

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public enum GameState { Playing, GameOver, GameWon }
    [SerializeField] private Transform tilePrefab;
    [SerializeField] private Transform gameHolder;
    [SerializeField] private Timer timer;
    [SerializeField] private MineCount mineCountDisplay;
    [SerializeField] public SmileButton smileButton;

    private List<Tile> tiles = new List<Tile>();
    public GameState gameState = GameState.Playing;

    public int width;
    public int height;
    public int numMines;
    public int flaggedTiles = 0;

    private const float tileSize = 0.25f;
    private Tile firstClickedTile = null;

    void Start() {
        CreateGameBoard(9, 9, 10);                  
    }

    public void CreateGameBoard(int width, int height, int numMines) {
        this.width = width;
        this.height = height;
        this.numMines = numMines;
        mineCountDisplay.SetMineCount(numMines);
        smileButton.SetSmileyDefault();
        gameState = GameState.Playing;

        CreateTiles();
    }

    private void CreateTiles() {
        for (int row = 0; row < height; row++) {
            for (int col = 0; col < width; col++) {
                Transform tileTransform = Instantiate(tilePrefab, gameHolder);
                float xIndex = col - (width - 1) / 2.0f;
                float yIndex = row - (height - 1) / 2.0f;
                tileTransform.localPosition = new Vector2(xIndex * tileSize, yIndex * tileSize);

                Tile tile = tileTransform.GetComponent<Tile>();
                tiles.Add(tile);
                tile.gameManager = this;
            }
        }
    }

    public void SetFirstClickedTile(Tile tile) {
        if (firstClickedTile == null) {
            firstClickedTile = tile;
            ResetGameState();
            timer.StartTimer();
        }
    }

    private void ResetGameState() {
        int[] minePositions = Enumerable.Range(0, tiles.Count)
                                        .Where(i => tiles[i] != firstClickedTile)
                                        .OrderBy(x => Random.value)
                                        .Take(numMines)
                                        .ToArray();

        foreach (int pos in minePositions) {
            tiles[pos].isMine = true;
        }

        foreach (Tile tile in tiles) {
            tile.mineCount = GetMineCount(tile);
        }
    }

    private int GetMineCount(Tile tile) {
        return GetNeighbors(tile).Count(neighbor => neighbor.isMine);
    }

    public List<Tile> GetNeighbors(Tile tile) {
        int index = tiles.IndexOf(tile);
        int row = index / width;
        int col = index % width;
        List<Tile> neighbors = new List<Tile>();

        void AddNeighbor(int r, int c) {
            if (r >= 0 && r < height && c >= 0 && c < width) {
                neighbors.Add(tiles[r * width + c]);
            }
        }

        AddNeighbor(row - 1, col);
        AddNeighbor(row + 1, col);
        AddNeighbor(row, col - 1);
        AddNeighbor(row, col + 1);
        AddNeighbor(row - 1, col - 1);
        AddNeighbor(row - 1, col + 1);
        AddNeighbor(row + 1, col - 1);
        AddNeighbor(row + 1, col + 1);

        return neighbors;
    }

    public void ClickNeighbours(Tile tile) {
        foreach (Tile neighbor in GetNeighbors(tile)) {
            neighbor.ClickedTile();
        }
    }

    public void GameOver() {
        gameState = GameState.GameOver;
        foreach (Tile tile in tiles) {
            tile.ShowGameOverState();
        }
        timer.StopTimer();
        smileButton.SetSmileyDead();
    }

    public void CheckGameOver() {
        if (tiles.Count(t => t.active) == numMines) {
            foreach (Tile tile in tiles) {
                tile.SetFlaggedIfMine();
                tile.active = false;
            }
            timer.StopTimer();
            mineCountDisplay.SetMineCount(0);
            smileButton.SetSmileyCool();
            gameState = GameState.GameWon;
        }
    }

    public void Chording(Tile tile) {
        if (GetNeighbors(tile).Count(neighbor => neighbor.flagged) == tile.mineCount) {
            ClickNeighbours(tile);
        }
    }

    public void UpdateFlagCount(int delta) {
        flaggedTiles += delta;
        mineCountDisplay.UpdateMineCount(-delta);
    }

    public void ResetGame() {
        foreach (Transform child in gameHolder) {
            Destroy(child.gameObject);
        }

        tiles.Clear();
        firstClickedTile = null;
        flaggedTiles = 0;
        timer.ResetTimer();
        smileButton.SetSmileyDefault();
        gameState = GameState.Playing;

        CreateGameBoard(width, height, numMines);
    }
}
