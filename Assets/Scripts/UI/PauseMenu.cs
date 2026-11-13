using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public InputActionReference pauseAction;
    public GameObject pausePanel;
    public GameObject firstSelectedButton;
    public string menuSceneName = "MainMenu";
    private bool isPaused = false;
    void Start()
    {
        pausePanel.SetActive(false);
    }
    void OnEnable()
    {
        pauseAction.action.performed += OnPausePressed;
    }
    void OnDisable()
    {
        pauseAction.action.performed -= OnPausePressed;
    }
    private void OnPausePressed(InputAction.CallbackContext context)
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
        if (firstSelectedButton != null && EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        }
    }
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }
}