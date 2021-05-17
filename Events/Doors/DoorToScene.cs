using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorToScene : Event
{
    [Header("Settings")]
    public PlayerSpawnData levelSpawnData;
    public string sceneToLoadName;
    public int spawnToUse;

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

        // set which spawn data will be used in the next scene.
        levelSpawnData.SetActiveSpawn(spawnToUse);

        _audio.PlaySound(0);

        // decrease level music.
        StartCoroutine(GameManager.instance.levelMusicManager.audio.FadeOutSongRoutine());
        yield return new WaitForSeconds(2f);
        
        // load new scene.
        canBeTriggered = true;
        SceneManager.LoadScene(sceneToLoadName);

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
