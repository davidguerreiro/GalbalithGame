using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class MainMenuButton : MonoBehaviour
{
    public string id;

    [Header("Status")]
    public bool onFocus;
    
    [Header("Components")]
    public MainMenuButton prev;
    public MainMenuButton next;
    public MainMenu mainMenu;
    public GameObject cursor;
    public AudioComponent audio;
    public Animator textAnim;

    [Header("Settings")]
    public int playerID;

    private Rewired.Player _rePlayer;
    private bool awaiting; 

    // Start is called before the first frame update
    void Start() 
    {
        Init();
    }


    // Update is called once per frame
    void Update()
    {
        if (onFocus && ! awaiting) {
            ListenUserInput();
        }

        if (onFocus) {
            ListenForActionInput();
        }
    }

    private void Awake() {
        if (id == "NewGame") {
            textAnim.SetBool("Focus", true);
        }
    }

    /// <summary>
    /// On focus.
    /// </summary>
    private void OnFocus() 
    {
        StartCoroutine(AwaitCursor());
        audio.PlaySound(0);
        cursor.SetActive(true);
        textAnim.SetBool("Focus", true);
        onFocus = true;
    }

    /// <summary>
    /// Lost focus.
    /// </summary>
    private void LostFocus() 
    {
        cursor.SetActive(false);
        textAnim.SetBool("Focus", false);
        onFocus = false;
    }

    /// <summary>
    /// Listen for user input.
    /// </summary>
    public void ListenUserInput() {
        float deltaY = _rePlayer.GetAxis("MoveVertical");

        if (deltaY > 0f) {
            LostFocus();
            prev.OnFocus();
        }

        if (deltaY < 0f) {
            LostFocus();
            next.OnFocus();
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
    }

    /// <summary>
    /// Selected logic.
    /// </summary>
    private void Selected()
    {
        switch(id) {
            case "NewGame":
                mainMenu.StartNewGame();
                this.audio.gameObject.SetActive(false);
                break;
            case "Continue":
                audio.PlaySound(1);
                break;
            case "Options":
                audio.PlaySound(1);
                break;
            case "Exit":
                mainMenu.ExitGame();
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
        yield return new WaitForSeconds(.3f);
        awaiting = false;
    }

    /// <summary>
    /// Init class methdo.
    /// </summary>
    private void Init()
    {
        _rePlayer = ReInput.players.GetPlayer(playerID);
    }

}
