using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{

    [Header("Settings")]
    public float rotateSpeed;
    public int rotateChecker = 70;

    [Header("Components")]
    public Transform orientator;

    [Header("Attributes")]
    public bool lookingAtPlayer;
    public bool lookingAtSomething;

    [Header("Status")]
    public bool canMove = true;
    public bool isMoving;

    [HideInInspector]
    public Coroutine patrolRoutine;

    private Coroutine _lookAtPlayerRoutine;
    private Coroutine _lookingAtSomethingRoutine;
    private int _securityChecker = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        if ( lookingAtPlayer ) {
            RotateBugChecker();
        } 
    }

    
    /// <summary>
    /// Look at player.
    /// </summary>
    public void LookAtPlayer()
    {
        if (!lookingAtPlayer && _lookAtPlayerRoutine == null) {
            _lookAtPlayerRoutine = StartCoroutine(LookAtPlayerRoutine());
        }
    }

    /// <summary>
    /// Look at something.
    /// </summary>
    /// <param name="something">Transform - something to look at.</param>
    /// <param name="lookSpeed">float - speed to use at rotation. Default to 1f</param>
    public void LookAtSomething(Transform something, float lookSpeed = 1f)
    {
        if (!lookingAtSomething && _lookingAtSomethingRoutine == null) {
            _lookingAtSomethingRoutine = StartCoroutine(LookAtSomethingRoutine(something, lookSpeed));
        }
    }
    

    /// <summary>
    /// Check security checker.
    /// Used to avoid endless rotation if any
    /// error occurr.
    /// </summary>
    private void RotateBugChecker()
    {
        _securityChecker++;
    }


    /// <summary>
    /// Look at player Coroutine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator LookAtPlayerRoutine()
    {
        lookingAtPlayer = true;
        Vector3 targetDirection = Player.instance.gameObject.transform.position - transform.position;
        Vector3 newDirection = new Vector3();

        while(!IsFacingObject(Player.instance.gameObject)) {
            newDirection = Vector3.RotateTowards(transform.forward, targetDirection, rotateSpeed * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
            transform.rotation = new Quaternion(0f, transform.rotation.y, 0f, transform.rotation.w);

            // ensure the game never bugs itself in an endless loop.
            if ( _securityChecker > rotateChecker ) {
                break;
            }

            yield return new WaitForEndOfFrame();
        }

        lookingAtPlayer = false;
        _securityChecker = 0;
        _lookAtPlayerRoutine = null;
    }

    /// <summary>
    /// Look at something Coroutine.
    /// </summary>
    /// <param name="something">Transform - something to look at.</param>
    /// <param name="lookSpeed">float - rotation speed multiplyer. Default to 1f</param>
    /// <returns>IEnumerator</returns>
    public IEnumerator LookAtSomethingRoutine(Transform something, float lookSpeed = 1f)
    {
        lookingAtSomething = true;
        Vector3 targetDirection = something.position - transform.position;
        Vector3 newDirection = new Vector3();

        while(!IsFacingObject(something.gameObject)) {
            newDirection = Vector3.RotateTowards(transform.forward, targetDirection, (rotateSpeed * lookSpeed) * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
            transform.rotation = new Quaternion(0f, transform.rotation.y, 0f, transform.rotation.w);

            // ensure the game never bugs itself in an endless loop.
            if ( _securityChecker > rotateChecker ) {
                break;
            }

            yield return new WaitForEndOfFrame();
        }

        lookingAtSomething = false;
        _securityChecker = 0;
        _lookingAtSomethingRoutine = null;
    }

    /// <summary>
    /// Check if the NPC is looking at the player.
    /// </summary>
    /// <param name="other">GameObject - the other gameObject</param>
    /// <returns>bool</returns>
    private bool IsFacingObject(GameObject other){
        // Check if the gaze is looking at the front side of the object
        Vector3 forward = transform.forward;
        Vector3 toOther = (other.transform.position - transform.position).normalized;
    
        if (Vector3.Dot(forward, toOther) <= 0.98f){
            return false;
        }
    
        return true;
    }
}
