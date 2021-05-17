using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class GameMenu : MonoBehaviour
{
    public bool inMenu;
    public bool menuBlocked;

    [Header("Components")] 
    public FadeElement background;
    public FadeElement sectionCover;
    [Header("Sections & Navegables")]
    public GameObject[] sections;
    public MenuSection[] navegables;

    public string currentSection;

    [Header("Section Parents")]
    public MenuSections mainSection;
    public MenuSections systemSection;

    [Header("Navegables")]
    public MenuSection menuNavegable;
    public MenuSection systemMenuNavegable;

    [Header("Settings")]
    public int playerID;

    [HideInInspector]
    public AudioComponent audio;
    private Rewired.Player _rePlayer;

    [HideInInspector]
    public Coroutine _displayRoutine;


    // Start is called before the first frame update
    void Start() {
        Init();
    }

    // Update is called once per frame
    void Update() {
        if (!menuBlocked) {
            ListenForUserInput();
        }
    }

    /// <summary>
    /// Listen for user input.
    /// </summary>
    private void ListenForUserInput() {
        if (_rePlayer.GetButtonDown("Menu") && _displayRoutine == null ) {
            if (!inMenu && !GameManager.instance.paused) {
                _displayRoutine = StartCoroutine(DisplayMenu());
            } else {
                _displayRoutine = StartCoroutine(ExitMenu());
            }
        }
    }

    /// <summary>
    /// Display menu.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator DisplayMenu()
    {
        inMenu = true;
        GameManager.instance.paused = true;
        Time.timeScale = 0;

        audio.PlaySound(0);
        background.FadeIn();
        sectionCover.FadeIn();
        yield return new WaitForSecondsRealtime(.5f);

        mainSection.gameObject.SetActive(true);
        mainSection.DisplaySections();
        sectionCover.FadeOut(2f);
        yield return new WaitForSecondsRealtime(.1f);
    
        menuNavegable.MakeNavegable();

        currentSection = "MainSection";

        _displayRoutine = null;
    }

    /// <summary>
    /// Exit menu.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator ExitMenu()
    {
        audio.PlaySound(1);

        mainSection.HideSections();
        
        foreach (GameObject section in sections) {
            section.SetActive(false);
        }

        foreach (MenuSection menu in navegables) {
            menu.DisableNavegable();
        }
        
        background.FadeOut();
        yield return new WaitForSecondsRealtime(.5f);

        currentSection = "MainSection";

        Time.timeScale = 1;
        GameManager.instance.paused = false;
        inMenu = false;

        _displayRoutine = null;
    }

    /// <sumamry>
    /// Close menu externally.
    /// This method is a wrapper of ExitMenu()
    /// and usually is called from another class.
    /// </summary>
    public void ExitMenuExternally()
    {
        if (_displayRoutine == null) {
            _displayRoutine = StartCoroutine(ExitMenu());
        }
    }

    /// <summary>
    /// Open section coroutine.
    /// </summary>
    /// <param name="sectionName">string - section name id</param>
    /// <returns>IEnumerator</returns>
    public IEnumerator OpenSectionRoutine(string sectionName)
    {
        sectionCover.FadeIn();

        foreach (GameObject section in sections) {
            section.GetComponent<MenuSections>().HideSections();
            section.SetActive(false);
        }

        foreach (MenuSection menu in navegables) {
            menu.DisableNavegable();
        }

        yield return new WaitForSecondsRealtime(.5f);

        switch(sectionName) {
            case "MainSection":
                mainSection.gameObject.SetActive(true);
                mainSection.DisplaySections();
                sectionCover.FadeOut(1.5f);
                yield return new WaitForSecondsRealtime(.1f);
                menuNavegable.MakeNavegable();
                currentSection = "MainSection";
                break;
            case "SystemSection":
                systemSection.gameObject.SetActive(true);
                systemSection.DisplaySections();
                sectionCover.FadeOut(1.5f);
                yield return new WaitForSecondsRealtime(.1f);
                systemMenuNavegable.MakeNavegable();
                currentSection = "SystemSection";
                break;
            default:
                break;
        }

        _displayRoutine = null;
    }

    /// <summary>
    /// Open section.
    /// </summary>
    /// <param name="sectionName">string - section name id</param>
    public void OpenSection(string sectionName)
    {
        if (_displayRoutine == null) {
            _displayRoutine = StartCoroutine(OpenSectionRoutine(sectionName));
        } 
    }

    /// <summary>
    /// Go back to menu title.
    /// </summary>
    public void GoToMenuTitle()
    {
        if (_displayRoutine == null) {
            _displayRoutine = StartCoroutine(GoToMenuRoutine());
        }
    }

    /// <summary>
    /// Go back to menu title coroutine.
    /// </summary>
    public IEnumerator GoToMenuRoutine()
    {
        audio.PlaySound(2);
        sectionCover.FadeIn(1.5f);
        yield return new WaitForSecondsRealtime(.8f);

        GameManager.instance.paused = false;
        Time.timeScale = 1;
        
        Utils.instance.LoadScene("MainMenu");
    }

    /// <summary>
    /// Exit game.
    /// </summary>
    public void ExitGame()
    {
        if (_displayRoutine == null) {
            _displayRoutine = StartCoroutine(ExitGameRoutine());
        }
    }

    /// <summary>
    /// Exit game coroutine.
    /// </summary>
    public IEnumerator ExitGameRoutine()
    {
        audio.PlaySound(2);
        sectionCover.FadeIn(1.5f);
        yield return new WaitForSecondsRealtime(.8f);

        Application.Quit();
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        audio = GetComponent<AudioComponent>();
        _rePlayer = ReInput.players.GetPlayer(playerID);
    }
}
