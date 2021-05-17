using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIconWrapper : MonoBehaviour
{
    public bool displayed;

    [Header("Components")]
    public GameObject icons;

    private Animator _anim;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        Init();
    }

    /// <summary>
    /// Display.
    /// </summary>
    public void Display()
    {
        _anim.SetBool("Displayed", true);
        displayed = true;
    }

    /// <summary>
    /// Hide.
    /// </summary>
    public void Hide()
    {
        _anim.SetBool("Displayed", false);
        displayed = false;
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init() 
    {
        _anim = GetComponent<Animator>();
    }
}
