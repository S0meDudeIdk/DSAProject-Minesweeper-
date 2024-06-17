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
        if (settingsUI != null) {
            settingsUI.SetActive(false);
            IsSettingsUIActive = false;
        }

        gameManager = FindObjectOfType<GameManager>();
        
        defaultButtonColor = beginnerButton.GetComponent<Image>().color;

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
        if (selectedButton != null) {
            selectedButton.GetComponent<Image>().color = defaultButtonColor;
        }
        button.GetComponent<Image>().color = Color.gray;
        selectedButton = button;

        widthInput.text = width.ToString();
        heightInput.text = height.ToString();
        mineInput.text = mines.ToString();
    }

    private void ApplySettings() {
        int width = Mathf.Clamp(int.Parse(widthInput.text), minWidth, maxWidth);
        int height = Mathf.Clamp(int.Parse(heightInput.text), minHeight, maxHeight);
        int maxMines = (width - 1) * (height - 1);
        int mines = Mathf.Clamp(int.Parse(mineInput.text), minMines, maxMines);

        gameManager.width = width;
        gameManager.height = height;
        gameManager.numMines = mines;
        gameManager.ResetGame();
        ToggleSettings();
    }

    private void CancelSettings() {
        ToggleSettings();
    }
}
