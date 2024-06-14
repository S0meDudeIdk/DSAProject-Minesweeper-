using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] private Transform tilePrefab;
    [SerializeField] private Transform gameHolder;
    [SerializeField] private Timer timer;

    private List<Tile> tiles = new();

    private int width;
    private int height;
    private int numMines;

    private readonly float tileSize = 0.5f;

    private Tile firstClickedTile = null;

    // Start is called before the first frame update
    void Start() {
        // CreateGameBoard(9, 9, 10);                  // Beginner
        CreateGameBoard (16, 16, 40);            // Intermidiate
        // CreateGameBoard (30, 16, 99);            // Expert
    }

    public void CreateGameBoard(int width, int height, int numMines) {
        // Save the game parameters
        this.width = width;
        this.height = height;
        this.numMines = numMines;

        // Create a Board with tiles
        for (int row = 0; row < height; row++) {
            for (int col = 0; col < width; col++) {
                // Position the tile in the center
                Transform tileTransform = Instantiate(tilePrefab);
                tileTransform.parent = gameHolder;

                float xIndex = col - ((width - 1) / 2.0f);
                float yIndex = row - ((height - 1) / 2.0f);
                tileTransform.localPosition = new Vector2(xIndex * tileSize,
                                                            yIndex * tileSize);

                // Keep referencing to the tile for setting up the game
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
        // Suffle tiles' position for the mine
        int[] minePositions = Enumerable.Range(0, tiles.Count)
                                        .Where(i => tiles[i] != firstClickedTile)
                                        .OrderBy(x => Random.Range(0.0f, 1.0f))
                                        .ToArray();

        // Set mines at the first numMines positions
        for (int i = 0; i < numMines; i++) {
            int pos = minePositions[i];
            tiles[pos].isMine = true;
        }

        // Update all the tiles to hold the correct number of mines
        for (int i = 0; i < tiles.Count; i++) {
            tiles[i].mineCount = HowManyMines(i);
        }
    }

    private int HowManyMines(int location) {
        int count = 0;
        foreach (int pos in GetNeighbors(location)) {
            if (tiles[pos].isMine) {
                count++;
            }
        }
        return count;
    }

    // Given a position, return the positions of all neighbors
    private List<int> GetNeighbors(int pos) {
        List<int> neighbors = new();
        int row = pos / width;
        int col = pos % width;
        // First position (0,0) is bottom left
        if (row < (height - 1)) {
            neighbors.Add(pos + width);             // North
            if (col > 0) {
                neighbors.Add(pos + width - 1);     // North West
            }
            if (col < (width - 1)) {
                neighbors.Add(pos + width + 1);     // North East
            }
        }
        if (col > 0) {
            neighbors.Add(pos - 1); // West
        }
        if (col < (width - 1)) {
            neighbors.Add(pos + 1); // East
        }
        if (row > 0) {
            neighbors.Add(pos - width); // South
            if (col > 0) {
                neighbors.Add(pos - width - 1); // South-West
            }
            if (col < (width - 1)) {
                neighbors.Add(pos - width + 1); // South-East
            }
        }
        return neighbors;
    }

    public void ClickNeighbours(Tile tile) {
        int location = tiles.IndexOf(tile);
        foreach (int pos in GetNeighbors(location)) {
            tiles[pos].ClickedTile();
        }
    }

    public void GameOver() {
        // Disable clicks
        foreach (Tile tile in tiles) {
            tile.ShowGameOverState();
        }
        timer.StopTimer();
    }

    public void CheckGameOver() {
        // if numMines still left active => We're cool
        int count = 0;
        foreach (Tile tile in tiles) {
            if (tile.active) {
                count++;
            }
        }
        if (count == numMines) {
            // Flag and disable everything, we won
            Debug.Log("You Survived! For now...");
            foreach (Tile tile in tiles) {
                tile.active = false;
                tile.SetFlaggedIfMine();
            }
            timer.StopTimer();
        }
    }

    public void Chording(Tile tile) {
        int location = tiles.IndexOf(tile);
        // Get the number of flags
        int flagCount = 0;
        foreach (int pos in GetNeighbors(location)) {
            if (tiles[pos].flagged) {
                flagCount++;
            }
        }
        // If we have the right number -> Click surrounding tiles.
        if (flagCount == tile.mineCount) {
            ClickNeighbours(tile);
        }
    }
}
