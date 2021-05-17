using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDialogue : Event
{
    [Header("Data Source")]
    public DialogueContent data;
    private AudioComponent _audio;
    private bool _canBeTriggered;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.inGameplay && _event == null) {
            CheckIfInteracted();
        }
    }

    /// <summary>
    /// Check if this event
    /// is interacted.
    /// </summary>
    private void CheckIfInteracted()
    {
        inPlayerSight = Player.instance.playerLookAt.inView == this.gameObject.name;
        canBeTriggered = (interactable && inPlayerSight);

        if (canBeTriggered && Player.instance.playerController.actionPressed && _event == null ) {
            _event = StartCoroutine(TriggerEvent());
        }
    }

    /// <summary>
    /// Trigger event.
    /// </summary>
    /// <returns>IEnumerator
    public override IEnumerator TriggerEvent()
    {
        canBeTriggered = false;
        Player.instance.playerUI.iconWrapper.Hide();

        Player.instance.playerController.SetPlayerInEventMode();

        _audio.PlaySound(0);
        
        // set up dialogue data.
        GameManager.instance.overWorldUI.dialogueBox.data = data;

        // play dialogue.
        GameManager.instance.overWorldUI.dialogueBox.Play();

        while (GameManager.instance.overWorldUI.dialogueBox.playing) {
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(.1f);
        Player.instance.playerController.SetPlayerOutOfEventMode();
        yield return new WaitForSeconds(.5f);

        canBeTriggered = true;
        _event = null;
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        _audio = GetComponent<AudioComponent>();
    }
    
}
