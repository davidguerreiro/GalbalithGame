using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Components")]
    public FadeElement cover;
    public FadeElement title;
    public FadeElement companyLogo;
    public FadeElement[] coverTitle;
    public GameObject pressStart;
    public GameObject menu;

    [Header("Rewired Controller")]
    public int playerID;

    private Coroutine _animRoutine;
    private bool _animCompleted;
    private bool _menuDisplayed;
    private Rewired.Player _rePlayer;
    private bool _startButton;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (_animCompleted && ! _menuDisplayed) {
            _startButton = _rePlayer.GetButtonDown("Start");
            if (_startButton) {
                DisplayMenu();
            }
        }
    }

    /// <summary>
    /// Display main menu animation coroutine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator MainMenuAnim()
    {
        // remove cover and play music.
        yield return new WaitForSeconds(1f);
        cover.FadeOut();
        GameManager.instance.levelMusicManager.PlayMainLevelSong();

        // display dragon in titlte.
        yield return new WaitForSeconds(2f);
        title.FadeIn(.05f);
        yield return new WaitForSeconds(5f);

        // display rest of the title.
        foreach (FadeElement cover in coverTitle) {
            cover.FadeOut();
        }
        yield return new WaitForSeconds(2f);

        // display company logo.
        companyLogo.FadeIn();
        yield return new WaitForSeconds(1.5f);

        // display press start text.
        pressStart.SetActive(true);
        
        _animCompleted = true;
        _animRoutine = null;
    }

    /// <summary>
    /// Display main menu options
    /// </summary>
    private void DisplayMenu()
    {
        pressStart.SetActive(false);
        menu.SetActive(true);
        _menuDisplayed = true;
    }

    /// <summary>
    /// Start new game.
    /// </summary>
    public void StartNewGame()
    {
        StartCoroutine(StartNewGameRoutine());
    }

    /// <summary>
    /// Start new game coroutine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator StartNewGameRoutine()
    {
        cover.FadeIn(.5f);
        GameManager.instance.levelMusicManager.StopLevelSong();
        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("TestingLevel");
    }

    /// <summary>
    /// Exit appplication.
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        _animRoutine = StartCoroutine(MainMenuAnim());
        _rePlayer = ReInput.players.GetPlayer(playerID);
    }

}
