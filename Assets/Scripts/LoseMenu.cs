using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoseMenu : MonoBehaviour
{
    [SerializeField]
    private TMP_Text scoreText;
    [SerializeField]
    private TMP_Text linesText;

    private void Awake()
    {
        scoreText.text = "score: " + GameStateManager.final_score;
        linesText.text = "lines: " + GameStateManager.lines;
    }
}
