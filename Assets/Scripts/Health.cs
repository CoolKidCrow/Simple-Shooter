using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Health : NetworkBehaviour
{
    public float health;
    public float maxHealth;
    public float timeTillRegen;
    public Text healthText;
    float timeSinceHit;

    GameObject[] respawns;
    // Start is called before the first frame update
    void Start()
    {
        respawns = GameObject.FindGameObjectsWithTag("Respawn");
    }

    // Update is called once per frame
    void Update()
    {
        if(healthText != null)
            healthText.text = "Health: " + health;

        if (timeSinceHit < timeTillRegen)
            timeSinceHit += Time.deltaTime;

        if(health <= 0f)
            Respawn();
    }

    private void Respawn()
    {
        health = 200f;
        transform.position = respawns[Random.Range(0, respawns.Length)].transform.position;
    }

    void FixedUpdate()
    {
        if(health != maxHealth && timeSinceHit > timeTillRegen)
        {
            health += 1f;
        }
    }

    public void DealDamage(float damage)
    {
        Debug.Log("hit for " + damage);
        timeSinceHit = 0f;
        health -= damage;
    }
}
