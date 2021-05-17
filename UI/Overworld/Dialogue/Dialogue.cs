using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class Dialogue : MonoBehaviour
{
    public DialogueContent data;

    [Header("Status")]
    public bool playing;

    [Header("Components")]
    public Image speakerBox;
    public TextComponent speakerName;
    public TextComponent content;
    public GameObject cursor;

    [Header("Settings")]
    public float toWait;                        // How long to wait between letters are displayed in the screen.
    public float toWaitReduced;                 // How long to wait with the action button pressed.
    public int playerID;

    private Animator _anim;
    private AudioComponent _audio;
    private Coroutine _dialogueRoutine;
    private Rewired.Player _rePlayer;
    private bool toContinue;                    // Flag to control if player press the button to continue or to finish dialogue.

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }


    /// <summary>
    /// Play dialogue.
    /// </summary>
    public void Play()
    {
        if (_dialogueRoutine == null && data != null) {
            _dialogueRoutine = StartCoroutine(PlayRoutine());
        }
    }

    /// <summary>
    /// Play dialogue coroutine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator PlayRoutine()
    {
        playing = true;
        string currentText;
        char[] dialogueLetters;

        if (data.useLocalVars) {
            data.SetLocalVars();
        }

        speakerBox.color = data.dialogue[0].color;
        _anim.SetBool("Displayed", true);
        yield return new WaitForSeconds(.5f);

        for (int i = 0; i < data.dialogue.Length; i++) {
            content.UpdateContent("");
            
            if (cursor.activeSelf == true) {
                cursor.SetActive(false);
            }

            // update speaker box;
            speakerName.UpdateContent(data.dialogue[i].speaker);
            speakerBox.color = data.dialogue[i].color;

            dialogueLetters = data.dialogue[i].content.ToCharArray();

            for (int j = 0; j < dialogueLetters.Length; j++) {
                
                currentText = content.GetContent();

                if (currentText == "") {
                    currentText = dialogueLetters[j].ToString();
                } else {
                    currentText += dialogueLetters[j].ToString();
                }

                content.UpdateContent(currentText);
                
                if (_rePlayer.GetButton("Action")) {
                    yield return new WaitForSeconds(toWaitReduced);
                } else {
                    yield return new WaitForSeconds(toWait);
                }
            }

            if ( (i + 1) < data.dialogue.Length) {
                cursor.SetActive(true);
            }

            // wait for user to press action button to continue, or close, the dialogue.
            while (_rePlayer.GetButtonDown("Action") == false) {
                yield return new WaitForEndOfFrame();
            }

            // update local vars for event.
            if (data.useLocalVars) {
                data.localVars[i] = true;
            }
            
            _audio.PlaySound(0);
        }

        ClearBox();
        _anim.SetBool("Displayed", false);
        playing = false;

        if (data.useLocalVars) {
            data.ResetLocalVars();
        }

        _dialogueRoutine = null;
    }

    /// <summary>
    /// Clear dialogue box.
    /// </summary>
    public void ClearBox() 
    {
        speakerName.UpdateContent("");
        content.UpdateContent("");
        cursor.SetActive(false);
    }



    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init() 
    {
        _anim = GetComponent<Animator>();
        _audio = GetComponent<AudioComponent>();
        _rePlayer = ReInput.players.GetPlayer(playerID);
    }
}
