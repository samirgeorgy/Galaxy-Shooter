using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Sprite[] lives;
    public Image livesImageDisplay;
    public int score;
    public Text scoreText;

    public GameObject gameTitleScreen;
    public GameObject pressSpaceText;

    private void Update()
    {
        
    }

    /// <summary>
    /// This function updates the lives images
    /// </summary>
    /// <param name="lifeIndex">The current life count of the player</param>
	public void UpdateLives(int lifeIndex)
    {
        livesImageDisplay.sprite = lives[lifeIndex];
    }

    /// <summary>
    /// This function updates the score when the player kills an enemy
    /// </summary>
    public void UpdateScore()
    {
        score += 10;
        scoreText.text = "Score: " + score.ToString();
    }

    /// <summary>
    /// Shows the title screen
    /// </summary>
    public void ShowTitleScreen()
    {
        gameTitleScreen.SetActive(true);
        pressSpaceText.SetActive(true);
    }

    /// <summary>
    /// Hides the title screen
    /// </summary>
    public void HideTitleScreen()
    {
        gameTitleScreen.SetActive(false);
        pressSpaceText.SetActive(false);
        scoreText.text = "Score: 0";
    }
}
