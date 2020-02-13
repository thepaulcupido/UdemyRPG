using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoader : MonoBehaviour
{

    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        // Create a new player if none exists
        if (PlayerController.instance == null) {
            Instantiate(Player);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
