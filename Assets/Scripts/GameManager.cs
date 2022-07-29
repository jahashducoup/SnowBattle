using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Ball ball;
    public Paddle P1Paddle;
    public Paddle P2Paddle;
    public TextMeshProUGUI P1ScoreText;
    public TextMeshProUGUI P2ScoreText;
    private int _p1Score;
    private int _p2Score;

    public void P1Scores()
    {
        _p1Score++;
        this.P1ScoreText.text = _p1Score.ToString();
        ResetRound();
    }
    public void P2Scores()
    {
        _p2Score++;
        this.P2ScoreText.text = _p2Score.ToString();
        ResetRound();
    }
    public void ResetRound()
    {
        this.ball.ResetPos();
        this.P1Paddle.ResetPos();
        this.P2Paddle.ResetPos();
    }
}
