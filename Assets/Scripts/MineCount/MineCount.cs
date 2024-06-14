using System.Collections;
using System.Collections.Generic;
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
