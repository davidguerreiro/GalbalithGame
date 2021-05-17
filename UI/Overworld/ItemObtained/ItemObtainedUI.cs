using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class ItemObtainedUI : MonoBehaviour
{
    public bool displayed;
    public enum Type {
        gold,
        item,
    };

    public Type type;

    [Header("Components")]
    public FadeElement title;
    public FadeElement glowingCircle;
    public Image itemIcon;
    public Animator itemIconAnim;
    
    [Header("Gold Components")]
    public FadeElement goldAnim;
    public FadeElement coinNameAnim;
    public Sprite goldSprite;
    public TextComponent goldText;

    [Header("Item Components")]
    public Animator itemTextAnim;
    public TextComponent itemText;

    [Header("Settings")]
    public int playerID;

    private Animator _anim;
    private AudioComponent _audio;
    private Coroutine _inDisplay;
    private bool _readyToClose;

    private Rewired.Player _rePlayer;


    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Update() {
        if (displayed && _readyToClose && _inDisplay == null) {
            ListenForPlayerInput();
        }
    }

    /// <summary>
    /// Display gold notification.
    /// </summary>
    /// <param name="amount">string - display amount</param>
    public void DisplayGoldNotification(string amount)
    {
        if (_inDisplay == null) {
            _inDisplay = StartCoroutine(DisplayGoldNotificationRoutine(amount));
        }
    }

    /// <summary>
    /// Display gold notification.
    /// </summary>
    /// <param name="amount">string - display amount</param>
    /// <returns>IEnumerator</returns>
    public IEnumerator DisplayGoldNotificationRoutine(string amount)
    {
        displayed = true;
        goldText.UpdateContent(amount);
        itemIcon.sprite = goldSprite;

        _anim.SetBool("Display", true);
        yield return new WaitForSeconds(.5f);

        title.FadeIn();
        glowingCircle.FadeIn();
        yield return new WaitForSeconds(.1f);

        itemIconAnim.SetBool("Display", true);
        _audio.PlaySound(0);
        yield return new WaitForSeconds(.1f);

        goldAnim.FadeIn();
        coinNameAnim.FadeIn();

        _readyToClose = true;
        _inDisplay = null;   
    }

    /// <summary>
    /// Listen for player close input.
    /// </summary>
    private void ListenForPlayerInput()
    {
        if ( _rePlayer.GetButtonDown("Action") || _rePlayer.GetButtonDown("Cancel") ) {
            _inDisplay = StartCoroutine(CloseGoldenNotification());
        }
    }

    /// <summary>
    /// Close golden notification.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator CloseGoldenNotification()
    {
        _readyToClose = false;

        goldAnim.FadeOut(2f);
        coinNameAnim.FadeOut(2f);

        itemIconAnim.SetBool("Display", false);
        glowingCircle.FadeOut(2f);
        title.FadeOut(2f);
        yield return new WaitForSeconds(.5f);

        _anim.SetBool("Displayed", false);

        displayed = false;
        _inDisplay = null;
    }


    public void Init()
    {
        _anim = GetComponent<Animator>();
        _audio = GetComponent<AudioComponent>();
        _rePlayer = ReInput.players.GetPlayer(playerID);
    }

}
