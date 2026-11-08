using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText;
    private int partsCount = 0;

    public void AddPart()
    {
        partsCount++;
        scoreText.text = "Parts Collected: " + partsCount.ToString();
    }

}
