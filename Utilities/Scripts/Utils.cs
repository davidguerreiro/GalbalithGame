using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Utils : MonoBehaviour
{
    public static Utils instance;                                      // Class instance.
    public CursorManager cursorManager;                         // Cursor manager class reference.

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake() {
        if ( instance == null ) {
            instance = this;
        }
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        Init();
    }

    private void Update() {
        // Quit game when pressing escape key.
        if ( Input.GetKeyDown( "escape" ) ) {
            QuitGame();
        }
    }

    /// <summary>
    /// Quit game.
    /// </summary>
    private void QuitGame() {
        Application.Quit();
    }

    /// <summary>
    /// Remove mesh for object which are only
    /// meant to be visible in the editor.
    /// </summary>
    public void DisableMesh()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("NoMesh");

        foreach (GameObject noMeshObject in objects) {
            noMeshObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    /// <summary>
    /// Load scene.
    /// </summary>
    /// <param name="sceneName">string - scene name</param>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init() 
    {
        DisableMesh();
    }

}
