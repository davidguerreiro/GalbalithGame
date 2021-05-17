using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorToSame : Event
{

    [Header("Settings")]
    public Transform spawnToUse;
    public PlayerSpawnData.Orientation orientation;

    private AudioComponent _audio;

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

        if (canBeTriggered && _event == null ) {
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

        GameManager.instance.overWorldUI.UICover.FadeIn();
        Player.instance.playerUI.iconWrapper.Hide();

        Player.instance.playerController.SetPlayerInEventMode();

        _audio.PlaySound(0);

        yield return new WaitForSeconds(1.5f);
        Player.instance.gameObject.transform.position = spawnToUse.position;
        Player.instance.playerController.SetOrientation(orientation);

        
        yield return new WaitForSeconds(.5f);
        
        GameManager.instance.overWorldUI.UICover.FadeOut();
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
        GetComponent<MeshRenderer>().enabled = false;
    }
}
