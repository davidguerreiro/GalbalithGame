using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPatrol : NPC
{
    public enum MovementType {
        random,
        defined,
    }

    [Header("Settings")]
    public MovementType movementType;
    public float speed;
    public float awaitBeforeReset;
    public string[] stoppersTags;
    public int[] stoppersLayers;

    [Header("Components")]
    public Animator animator;

    [Header("Random Settings")]
    public Transform offset;
    public Vector3 ranges;
    public enum Frecuency {
        low,
        medium,
        hight,
    }

    public Frecuency frecuency;

    [Header("Defined Settings")]
    public GameObject definedOffsetsWrapper;
    public Transform[] offsets;
    public enum Type {
        linear,
        loop,
    };

    public Type type;
    public float timeAwaitInStation;

    [Header("State")]
    public bool inPatrol;

    private Vector3 _offsetOriginalPosition;
    private int _station;
    private bool _stopped;
    private Coroutine _restartPatrolRoutine;
    private Rigidbody _rigi;



    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfTriggerPatrol();
        if (inPatrol) {
            AnimationCotroller();
        }
    }

    /// <summary>
    /// Check if patrol has to be triggered.
    /// </summary>
    private void CheckIfTriggerPatrol()
    {
        if (inPatrol && canMove && patrolRoutine == null && !_stopped) {
            if (movementType == MovementType.random) {
                patrolRoutine = StartCoroutine(RandomMovementCoroutine());
            }

            if (movementType == MovementType.defined) {
                patrolRoutine = StartCoroutine(DefinedMovementCoroutine());
            }
        }
    }

    /// <summary>
    /// Animation controller.
    /// </summary>
    private void AnimationCotroller()
    {
        if (isMoving) {
            animator.SetBool("Moving", true);
        } else {
            animator.SetBool("Moving", false);
        }
    }

    /// <summary>
    /// Random movement coroutine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator RandomMovementCoroutine()
    {
        Vector2 frecuencyRange = GetFrecuencyRanges();
        float toWait = UnityEngine.Random.Range(frecuencyRange.x, frecuencyRange.y);
        yield return new WaitForSeconds(toWait);

        // set movement position.
        SetOffSet();

        // look at offset.
        LookAtSomething(offset.transform, 3f);

        while (lookingAtSomething) {
            yield return new WaitForFixedUpdate();
        }

        transform.LookAt(offset.transform);

        // move NPC to offset.
        while (Vector3.Distance(transform.position, offset.position) >= .8f) {
            isMoving = true;
            transform.position = Vector3.MoveTowards(transform.position, offset.position, speed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        isMoving = false;
        
        offset.transform.position = _offsetOriginalPosition;

        // remove this later.
        yield return new WaitForSeconds(1f);
        patrolRoutine = null;
    }

    /// <summary>
    /// Random movement coroutine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator DefinedMovementCoroutine()
    {
        for (int i = 0; i < offsets.Length; i++) {
            yield return new WaitForSeconds(timeAwaitInStation);
            LookAtSomething(offsets[i], 3f);

            while (lookingAtSomething) {
                yield return new WaitForFixedUpdate();
            }
            
            transform.LookAt(offset.transform);

            // move NPC to offset.
            while (Vector3.Distance(transform.position, offsets[i].position) >= .8f) {
                isMoving = true;
                transform.position = Vector3.MoveTowards(transform.position, offsets[i].position, speed * Time.deltaTime);
                yield return new WaitForFixedUpdate();
            }

            isMoving = false;

        }
        
        if (type == Type.linear) {
            Array.Reverse(offsets);
        }

        patrolRoutine = null;
    }



    /// <summary>
    /// Set offset in scene.
    /// </summary>
    private void SetOffSet()
    {
        float x = UnityEngine.Random.Range(-ranges.x, ranges.x);
        float z = UnityEngine.Random.Range(-ranges.z, ranges.z);
        offset.transform.position = new Vector3(offset.transform.position.x + x, transform.position.y,offset.transform.position.z + z);
        offset.transform.parent = null;
    }

    /// <summary>
    /// Get frecuency ranges.
    /// </summary>
    /// <returns>Vector2</returns>
    private Vector2 GetFrecuencyRanges()
    {
        Vector2 baseRanges = new Vector2(4f, 6f);

        switch (frecuency) {
            case Frecuency.low:
                baseRanges = new Vector2(8f, 11f);
                break;
            case Frecuency.medium:
                baseRanges = new Vector2(5f, 8f);
                break;
            case Frecuency.hight:
                baseRanges = new Vector2(2f, 5f);
                break;
        }

        return baseRanges;
    }

    /// <summary>
    /// Stop patrol.
    /// </summary>
    public void StopPatrol()
    {
        if (patrolRoutine != null) {
            StopCoroutine(patrolRoutine);
            isMoving = false;
            lookingAtSomething = false;
            lookingAtPlayer = false;
            patrolRoutine = null;

            if (movementType == MovementType.random) {
                offset.transform.position = _offsetOriginalPosition;
            }
        }
    }

    /// <summary>
    /// Check if restart patrol.
    /// </summary>
    /// <parma name="restart">bool - if true, the patrol will be restarted later.</param>
    public IEnumerator CheckIfRestartPatrol(bool restart = false)
    {
        StopPatrol();

        yield return new WaitForSeconds(awaitBeforeReset);
        _stopped = false;
        _restartPatrolRoutine = null;

    }

    private void OnTriggerEnter(Collider other) {
        if ( Array.IndexOf(stoppersTags, other.gameObject.tag) != -1 || Array.IndexOf(stoppersLayers, other.gameObject.layer) != -1) {
            _stopped = true;

            if (_restartPatrolRoutine == null) {
                
                if (other.gameObject.tag == "Player") {
                    StopPatrol();
                } else {
                    _restartPatrolRoutine = StartCoroutine(CheckIfRestartPatrol(true));
                }
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "Player") {
            _rigi.isKinematic = true;
            _stopped = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (_stopped && other.gameObject.tag == "Player") {
            _rigi.isKinematic = false;
            _stopped = false;
            _restartPatrolRoutine = null;
        }
    }
    

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        _rigi = GetComponent<Rigidbody>();

        if (movementType == MovementType.random) {
            _offsetOriginalPosition = offset.transform.position;
        }

        if (movementType == MovementType.defined) {
            definedOffsetsWrapper.transform.parent = null;
        }
        
        CheckIfTriggerPatrol();
    }
}
