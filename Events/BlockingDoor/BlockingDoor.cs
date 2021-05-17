using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingDoor : Event
{
    [Header("Settings")]
    public bool opened;
    public bool useMoveCameraAnim;
    public bool useCinematic;
    public bool disableBox;
    public bool disableMesh;
    public bool useCover;
    public Vector3 openedPosition;

    [Header("Data Source")]
    public DialogueContent data;

    [Header("Components")]
    public Transform cameraPosition;
    public Transform cameraFocus;
    public GameObject cover;


    private AudioComponent _audio;
    private Animator _anim;
    private bool _canBeTriggered;
    private BoxCollider _boxCollider;
    private MeshRenderer _meshRenderer;
    private bool _movingCamera;
    private Vector3 _originalPosition;
    private bool _inAnimation;
    private Coroutine _removeRoutine;


    // Start is called before the first frame update
    void Start() {
        Init();
        CheckIfAlreadyRemoved();
    }

    // Update is called once per frame
    void Update()
    {
        if (!opened) {
            if (GameManager.instance.inGameplay && _event == null) {
                CheckIfInteracted();
            }

            
            if (CheckVars() && ! _inAnimation && _removeRoutine == null) {
                _removeRoutine = StartCoroutine(RemoveObstacle());
            }
            
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
    /// Check if has already been opened
    /// by the player.
    /// </summary>
    public void CheckIfAlreadyRemoved()
    {
        if (CheckVars()) {
            // Debug.Log("here");
            if (disableBox) {
                _boxCollider.enabled = false;
            }

            if (disableMesh) {
                _meshRenderer.enabled = false;
            }
            _anim.enabled = false;
            gameObject.transform.localPosition = openedPosition;
            
            opened = true;
        }
    }

    /// <summary>
    /// Remove obstacle.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator RemoveObstacle()
    {
        // TODO: Add remove with camera animations.
        _inAnimation = true;
        Player.instance.playerController.SetPlayerInEventMode();

        _audio.PlaySound(1);
        _anim.SetBool("Remove", true);
        yield return new WaitForSeconds(1.5f);

        Player.instance.playerController.SetPlayerOutOfEventMode();
        
        _inAnimation = false;
        opened = true;
        _removeRoutine = null;
        yield return null;
    }


    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        _audio = GetComponent<AudioComponent>();
        _boxCollider = GetComponent<BoxCollider>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _anim = GetComponent<Animator>();
        _originalPosition = gameObject.transform.position;
        
        if (!useCover) {
            cover.SetActive(false);
        }
    }
}
