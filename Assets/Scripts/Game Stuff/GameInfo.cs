using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameInfo : MonoBehaviour
{
    public DungeonEnemyRoom dungeonEnemyRoom;
    public TMPro.TextMeshProUGUI difficultyText;

    // Update is called once per frame
    void Update()
    {
        UpdateCurrDifficulty();
    }

    public void UpdateCurrDifficulty()
    {
        difficultyText.text = "" + dungeonEnemyRoom.difficulty.ToString();
    }
}
