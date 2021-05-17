using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public string mapName;

    [Header("Components")]
    public PlayerSpawnData playerSpawnData;

    void Awake() 
    {
        if ( instance == null ) {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start() 
    {
        Init();
    }

    /// <summary>
    /// Init level scene.
    /// </summary>
    private IEnumerator InitLevelScene()
    {

        yield return new WaitForSeconds(.5f);

        GameManager.instance.overWorldUI.UICover.FadeOut(.8f);
        yield return new WaitForSeconds(.1f);

        GameManager.instance.inGameplay = true;
    }

    /// <summary>
    /// Spawn player in the set player
    /// spawn data.
    /// </summary>
    public void SpawnPlayer()
    {
        // ensure at least one active spawn is set when loading the scene.
        if (playerSpawnData.activeSpawn.name == "" || playerSpawnData.activeSpawn.name == null) {
            playerSpawnData.SetActiveSpawn(0);
        }

        // move player at the spawn location.
        Transform spawnPosition = GameObject.Find(playerSpawnData.activeSpawn.gameObjectName).GetComponent<Transform>();
        Player.instance.gameObject.transform.position = spawnPosition.position;
        Player.instance.playerController.SetOrientation(playerSpawnData.activeSpawn.orientation);

    }

    /// <summary>
    /// Init class method.
    /// </summary>
    public void Init() 
    {
        SpawnPlayer();
        StartCoroutine(InitLevelScene());
    }
}
