using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    public PlayerData data;

    public string[] interactables;

    public Transform playerTransform;

    void Awake() {
        if ( instance == null ) {
            instance = this;
        }
    }

    private void Start() {
        // init game played count.
        StartCoroutine(data.UpdateGameTimePlayed());
    }
    
    [Header("Components")]
    public PlayerController playerController;
    public PlayerLookAt playerLookAt;
    public PlayerUI playerUI;
    public PlayerCamera playerCamera;

    private void OnTriggerEnter(Collider other) {
        if (Array.IndexOf(interactables, other.gameObject.tag) != -1) {
            other.gameObject.GetComponent<Event>().interactable = true;
        }
    }

    private void OnTriggerStay(Collider other) {
        if (Array.IndexOf(interactables, other.gameObject.tag) != -1) {
            if (other.gameObject.GetComponent<Event>().inPlayerSight && other.gameObject.GetComponent<Event>().canBeTriggered) {
                playerUI.iconWrapper.Display();
            } else {
                playerUI.iconWrapper.Hide();
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (Array.IndexOf(interactables, other.gameObject.tag) != -1) {
            other.gameObject.GetComponent<Event>().interactable = false;

            if (playerUI.iconWrapper.displayed) {
                playerUI.iconWrapper.Hide();
            }
        }
    }
}
