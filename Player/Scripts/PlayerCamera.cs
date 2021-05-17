using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerCamera : MonoBehaviour
{
    public string status = "normal";
    public bool playerControl;

    [Header("Settings")]
    public int playerID;

    private AudioComponent _audio;
    private Animator _anim;
    private Rewired.Player _rePlayer;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.inGameplay && !GameManager.instance.paused && playerControl && !Player.instance.playerController.isInteracting) {
            if (_rePlayer.GetButtonDown("Left1") || _rePlayer.GetButtonDown("Right1")) {
                if (status == "normal") {
                    ZoomIn();
                } else {
                    ZoomOut();
                }
            }
        }
    }

    /// <summary>
    /// Block camera so user cannot 
    /// zoom in - out.
    /// </summary>
    public void BlockCamera()
    {
        playerControl = false;
    }

    /// <summary>
    /// Unblock camera.
    /// </summary>
    public void UnLockCamera()
    {
        playerControl = true;
    }

    /// <summary>
    /// Zoom camera in.
    /// </summary>
    public void ZoomIn()
    {
        _anim.SetBool("Zoom", true);
        _audio.PlaySound(0);
        status = "zoom";
    }

    /// <summary>
    /// Zoom camera out.
    /// </summary>
    public void ZoomOut()
    {
        _anim.SetBool("Zoom", false);
        _audio.PlaySound(0);
        status = "normal";
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        _audio = GetComponent<AudioComponent>();
        _anim = GetComponent<Animator>();
        _rePlayer = ReInput.players.GetPlayer(playerID);
    }
}
