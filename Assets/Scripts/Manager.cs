using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


// Game manager class
public class Manager : MonoBehaviour
{
    // Board of the game
    public TileBoard board;

    // Game over canvas
    public CanvasGroup gameOver;
    
    // Score texts
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highscoreText;
    
    // Player's score
    private int _score;

    private void Start()
    {
        // Start the game
        NewGame();
    }
    
    // Prepare a new game
    public void NewGame()
    {
        // When starting, make the game over screen disappear and disable interactions with it
        gameOver.alpha = 0f;
        gameOver.interactable = false;
        
        // Reset the score
        SetScore(0);
        // Set the highscore text
        highscoreText.text = LoadHighscore().ToString();
        
        // Clear the board
        board.ClearBoard();
        
        // Create two tiles to begin
        board.CreateTile();
        board.CreateTile();
        
        // Enable the board to allow input
        board.enabled = true;
    }

    // Handle losing
    public void GameOver()
    {
        // Disable the board, prevent from input
        board.enabled = false;
        // Enable the game over screen interactions
        gameOver.interactable = true;

        // Start the animation
        StartCoroutine(Fade(gameOver, 1f, 1.5f));
    }

    // Animate game fading when losing
    private IEnumerator Fade(CanvasGroup canvasGroup, float max, float delay)
    {
        // Wait for the delay time
        yield return new WaitForSeconds(delay);

        // Store the animation information
        float elapsed = 0f;
        float duration = 0.5f;
        float current = canvasGroup.alpha;

        // While the animation is on
        while (elapsed < duration)
        {
            // Increase the current alpha of game over screen
            canvasGroup.alpha = Mathf.Lerp(current, max, elapsed / duration);
            // Increase animation frame
            elapsed += Time.deltaTime;
            
            yield return null;
        }
        
        // Set the alpha to the target amount
        canvasGroup.alpha = max;
    }
    
    // Increase the score by given amount of points
    public void IncreaseScore(int points)
    {
        SetScore(_score + points);
    }

    // Set the score
    private void SetScore(int score)
    {
        this._score = score;
        scoreText.text = score.ToString();
        
        // Save the highscore if needed
        SaveHighscore();
    }
    
    // Load the highscore from player's prefs
    private int LoadHighscore()
    {
        return PlayerPrefs.GetInt("highscore", 0);
    }

    // Save the highscore if the current score it's higher than it
    private void SaveHighscore()
    {
        // Load the highscore
        int highscore = LoadHighscore();
        
        // If current score is higher than it, save it as a new highscore
        if (highscore < _score)
            PlayerPrefs.SetInt("highscore", _score);
    }
}
