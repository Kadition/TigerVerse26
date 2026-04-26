using UnityEngine;
using TMPro;
using System.IO.Ports;
using System;
// REDO
// REDO
public class ScoreTracker : MonoBehaviour
{
    public static ScoreTracker instance;
    private int playerPoints = 0;
    private int opponentPoints = 0;

    private int redo;

    public TMP_Text floatingScoreText;
    public AudioClip Cheer;
    public AudioClip Aww;
    public AudioSource LeftCrowdAudioSource;
    public AudioSource RightCrowdAudioSource;

    // Serial port for LCD
    private SerialPort serial;
    private string portName = "COM4"; // Update this to your actual port

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

        // Open serial connection
        try
        {
            serial = new SerialPort(portName, 115200);
            serial.Open();
            Debug.Log("Serial port opened");
        }
        catch(Exception ex)
        {
            Debug.LogWarning($"Could not open serial port - LCD will not update {ex}");
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
        if (playerPoints >= 4 && playerPoints >= opponentPoints + 2) 
        {
            Debug.Log("Player Wins the Game!");
            BallCollision.instance.flipWhoServes();
            RightCrowdAudioSource.PlayOneShot(Cheer);
            LeftCrowdAudioSource.PlayOneShot(Cheer);
            SendToLCD("Player Wins!", "");
            ResetGameScore();
            return;
        }
        else if (opponentPoints >= 4 && opponentPoints >= playerPoints + 2)
        {
            Debug.Log("Opponent Wins the Game!");
            BallCollision.instance.flipWhoServes();
            LeftCrowdAudioSource.PlayOneShot(Aww);
            RightCrowdAudioSource.PlayOneShot(Aww);
            SendToLCD("Opponent Wins!", "");
            ResetGameScore();
            return;
        }

        floatingScoreText.text = GetTennisScoreText();
        SendToLCD(GetTennisScoreText(), "");
    }

    private void SendToLCD(string line1, string line2)
    {
        if (serial != null && serial.IsOpen)
        {
            try
            {
                serial.WriteLine(line1 + "|" + line2);
            }
            catch
            {
                Debug.LogWarning("Failed to send to LCD");
            }
        }
    }

    public string GetTennisScoreText()
    {
        string[] scoreTerms = { "Love", "15", "30", "40" };

        if (playerPoints >= 3 && opponentPoints >= 3)
        {
            if (playerPoints == opponentPoints)
                return "Deuce";
            else if (playerPoints == opponentPoints + 1)
                return "Advantage Player";
            else if (opponentPoints == playerPoints + 1)
                return "Advantage Opponent";
        }

        string pText = scoreTerms[playerPoints];
        string oText = scoreTerms[opponentPoints];
        return $"{pText} - {oText}";
    }

    private void ResetGameScore()
    {
        playerPoints = 0;
        opponentPoints = 0;
        floatingScoreText.text = GetTennisScoreText();
        Debug.Log("Score reset. New Game!");
    }

    void Start()
    {
        if (floatingScoreText != null)
            floatingScoreText.text = GetTennisScoreText();

        SendToLCD("Love - Love", "");
    }

    void OnApplicationQuit()
    {
        if (serial != null && serial.IsOpen)
            serial.Close();
    }

    void Update() { }
}