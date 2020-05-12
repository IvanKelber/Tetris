using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{

    [SerializeField]
    private TMP_Text scoreText;
    [SerializeField]
    private TMP_Text speedText;
    [SerializeField]
    private TMP_Text linesText;

    int lines = 0;
    int score = 0;
    // float speed = 0.0f;
    void Awake()
    {
        Messenger<int>.AddListener(GameEvent.ROW_COMPLETED, OnRowCompleted);
        Messenger<int>.AddListener(GameEvent.SCORE_UPDATED, OnScoreUpdated);
        // Messenger<float>.AddListener(GameEvent.SPEED_UPDATED, OnSpeedUpdated);
    }

    void Destroy()
    {
        Messenger<int>.RemoveListener(GameEvent.ROW_COMPLETED, OnRowCompleted);
        Messenger<int>.RemoveListener(GameEvent.SCORE_UPDATED, OnScoreUpdated);
    }

    void Start()
    {
        scoreText.text = "Score: " + score;
        // speedText.text = "Speed: " + speed;
        linesText.text = "Lines: " + lines;
    }

    void OnRowCompleted(int lines)
    {
        this.lines = lines;
        linesText.text = "Lines: " + this.lines;
    }

    void OnScoreUpdated(int score)
    {
        this.score = score;
        scoreText.text = "Score: " + score;
    }

    void OnSpeedUpdated(float speed)
    {
        this.speed = speed;
        speedText.text = "Speed: " + speed;
    }


}
