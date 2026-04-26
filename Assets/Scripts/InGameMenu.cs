using UnityEngine;

public class Menu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Menu UI Reference")]
    public GameObject menuPanel;

    public void PlayGame()
    {
        // Unfreezes the game to start playing
        Time.timeScale = 1f;

        // Destroys the menu object completely
        Destroy(menuPanel);
    }
    void Start()
    {
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
