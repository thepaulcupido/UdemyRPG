using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItemApplication : MonoBehaviour
{

    public Image image;
    public BattleCharacter player;
    public string playerName;
    public Text playerNameText;

    void Start()
    {
        playerName = player.characterName;
        playerNameText.text = playerName;
    }

    public void Press()
    {
        BattleManager.instance.SelectPlayer(player);   
    }

    void Update()
    {
        
    }
}
