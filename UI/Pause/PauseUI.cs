using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PauseUI : MonoBehaviour
{
    [Header("Settings")]
    public float fadeSpeed;
    public float toWait;
    public int playerID;

    [Header("Components")]
    public Animator background;
    public FadeElement pauseText;
    public FadeElement lineLeft;
    public FadeElement lineRight;

    private Coroutine _pauseRutine;
    private AudioComponent _audio;
    private Rewired.Player _rePlayer;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.gameMenu.inMenu) {
            ListenForUserInput();
        }
    }

    /// <summary>
    /// Pause game and display pause screen.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator PauseGameRoutine()
    {
        _audio.PlaySound();
        Time.timeScale = 0;
        GameManager.instance.paused = true;
        GameManager.instance.LockGameMenu();

        background.SetBool("Displayed", true);
        yield return new WaitForSecondsRealtime(toWait);

        pauseText.FadeIn(fadeSpeed);
        lineLeft.FadeIn(fadeSpeed);
        lineRight.FadeIn(fadeSpeed);

        _pauseRutine = null;
    }

    /// <summary>
    /// Return game.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator ReturnToGame()
    {
        _audio.PlaySound();
        pauseText.FadeOut(fadeSpeed);
        lineLeft.FadeOut(fadeSpeed);
        lineRight.FadeOut(fadeSpeed);
        yield return new WaitForSecondsRealtime(.1f);

        background.SetBool("Displayed", false);

        Time.timeScale = 1;

        GameManager.instance.paused = false;
        GameManager.instance.UnlockGameMenu();

        _pauseRutine = null;
    }

    /// <summary>
    /// Listen for user input.
    /// </summary>
    private void ListenForUserInput()
    {
        if ( _rePlayer.GetButtonDown("Start") && _pauseRutine == null ) {
            if (GameManager.instance.paused) {
                _pauseRutine = StartCoroutine(ReturnToGame());
            } else {
                _pauseRutine = StartCoroutine(PauseGameRoutine());
            }
        }
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        _audio = GetComponent<AudioComponent>();
        _rePlayer = ReInput.players.GetPlayer(playerID);
    }
}
