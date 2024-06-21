/*  Name: Pham Vu Hoang Bao
    ID: ITCSIU22250
    Purpose: Manages the smiley button functionality in the Minesweeper game, 
            including changing the smiley face sprite and resetting the game.
*/
using UnityEngine;
using UnityEngine.UI;

public class SmileButton : MonoBehaviour {
    [SerializeField] private Sprite smileyDefault;
    [SerializeField] private Sprite smileyOpen;
    [SerializeField] private Sprite smileyDead;
    [SerializeField] private Sprite smileyCool;

    private Image image;
    private Button button;

    void Awake() {
        image = GetComponent<Image>();
        button = GetComponent<Button>();

        // Add listener for the button click
        button.onClick.AddListener(OnClick);

        SetSmileyDefault();
    }

    public void SetSmileyDefault() {
        image.sprite = smileyDefault;
    }

    public void SetSmileyOpen() {
        image.sprite = smileyOpen;
    }

    public void SetSmileyDead() {
        image.sprite = smileyDead;
    }

    public void SetSmileyCool() {
        image.sprite = smileyCool;
    }

    private void OnClick() {
        FindObjectOfType<GameManager>().ResetGame();
    }
}

