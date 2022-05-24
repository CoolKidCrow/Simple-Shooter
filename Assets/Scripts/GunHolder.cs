using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunHolder : MonoBehaviour
{
    public GameObject activeGun;
    public GameObject[] guns;
    public Text ammoText;
    // Start is called before the first frame update
    void Start()
    {
        activeGun = guns[0];
        foreach(GameObject gun in guns)
            gun.SetActive(false);
        activeGun.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeActiveGun(0);
        }else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeActiveGun(1);
        }else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeActiveGun(2);
        }else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ChangeActiveGun(3);
        }else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ChangeActiveGun(4);
        }

        if(Input.GetButtonDown("Fire1"))
        {
            activeGun.SendMessage("Fire", true, SendMessageOptions.DontRequireReceiver);
        }
        
        if(Input.GetButtonUp("Fire1"))
        {
            activeGun.SendMessage("Fire", false, SendMessageOptions.DontRequireReceiver);
        }

        if(Input.GetButtonDown("Reload"))
        {
            activeGun.SendMessage("Reload", SendMessageOptions.DontRequireReceiver);
        }
    }

    private void ChangeActiveGun(int index)
    {
        if(activeGun == guns[index]) return;

        activeGun.SetActive(false);
        activeGun = guns[index];
        activeGun.SetActive(true);
    }

    public void UpdateAmmo(int ammo)
    {
        ammoText.text = "Ammo: " + ammo;
    }
}