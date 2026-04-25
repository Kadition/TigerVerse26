using UnityEngine;

public class ScoreTracker : MonoBehaviour
{

    // keep player scores
    private int playerPoints = 0;
    private int opponentPoints = 0;
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
            ResetGameScore();
            return; //game over
        }
        else if (opponentPoints >= 4 && opponentPoints >= playerPoints + 2)
        {
            Debug.Log("Opponent Wins the Game!");
            ResetGameScore();
            return; //game over
        }

        //If no one won yet, update the display
        Debug.Log("Current Score: " + GetTennisScoreText());
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
        Debug.Log("Score reset. New Game!");
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
