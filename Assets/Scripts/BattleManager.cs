using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    public GameObject battleScene;
    public Transform[] playerPositions;
    public Transform[] enemyPositions;

    public BattleCharacter[] playerPrefabs;
    public BattleCharacter[] enemyPrefabs;
    public List<BattleCharacter> activeBattlers = new List<BattleCharacter>();

    public int currentTurn = 0;
    public bool isTurnWaiting = true;
    public GameObject UIButtonMenuHolder;

    public BattleMove[] movesList;

    public GameObject enmeyAttackEffect;

    public DamageCalculations damageNumber;

    public Text[] playerName, playerHp, playerMp;
    public GameObject targetMenu;
    public BattleTargetButton[] targetButtons;

    public GameObject magicMenu;
    public BattleMagicSelection[] magicButtons;

    public BattleNotification notification;
    public int chanceToFlee = 35;

    public GameObject itemMenu;
    public ItemButton[] itemButtons;
    public Item selectedItem;
    public GameObject itemApplicationMenu;
    public PlayerItemApplication[] activePlayers;
    
    public GameObject[] playerContainer;
    public BattleCharacter selectedPlayer;

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

            UpdateUIStats();
        }
    }

    public void NextTurn()
    {
        currentTurn = (currentTurn >= activeBattlers.Count - 1) ? 0 : currentTurn+1;
        isTurnWaiting = true;
        UpdateBattle();
        UpdateUIStats();
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

            // skipping turns for dead battlers
            while (activeBattlers[currentTurn].currentHp == 0) {
                //todo: turn off the objects representing the dead characters
                currentTurn++;
                if (currentTurn >= activeBattlers.Count) {
                    currentTurn = 0;
                }
            }

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

        Instantiate(damageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetDamage(Mathf.RoundToInt(damageCalculation));
        UpdateUIStats();
    }

    public void UpdateUIStats()
    {
        for (int i = 0 ; i < playerName.Length; i++) {
            if (activeBattlers.Count > i) {
                if (activeBattlers[i].isPlayer) {
                    BattleCharacter playerData = activeBattlers[i];
                    playerName[i].text = playerData.characterName;
                    playerHp[i].text = Mathf.Clamp(playerData.currentHp, 0, int.MaxValue) + "/" + playerData.maxHp;
                    playerMp[i].text = Mathf.Clamp(playerData.currentMp, 0, int.MaxValue) + "/" + playerData.maxMp;
                }
            } 
            playerName[i].gameObject.SetActive(activeBattlers[i].isPlayer && activeBattlers.Count > i);
        }
    }

    public void PlayerAttack(string moveName, int selectedTarget)
    {
        //int selectedTarget = 2;
        int movePower =0;
        for (int i = 0; i < movesList.Length; i++) {
            if (movesList[i].moveName == moveName) {
                movePower = movesList[i].movePower;
                Instantiate(movesList[i].effect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
            }
        }

        Instantiate(enmeyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);
        DealDamage(selectedTarget, movePower);

        targetMenu.SetActive(false);
        
        UIButtonMenuHolder.SetActive(false);
        NextTurn();
    }

    public void OpenTargetMenu(string moveName)
    {
        // activate menu
        targetMenu.SetActive(true);

        List<int> enemyIndices = new List<int>();
        // update active battler target name
        for (int i =0; i < activeBattlers.Count; i++) {
            if (!activeBattlers[i].isPlayer) {
                enemyIndices.Add(i);
            }
        }

        for (int i = 0; i < targetButtons.Length; i++) {

            targetButtons[i].gameObject.SetActive(false);

            if (enemyIndices.Count > i) {
                targetButtons[i].gameObject.SetActive(true);
                targetButtons[i].moveName = moveName;
                targetButtons[i].activeBattlerTargetIndex = enemyIndices[i];
                targetButtons[i].targetName.text = activeBattlers[enemyIndices[i]].characterName;
            }
        }
    }

    public void OpenMagicMenu()
    {
        magicMenu.SetActive(true);

        for (int i = 0; i < magicButtons.Length; i ++) {

            magicButtons[i].gameObject.SetActive(false);

            if (activeBattlers[currentTurn].availableMoves.Length > i) {
                magicButtons[i].gameObject.SetActive(true);

                magicButtons[i].spellName = activeBattlers[currentTurn].availableMoves[i];
                magicButtons[i].nameText.text = activeBattlers[currentTurn].availableMoves[i];

                for (int j = 0; j < movesList.Length; j++) {
                    if (movesList[j].moveName == magicButtons[i].spellName) {
                        magicButtons[i].manaCost = movesList[j].moveCost;
                        magicButtons[i].costText.text = magicButtons[i].manaCost.ToString();
                    }
                }
            }
        }
    }

    public void OpenItemMenu()
    {       
        itemMenu.SetActive(true);

        bool isButtonActive = false;
        Item item;

        for (int i = 0; i < itemButtons.Length; i++) {
            isButtonActive = GameManager.instance.itemsHeld[i] != "" || GameManager.instance.numberOfItems[i] > 0;

            if (isButtonActive) {
                item = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]);
                itemButtons[i].buttonValue = i;
                itemButtons[i].amountText.text = GameManager.instance.numberOfItems[i].ToString();
                itemButtons[i].buttonImage.sprite = item.itemSprite;
            }

            itemButtons[i].gameObject.SetActive(isButtonActive);
        }
    }

    public void OpenItemApplicationMenu()
    {
        itemApplicationMenu.SetActive(true);

        for (int j = 0; j < playerContainer.Length; j++) {

            playerContainer[j].SetActive(false);

            for (int i = 0; i < activeBattlers.Count; i++) {
                if (activeBattlers[i].isPlayer && activePlayers[j].playerName == activeBattlers[i].characterName) {
                    playerContainer[j].SetActive(true);
                }
            }
        }

    }

    public void SelectItem(Item item)
    {
        selectedItem = item;

        itemMenu.SetActive(false);
        OpenItemApplicationMenu();
        
    }

    public void SelectPlayer(BattleCharacter player)
    {
        selectedPlayer = player;
        selectedItem.Use(player);

        itemApplicationMenu.SetActive(false);

        selectedItem = null;
        selectedPlayer = null;

        NextTurn();
    }

    public void UseItem()
    {
        if (selectedItem != null && selectedPlayer != null) {

            for (int i = 0; i < activeBattlers.Count; i++) {
                if (activeBattlers[i].isPlayer && activePlayers[i].playerName == selectedPlayer.characterName) {

                    // apply the item's effects here

                    selectedItem.Use(activeBattlers[i]);
                    
                    itemApplicationMenu.SetActive(false);

                }
            }

            selectedItem = null;
            selectedPlayer = null;
        }

    }

    public void Flee()
    {
        int fleeSuccess = Random.Range(0,100);
        if (fleeSuccess < chanceToFlee) {
            // end the battle
            isBattleActive = false;
            battleScene.SetActive(false);
        } else {
            notification.description.text = "Could not escape!";
            notification.Activate();
            // should have a co-routine to wait for however long the description is on screen for
            NextTurn();
        }
    }
}
