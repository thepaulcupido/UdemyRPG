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

    public int currentTurn = 0; // why is this public?
    public bool isTurnWaiting = true;
    public GameObject UIButtonMenuHolder;

    public BattleMove[] movesList;

    public GameObject enmeyAttackEffect;

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

        if (isBattleActive) {
            if (isTurnWaiting) {
                UIButtonMenuHolder.SetActive(activeBattlers[currentTurn].isPlayer);
                if (!activeBattlers[currentTurn].isPlayer) {
                    StartCoroutine(EnemyMoveCoroutine());
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.M)) {
            NextTurn();
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

    public void NextTurn()
    {
        currentTurn = (currentTurn >= activeBattlers.Count - 1) ? 0 : currentTurn+1;
        isTurnWaiting = true;
        UpdateBattle();
    }

    public void UpdateBattle()
    {
        bool allEnemiesDead = true;
        bool allPlayersDead = true;

        for (int x = 0; x < activeBattlers.Count; x++) {
            if (activeBattlers[x].isAlive && activeBattlers[x].currentHp > 0) {
                if (activeBattlers[x].isPlayer) {
                    allPlayersDead = false;
                } else {
                    allEnemiesDead = false;
                }
            } else {
                activeBattlers[x].currentHp = 0;
                activeBattlers[x].isAlive = false;
            }
        }

        if (!allEnemiesDead && !allPlayersDead) {
            return;
        }

        if (allEnemiesDead) {
            // battle ends in victory
        } else {
            // game over - party was defeated
        }


        EndBattle();
    }

    //todo: destroy all battlers and remove superfluous game objects
    private void EndBattle()
    {
                
        // for (int x = 0; x < activeBattlers.Count; x++) {
        //     Destroy(GameObject.Find("Player1(Clone)"));
        // }

        // activeBattlers.Clear();

        battleScene.SetActive(false);
        GameManager.instance.isBattleActive = false;
        isBattleActive = false;
    }

    public IEnumerator EnemyMoveCoroutine()
    {
        // Co-routine - works on a different thread to the main program
        isTurnWaiting = false;

        yield return new WaitForSeconds(1f);
        EnemyAttack();

        yield return new WaitForSeconds(1f);
        NextTurn();
    }

    public void EnemyAttack()
    {
        List<int> players = new List<int>();
        for (int i  =0; i < activeBattlers.Count; i ++) {
            if (activeBattlers[i].isPlayer && activeBattlers[i].currentHp > 0) {
                players.Add(i);
            }
        }

        // Enemy AI logic here
        int selectedTarget = players[Random.Range(0, players.Count)];
        int movePower = 0;

        int selectAttack = Random.Range(0, activeBattlers[currentTurn].availableMoves.Length-1);
        for (int i = 0; i < movesList.Length; i++) {
            if (movesList[i].moveName == activeBattlers[currentTurn].availableMoves[selectAttack]) {
                movePower = movesList[i].movePower;
                Instantiate(movesList[i].effect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
            }
        }

        Instantiate(enmeyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);

        DealDamage(selectedTarget, movePower);
    }

    public void DealDamage(int target, int movePower)
    {
        float attackPower = activeBattlers[currentTurn].strength + activeBattlers[currentTurn].weaponPower;
        float defencePower = activeBattlers[target].defence + activeBattlers[target].armourPower;

        float damageCalculation = (attackPower / defencePower) * movePower * Random.Range(0.9f, 1.1f);
        activeBattlers[target].currentHp -= Mathf.RoundToInt(damageCalculation);
        Debug.Log(activeBattlers[currentTurn].characterName + " is dealing " + Mathf.RoundToInt(damageCalculation) + " damage");
    }


}
