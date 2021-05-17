using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 5f;
    public float gravity = -9.8f;

    [Header("Status")]
    public bool canMove;
    public bool canInteract;
    public bool isMoving;
    public bool isInteracting;

    [Header("Rewired Controller")]
    public int playerID;

    [Header("Interactions")]
    public bool actionPressed;

    [Header("Components")]
    public Animator animator;
    public Transform orientator;

    [HideInInspector]
    public CharacterController controller;

    private Rewired.Player _rePlayer;
    private Transform _model;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.paused) {

            if ( GameManager.instance.inGameplay ) {

                if (canMove && !isInteracting) {
                    MovePlayer();
                    OrientateModel();
                }

                if (canInteract) {
                    Interact();
                }
            }
            
            AnimationController();
        }
    }

    /// <summary>
    /// Move player.
    /// </summary>
    private void MovePlayer()
    {
        float deltaZ = -(_rePlayer.GetAxis("MoveHorizontal") * speed);
        float deltaX = _rePlayer.GetAxis("MoveVertical") * speed;

        Vector3 movement = new Vector3(deltaX, 0f, deltaZ);
        movement = Vector3.ClampMagnitude(movement, speed);

        this.isMoving = (movement.magnitude > 0f) ? true : false;

        movement.y = gravity;

        orientator.localPosition = new Vector3(movement.z, 0f, -movement.x);
        controller.Move(movement * Time.deltaTime);
    }

    /// <summary>
    /// Animation controller.
    /// </summary>
    private void AnimationController() {
        if (isMoving) {
            animator.SetBool("Moving", true);
        } else {
            animator.SetBool("Moving", false);
        }
    }

    /// <summary>
    /// Interact with the enviroment
    /// using the action button.
    /// </summary>
    private void Interact()
    {
        actionPressed = _rePlayer.GetButtonDown("Action");
    }

    /// <summary>
    /// Update orientation.
    /// </summary>
    private void OrientateModel()
    {
        _model.LookAt(orientator);
    }

    /// <summary>
    /// Set player in event mode, so no movement
    /// controls or interact with events.
    /// </summary>
    public void SetPlayerInEventMode()
    {
        canMove = false;
        canInteract = false;
        isMoving = false;

        GameManager.instance.LockGameMenu();
    }

    /// <summary>
    /// Set player out of event mode.
    /// </summary>
    public void SetPlayerOutOfEventMode()
    {
        canMove = true;
        canInteract = true;

        GameManager.instance.UnlockGameMenu();
    }

    /// <summary>
    /// Disable player controller
    /// component.
    /// </sumamry>
    public void DisablePlayerController()
    {
        controller.enabled = false;
    }

    /// <summary>
    /// Enabled player controller component.
    /// </summary>
    public void EnablePlayerController()
    {
        controller.enabled = true;
    }

    /// <summary>
    /// Set orientation.
    /// This methid is usually called when
    /// the player is spawned.
    /// </summary>
    public void SetOrientation(PlayerSpawnData.Orientation orientation)
    {
        Quaternion modelRotation;

        switch (orientation) {
            case PlayerSpawnData.Orientation.up:
                modelRotation = Quaternion.Euler(0f, 180f, 0f);
                break;
            case PlayerSpawnData.Orientation.down:
                modelRotation = Quaternion.Euler(0f, -1.073f, 0f);
                break;
            case PlayerSpawnData.Orientation.left:
                modelRotation = Quaternion.Euler(0f, 90f, 0f);
                break;
            case PlayerSpawnData.Orientation.right:
                modelRotation = Quaternion.Euler(0f, -90f, 0f);
                break;
            default:
                modelRotation = Quaternion.Euler(0f, 180f, 0f);
                break;
        }

        if (_model == null) {
            _model = animator.gameObject.transform;
        }

        _model.localRotation = modelRotation;
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        controller = GetComponent<CharacterController>();
        _model = animator.gameObject.transform;
        _rePlayer = ReInput.players.GetPlayer(playerID);
    }
}
