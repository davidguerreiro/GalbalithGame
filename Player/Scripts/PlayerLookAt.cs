using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookAt : MonoBehaviour
{
    public string inView;

    [Header("Settings")]
    public float viewDistance;
    public string nullValue = "none";

    private RaycastHit _hit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.inGameplay) {
            TriggerRayCast();
        }
    }

    /// <summary>
    /// Trigger raycast and update inView.
    /// </summary>
    private void TriggerRayCast()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out _hit, viewDistance)) {
            if (Array.IndexOf(Player.instance.interactables, _hit.transform.gameObject.tag) != -1) {
                inView = _hit.transform.gameObject.name;
            } else {
                inView = nullValue;
            }
        } else {
            inView = nullValue;
        } 
    }

}
