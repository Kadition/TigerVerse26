using UnityEngine;
using UnityEngine.InputSystem;

public class Menu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Menu UI Reference")]
    public GameObject menuPanel;
    public static int currentDifficulty = 2; // Default to Medium
                                             //1=easy, 2=medium, 3=hard
    public InputActionReference rightMenuButton;
    private bool isMenuOpen = true;

    private void OnEnable()
    {
        if (rightMenuButton != null)
        {
            rightMenuButton.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (rightMenuButton != null)
        {
            rightMenuButton.action.Disable();
        }
    }
    void Start()
    {
        OpenMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (rightMenuButton != null && rightMenuButton.action.WasPressedThisFrame())
        {
            if (isMenuOpen)
            {
                PlayGame();
            }
            else
            {
                OpenMenu();
            }
        }
    }

    public void OpenMenu()
    {
        isMenuOpen = true;
        menuPanel.SetActive(true); // Show the menu

        Time.timeScale = 0f;
    }

    public void PlayGame()
    {
        isMenuOpen = false;
        menuPanel.SetActive(false); // Hide the menu
        Time.timeScale = 1f;        // Unfreeze the game
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game triggered!");
        Application.Quit();
    }

    // --- DIFFICULTY FUNCTIONS ---
    public void SetDifficultyEasy()
    {
        currentDifficulty = 1;
        Debug.Log("Difficulty set to: Easy");
    }

    public void SetDifficultyMedium()
    {
        currentDifficulty = 2;
        Debug.Log("Difficulty set to: Medium");
    }

    public void SetDifficultyHard()
    {
        currentDifficulty = 3;
        Debug.Log("Difficulty set to: Hard");
    }
    
}
