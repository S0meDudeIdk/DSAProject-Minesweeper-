/*  Name: Pham Vu Hoang Bao
    ID: ITCSIU22250
    Purpose: Manages the display of the mine count in the Minesweeper game, 
            updating the count as tiles are flagged or unflagged.
*/

using UnityEngine;
using TMPro;

public class MineCount : MonoBehaviour {
    [SerializeField] TextMeshProUGUI countText;

    private int mineCount;

    public void SetMineCount(int count) {
        mineCount = count;
        UpdateDisplay();
    }

    public void UpdateMineCount(int delta) {
        mineCount += delta;
        UpdateDisplay();
    }

    private void UpdateDisplay() {
        if (mineCount < 0) {
            int absCount = Mathf.Abs(mineCount) % 100;
            countText.text = "-" + absCount.ToString("D2");
        } else {
            countText.text = mineCount.ToString("D3");
        }
    }
}
