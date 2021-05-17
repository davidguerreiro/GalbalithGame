using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class GameMenuButton : MonoBehaviour
{   
    public string id;

    [Header("Status")]
    public bool onFocus;

    [Header("Components")]
    public GameMenuButton prev;
    public GameMenuButton next;
    public GameObject cursor;
    public TextComponent infoText;
    public string wrapperSectionName;

    [Header("Settings")]
    public int playerID;

    [TextArea(5, 30)]
    public string info;

    private Rewired.Player _rePlayer;
    private bool awaiting;
    private Animator _cursorAnim;


    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (onFocus && ! awaiting) {
            ListerForDirectionInput();
        }

        if (onFocus) {
            ListenForActionInput();
        }
    }

    /// <summary>
    /// On focus.
    /// </summary>
    private void OnFocus() 
    {
        StartCoroutine(AwaitCursor());
        GameManager.instance.gameMenu.audio.PlaySound(2);
        cursor.SetActive(true);
        infoText.UpdateContent(info);
        onFocus = true;
    }

    /// <summary>
    /// Lost focus.
    /// </summary>
    private void LostFocus() 
    {
        cursor.SetActive(false);
        onFocus = false;
    }

    /// <summary>
    /// Selected logic.
    /// </summary>
    private void Selected()
    {
        switch(id) {
            case "items":
                GameManager.instance.gameMenu.audio.PlaySound(3);
                break;
            case "magic":
                GameManager.instance.gameMenu.audio.PlaySound(3);
                break;
            case "skills":
                GameManager.instance.gameMenu.audio.PlaySound(3);
                break;
            case "equipment":
                GameManager.instance.gameMenu.audio.PlaySound(3);
                break;
            case "quests":
                GameManager.instance.gameMenu.audio.PlaySound(3);
                break;
            case "status":
                GameManager.instance.gameMenu.audio.PlaySound(3);
                break;
            case "formation":
                GameManager.instance.gameMenu.audio.PlaySound(3);
                break;
            case "system":
                GameManager.instance.gameMenu.audio.PlaySound(2);
                GameManager.instance.gameMenu.OpenSection("SystemSection");
                break;
            case "save":
                GameManager.instance.gameMenu.audio.PlaySound(3);
                break;
            case "load":
                GameManager.instance.gameMenu.audio.PlaySound(3);
                break;
            case "options":
                GameManager.instance.gameMenu.audio.PlaySound(3);
                break;
            case "exitToMenu":
                GameManager.instance.gameMenu.GoToMenuTitle();
                break;
            case "exitGame":
                GameManager.instance.gameMenu.ExitGame();
                break;
        }
    }

    /// <summary>
    /// Listen for user input.
    /// </summary>
    private void ListerForDirectionInput() {
        float deltaY = _rePlayer.GetAxis("MoveVertical");

        if (deltaY > 0f) {
            LostFocus();
            prev.OnFocus();
        }

        if (deltaY < 0f) {
            LostFocus();
            next.OnFocus();
        }

        if (_rePlayer.GetButtonDown("Action")) {
            Selected();
        }

        if (_rePlayer.GetButtonDown("Cancel")) {
            Canceled();
        }

    }

    /// <summary>
    /// Listen for action input.
    /// </summary>
    private void ListenForActionInput()
    {
        if (_rePlayer.GetButtonDown("Action")) {
            Selected();
        }

        if (_rePlayer.GetButtonDown("Cancel")) {
            Canceled();
        }
    }

    /// <summary>
    /// Canceled behaviour.
    /// This behaviour depends of current 
    /// active section.
    /// </summary>
    public void Canceled()
    {
        switch (wrapperSectionName) {
            case "MainSection":
                GameManager.instance.gameMenu.ExitMenuExternally();
                break;
            case "SystemSection":
                GameManager.instance.gameMenu.audio.PlaySound(1);
                GameManager.instance.gameMenu.OpenSection("MainSection");
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Await to avoid multiple
    /// joystick, gamepad cursor.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator AwaitCursor()
    {
        awaiting = true;
        yield return new WaitForSecondsRealtime(.25f);
        awaiting = false;
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    public void Init()
    {
        _rePlayer = ReInput.players.GetPlayer(playerID);
        _cursorAnim = cursor.GetComponent<Animator>();

        if (onFocus) {
            cursor.SetActive(true);
        }
    }
}
