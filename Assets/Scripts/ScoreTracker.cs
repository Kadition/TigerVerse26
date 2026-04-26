using UnityEngine;
using TMPro;

public class ScoreTracker : MonoBehaviour
{
    public static ScoreTracker instance;
    // keep player scores
    private int playerPoints = 0;
    private int opponentPoints = 0;

    public TMP_Text floatingScoreText;
    public AudioClip Cheer;
    public AudioClip Aww;
    public AudioSource LeftCrowdAudioSource;
    public AudioSource RightCrowdAudioSource;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void RecordPoint(bool playerWins)
    {
        if (playerWins)
        {
            playerPoints++;
        }
        else
        {
            opponentPoints++;
        }

        ProcessScore();
    }

    private void ProcessScore()
    {
        //check game win
        if (playerPoints >= 4 && playerPoints >= opponentPoints + 2) 
        {
            Debug.Log("Player Wins the Game!");
            BallCollision.instance.flipWhoServes();
            RightCrowdAudioSource.PlayOneShot(Cheer);
            LeftCrowdAudioSource.PlayOneShot(Cheer);
            ResetGameScore();
            return; //game over
        }
        else if (opponentPoints >= 4 && opponentPoints >= playerPoints + 2)
        {
            Debug.Log("Opponent Wins the Game!");
            BallCollision.instance.flipWhoServes();
            LeftCrowdAudioSource.PlayOneShot(Aww);
            RightCrowdAudioSource.PlayOneShot(Aww);
            ResetGameScore();
            return; //game over
        }

        //If no one won yet, update the display
        floatingScoreText.text = GetTennisScoreText();
    }

    // Translates integers into tennis terms
    public string GetTennisScoreText()
    {
        // Array mapping index to tennis terms (0=Love, 1=15, 2=30, 3=40)
        string[] scoreTerms = { "Love", "15", "30", "40" };

        // Handle Deuce and Advantage (triggered if both players have at least 3 points / 40)
        if (playerPoints >= 3 && opponentPoints >= 3)
        {
            if (playerPoints == opponentPoints)
            {
                return "Deuce";
            }
            else if (playerPoints == opponentPoints + 1)
            {
                return "Advantage Player";
            }
            else if (opponentPoints == playerPoints + 1)
            {
                return "Advantage Opponent";
            }
        }

        //Handle standard scoring
        string pText = scoreTerms[playerPoints];
        string oText = scoreTerms[opponentPoints];

        return $"{pText} - {oText}";
    }
    private void ResetGameScore()
{
    playerPoints = 0;
    opponentPoints = 0;
    // Add this line so the UI updates to "Love - Love" immediately
    floatingScoreText.text = GetTennisScoreText(); 
    Debug.Log("Score reset. New Game!");
}


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (floatingScoreText != null)
        {
            floatingScoreText.text = GetTennisScoreText();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
