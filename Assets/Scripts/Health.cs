using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    CharacterController controller;
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
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = "Health: " + health;

        if(Input.GetKeyDown(KeyCode.K))
            DealDamage(50f);

        if (timeSinceHit < timeTillRegen)
            timeSinceHit += Time.deltaTime;

        if(health <= 0f)
            Respawn();
    }

    private void Respawn()
    {
        health = 200f;
        controller.enabled = false;
        transform.position = respawns[Random.Range(0, respawns.Length)].transform.position;
        controller.enabled = true;
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
