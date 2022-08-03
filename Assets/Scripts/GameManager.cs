using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Range(1,4)]
    public int nbPlayers;
    public GameObject paddle;
    ArrayList padList = new ArrayList();
    object[,] keys = new object[4, 2] { { KeyCode.Z, KeyCode.S }, { KeyCode.UpArrow, KeyCode.DownArrow}, {KeyCode.V, KeyCode.B }, { KeyCode.R, KeyCode.T } };
    object[] posList = new object[4] { new Vector2(-4.5f, 0.0f), new Vector2(+4.5f, 0.0f), new Vector2(0.0f, -4.5f), new Vector2(0.0f, +4.5f) };
    public Ball ball;
    public GameObject goal1, goal2, goal3, goal4;
    public TextMeshProUGUI P1ScoreText, P2ScoreText, P3ScoreText, P4ScoreText;
    private int _p1Score, _p2Score, _p3Score, _p4Score;

    public void Awake()
    {
        goal2.SetActive(false);
        goal3.SetActive(false);
        goal4.SetActive(false);
        for (int k = 0; k < this.nbPlayers; k++)
        {
            GameObject prefabObject = Instantiate(paddle, new Vector2(0f, 0f), Quaternion.identity);
            this.padList.Add(prefabObject);
            if (k < 2)
            {
                prefabObject.GetComponent<Paddle>()._rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                prefabObject.GetComponent<Paddle>()._upDirection = Vector2.up;
                prefabObject.GetComponent<Paddle>()._downDirection = Vector2.down;
            }
            else
            {
                prefabObject.GetComponent<Paddle>()._rigidbody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                prefabObject.GetComponent<Paddle>().transform.Rotate(0, 0, 90);
                prefabObject.GetComponent<Paddle>()._upDirection = Vector2.left;
                prefabObject.GetComponent<Paddle>()._downDirection = Vector2.right;
            }
            prefabObject.GetComponent<Paddle>().upKey = (KeyCode)keys[k,0];
            prefabObject.GetComponent<Paddle>().downKey = (KeyCode)keys[k, 1];
            prefabObject.GetComponent<Paddle>()._basePos = (Vector2)posList[k];
            prefabObject.GetComponent<Paddle>().ResetPos();
            if (k == 1)
            {
                goal2.SetActive(true);
            }
            else if (k == 2)
            {
                goal3.SetActive(true);
            }
            else if (k == 3)
            {
                goal4.SetActive(true);
            }
        }
    }
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
    public void P3Scores()
    {
        _p3Score++;
        this.P3ScoreText.text = _p3Score.ToString();
        ResetRound();
    }
    public void P4Scores()
    {
        _p4Score++;
        this.P4ScoreText.text = _p4Score.ToString();
        ResetRound();
    }
    public void ResetRound()
    {
        this.ball.ResetPos();
        foreach(GameObject p in this.padList){
            p.GetComponent<Paddle>().ResetPos();
        }
    }
}
