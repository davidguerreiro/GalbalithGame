using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : Event
{
    [Header("Data Source")]
    public DialogueContent data;

    [Header("Settings")]
    public bool lookAtPlayer;
    public bool returnLook;
    private AudioComponent _audio;
    private bool _canBeTriggered;
    private NPC _npc;

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
        _canBeTriggered = (interactable && Player.instance.playerLookAt.inView == this.gameObject.name);

        if (_canBeTriggered && Player.instance.playerController.actionPressed && _event == null ) {
            _event = StartCoroutine(TriggerEvent());
        }
    }

    /// <summary>
    /// Trigger event.
    /// </summary>
    /// <returns>IEnumerator
    public override IEnumerator TriggerEvent()
    {
        // TODO: Add NPC looks at the player.
        _canBeTriggered = false;

        Player.instance.playerController.SetPlayerInEventMode();

        _audio.PlaySound(0);

        if (lookAtPlayer) {
            _npc.LookAtPlayer();

            while (_npc.lookingAtPlayer) {
                yield return new WaitForEndOfFrame();
            }
        }
        
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

        _canBeTriggered = true;
        _event = null;
    }



    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        _audio = GetComponent<AudioComponent>();
        _npc = GetComponent<NPC>();
    }
}
