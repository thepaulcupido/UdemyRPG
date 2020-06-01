using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    public GameObject battleScene;
    public Transform[] playerPositions;
    public Transform[] enemyPositions;

    public BattleCharacter[] playerPrefabs;
    public BattleCharacter[] enemyPrefabs;
    public List<BattleCharacter> activeBattlers = new List<BattleCharacter>();

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
            string[] enemies = {"Skeleton", "Eyeball"};
            BattleStart(enemies);
        }
    }

    public void BattleStart(string[] enemiesToSpawn)
    {
        if (!isBattleActive) {
            
            isBattleActive = GameManager.instance.isBattleActive = true;
            battleScene.SetActive(true);
            AudioManager.instance.PlayBackgroundMusic(0);

            Vector3 newPosition = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
            transform.position = newPosition;

            CharacterStats playerObject;
            BattleCharacter newPlayer;
            int index = 0;

            for (int i = 0; i < playerPositions.Length; i++) {
                playerObject = GameManager.instance.playerStats[i];
                if (playerObject.gameObject.active) {
                    for (int j = 0; j < playerPrefabs.Length; j++) {
                        if (playerPrefabs[j].characterName == playerObject.characterName) {
                            newPlayer = Instantiate(playerPrefabs[j], playerPositions[i].position, playerPositions[i].rotation);
                            //newPlayer.transform.parent = playerPositions[i];
                            activeBattlers.Add(newPlayer);
                            index = activeBattlers.Count - 1;

                            activeBattlers[index].maxHp = playerObject.maxHP;
                            activeBattlers[index].currentHp = playerObject.currentHP;
                            activeBattlers[index].maxMp = playerObject.maxMP;
                            activeBattlers[index].currentMp = playerObject.currentMp;
                            activeBattlers[index].strength = playerObject.strength;
                            activeBattlers[index].defence = playerObject.defence;
                            activeBattlers[index].weaponPower = playerObject.weaponPower;
                            activeBattlers[index].armourPower = playerObject.armourPower;
                        }
                    }
                }
            }

            for (int i = 0; i < enemiesToSpawn.Length; i++) {
                if (enemiesToSpawn[i] != "") {
                    
                    for (int j = 0; j < enemyPrefabs.Length; j++) {
                        if (enemyPrefabs[j].characterName == enemiesToSpawn[i]) {
                            BattleCharacter newEnemy = Instantiate(enemyPrefabs[j], enemyPositions[i].position, enemyPositions[i].rotation);

                            activeBattlers.Add(newEnemy);
                            index = activeBattlers.Count - 1;
                        }
                    }
                }
            }
        }
    }
}
