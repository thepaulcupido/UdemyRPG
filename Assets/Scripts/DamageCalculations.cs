using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageCalculations : MonoBehaviour
{

    public Text damageText;
    public float lifetime = 1f;
    public float movementSpeed = 1f;
    public float placementJitter = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, lifetime);
        transform.position += new Vector3(0f, movementSpeed * Time.deltaTime, 0f);
    }

    public void SetDamage(int amount)
    {
        damageText.text = amount.ToString();
        transform.position += new Vector3(Random.Range(-placementJitter,placementJitter), Random.Range(-placementJitter,placementJitter), 0f); 
    }
}
