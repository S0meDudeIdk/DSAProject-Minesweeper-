/*  Name: Pham Vu Hoang Bao
    ID: ITCSIU22250
    Purpose: Manages the settings UI for the Minesweeper game, 
            including difficulty selection and custom game settings.
*/

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour {
    [SerializeField] private GameObject settingsUI;
    [SerializeField] private Button beginnerButton;
    [SerializeField] private Button intermediateButton;
    [SerializeField] private Button expertButton;
    [SerializeField] private Button insaneButton;
    [SerializeField] private TMP_InputField widthInput;
    [SerializeField] private TMP_InputField heightInput;
    [SerializeField] private TMP_InputField mineInput;
    [SerializeField] private Button applyButton;
    [SerializeField] private Button cancelButton;

    private Color defaultButtonColor;
    private Button selectedButton;
    private GameManager gameManager;

    private const int minWidth = 9;
    private const int maxWidth = 30;
    private const int minHeight = 9;
    private const int maxHeight = 30;
    private const int minMines = 10;

    public static bool IsSettingsUIActive { get; private set; } = false;

    void Start() {
        InitializeSettingsUI();
        gameManager = FindObjectOfType<GameManager>();
        defaultButtonColor = beginnerButton.GetComponent<Image>().color;

        AddButtonListeners();
    }

    private void InitializeSettingsUI() {
        if (settingsUI != null) {
            settingsUI.SetActive(false);
            IsSettingsUIActive = false;
        }
    }

    private void AddButtonListeners() {
        beginnerButton.onClick.AddListener(() => SelectDifficulty(beginnerButton, 9, 9, 10));
        intermediateButton.onClick.AddListener(() => SelectDifficulty(intermediateButton, 16, 16, 40));
        expertButton.onClick.AddListener(() => SelectDifficulty(expertButton, 30, 16, 99));
        insaneButton.onClick.AddListener(() => SelectDifficulty(insaneButton, 30, 24, 200));

        applyButton.onClick.AddListener(ApplySettings);
        cancelButton.onClick.AddListener(CancelSettings);
    }

    public void ToggleSettings() {
        if (settingsUI != null) {
            settingsUI.SetActive(!settingsUI.activeSelf);
            IsSettingsUIActive = settingsUI.activeSelf;
        }
    }

    private void SelectDifficulty(Button button, int width, int height, int mines) {
        DeselectPreviousButton();
        HighlightSelectedButton(button);

        SetInputFields(width, height, mines);
    }

    private void DeselectPreviousButton() {
        if (selectedButton != null) {
            selectedButton.GetComponent<Image>().color = defaultButtonColor;
        }
    }

    private void HighlightSelectedButton(Button button) {
        button.GetComponent<Image>().color = Color.gray;
        selectedButton = button;
    }

    private void SetInputFields(int width, int height, int mines) {
        widthInput.text = width.ToString();
        heightInput.text = height.ToString();
        mineInput.text = mines.ToString();
    }

    private void ApplySettings() {
        int width = ParseInput(widthInput.text, minWidth, maxWidth);
        int height = ParseInput(heightInput.text, minHeight, maxHeight);
        int maxMines = (width - 1) * (height - 1);
        int mines = ParseInput(mineInput.text, minMines, maxMines);

        gameManager.width = width;
        gameManager.height = height;
        gameManager.numMines = mines;
        gameManager.ResetGame();
        ToggleSettings();
    }

    private int ParseInput(string input, int minValue, int maxValue) {
        if (int.TryParse(input, out int value)) {
            return Mathf.Clamp(value, minValue, maxValue);
        }
        return minValue;
    }

    private void CancelSettings() {
        ToggleSettings();
    }
}
