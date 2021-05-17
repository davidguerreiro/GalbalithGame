using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class ClimbingStairs : Event
{
    [Header("Settings")]
    public float climbingSpeed;
    public float enterExistSpeed;
    public int playerID;
    public string position;

    [Header("Components")]
    public Transform bottomFloor;
    public Transform bottomStair;
    public Transform topFloor;
    public Transform topStair;
    public BoxCollider bottomCollider;
    public BoxCollider topCollider;

    [HideInInspector]
    public bool readyToLeave;
    [HideInInspector]
    public bool climbing;
    [HideInInspector]
    public bool movingToPoint;
    [HideInInspector]
    public string leavingFrom;

    private AudioComponent _audio;
    private bool _canBeTriggered;
    private Rewired.Player _rePlayer;
    private bool _enterFromDown;
    private bool _isMoving;
    private Coroutine _movingRoutine;
    

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

        if (_event != null && !movingToPoint && climbing && !Player.instance.playerController.canMove ) {
            Climb();

            if (readyToLeave && _rePlayer.GetButtonDown("Action")) {
                StartCoroutine(MovePlayerOut());
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
    /// <returns>IEnumerator</returns>
    public override IEnumerator TriggerEvent()
    {
        canBeTriggered = false;
        Player.instance.playerUI.iconWrapper.Hide();

        Player.instance.playerController.SetPlayerInEventMode();

        topCollider.enabled = false;
        bottomCollider.enabled = false;

        _audio.PlaySound(0);

        // _enterFromDown = IsEnteringFromDown();
        
        Vector3 destination = (position == "bottom") ? bottomStair.position : topStair.position;
        Player.instance.playerController.SetOrientation(PlayerSpawnData.Orientation.up);

        if ( _movingRoutine == null ) {
            _movingRoutine = StartCoroutine(MovePlayerToStairPoint(destination));
        }

        while (movingToPoint && _movingRoutine != null) {
            yield return new WaitForFixedUpdate();
        }

        climbing = true;

        // enable climbing controls here.
        while (climbing) {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(.1f);
        Player.instance.playerController.SetPlayerOutOfEventMode();

        topCollider.enabled = true;
        bottomCollider.enabled = true;

        yield return new WaitForSeconds(.5f);

        canBeTriggered = true;
        _event = null;
    }

    /// <summary>
    /// Get enter point.
    /// If true, player starts
    /// climbing from downstairs.
    /// </summary>
    /// <returns>bool</returns>
    private bool IsEnteringFromDown()
    {
        if (topStair.position.y > Player.instance.gameObject.transform.position.y) {
            return true;
        } else {
            return false;
        }
    } 

    /// <summary>
    /// Move player to point.
    /// </summary>
    /// <param name="destination">Vector3 - destination object</param>
    /// <returns>IEnumerator</returns>
    private IEnumerator MovePlayerToStairPoint(Vector3 destination)
    {
        movingToPoint = true;
        while (Vector3.Distance(Player.instance.transform.position, destination) > Mathf.Epsilon) {
            Player.instance.transform.position = Vector3.MoveTowards(Player.instance.transform.position, destination, enterExistSpeed * Time.deltaTime);
            
            yield return new WaitForFixedUpdate();
        }

        movingToPoint = false;
        _movingRoutine = null;
    }

    /// <summary>
    /// Climbing controls to move player
    /// up and down in the stair.
    /// </summary>
    private void Climb()
    {
        float deltaY = _rePlayer.GetAxis("MoveVertical") * climbingSpeed;
        // check limits.
        if ( (deltaY > 0f && Player.instance.playerTransform.position.y > topStair.position.y) || (deltaY < 0f && Player.instance.playerTransform.position.y < bottomStair.position.y) ) {
            deltaY = 0f;
        }

        Vector3 movement = new Vector3(0f, deltaY, 0f);

        _isMoving = (movement.magnitude > 0f) ? true : false;
        
        Player.instance.playerController.controller.Move(movement * Time.deltaTime);
    }

    /// <summary>
    /// Move player out.
    /// </summary>
    public IEnumerator MovePlayerOut() 
    {
        readyToLeave = false;

        Vector3 destination = (leavingFrom == "top") ? topFloor.position : bottomFloor.position;

        _audio.PlaySound(0);

        if (_movingRoutine == null) {
            _movingRoutine = StartCoroutine(MovePlayerToStairPoint(destination));
        }

        while (movingToPoint && _movingRoutine != null) {
            yield return new WaitForFixedUpdate();
        }

        climbing = false;
    }


    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        _audio = GetComponent<AudioComponent>();
        _rePlayer = ReInput.players.GetPlayer(playerID);

        GetComponent<MeshRenderer>().enabled = false;
    }

    
}
