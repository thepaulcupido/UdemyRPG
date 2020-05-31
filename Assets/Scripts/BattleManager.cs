using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    public GameObject battleScene;
    public Transform[] playerPositions;
    public Transform[] transformPositions;

    public BattleCharacter[] playerPrefabs;
    public BattleCharacter[] enemyPrefabs;

    // private variables
    private bool isBattleActive;

    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N)) {
            string[] enemies = {"Skeleton", "Spider"};
            BattleStart(enemies);
        }
    }

    public void BattleStart(string[] enemiesToSpawn)
    {
        if (!isBattleActive) {
            
            isBattleActive = GameManager.instance.isBattleActive = true;
            transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
            battleScene.SetActive(true);
            AudioManager.instance.PlayBackgroundMusic(0);
        }
    }
}
