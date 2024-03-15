using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KilledInfo : MonoBehaviour
{
    public PlayerMovement playerScore;
    public TMPro.TextMeshProUGUI scoreText;

    // Update is called once per frame
    void Update()
    {
        UpdateScore();
    }

    public void UpdateScore()
    {
        scoreText.text = "" + playerScore.GetPlayerScore();
    }
}
