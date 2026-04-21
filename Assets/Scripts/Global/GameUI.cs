using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{

    public GameObject gameLoseUI;
    public GameObject gameWinUI;
    bool gameIsOver;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GuardPath.OnGuardHasSpottedPlayer += ShowGameLoseUI;
        FindFirstObjectByType<PlayerController>().OnLevelEnd += ShowGameWinUI;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameIsOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    void ShowGameWinUI()
    {
        OnGameOver (gameWinUI);
    }

    void ShowGameLoseUI()
    {
        OnGameOver (gameLoseUI);
    }

    void OnGameOver(GameObject gameOverUI)
    {
        gameOverUI.SetActive(true);
        gameIsOver = true;
        GuardPath.OnGuardHasSpottedPlayer -= ShowGameLoseUI;
        FindFirstObjectByType<PlayerController>().OnLevelEnd -= ShowGameWinUI;
    }
}
