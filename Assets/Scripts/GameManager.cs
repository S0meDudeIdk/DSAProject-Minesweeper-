using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] private Transform tilePrefab;
    [SerializeField] private Transform gameHolder;

    private int width;
    private int height;
    private int numMines;

    private readonly float tileSize = 0.5f;

    // Start is called before the first frame update
    void Start() {
        CreateGameBoard(9, 9, 10);                  // Beginner
        // CreateGameBoard (16, 16, 40);            // Intermidiate
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
            }
        }
    }
}
