using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public Camera cam;
    public Collider self;
    public LayerMask bulletMask;
    public int currentAmmo;
    public int maxAmmo;
    public int damage;
    public float fireRate;
    public float reloadTime;
    public bool isFullAuto;
    public bool isShotgun;
    public Vector3[] pelletPos;
    float timeTillFire;

    // Update is called once per frame
    bool isFiring;
    bool lockGun;
    Light[] lights;
    int lightIndex;
    RaycastHit rayHit;

    void Start()
    {
        cam = GetComponentInParent<Camera>();
        lights = GetComponentsInChildren<Light>();

        timeTillFire = 60f / fireRate;
        timeDifference = timeTillFire;

        foreach(Light light in lights)
            light.enabled = false;
    }

    void OnDisable()
    {
        lockGun = false;
        isFiring = false;
        foreach(Light light in lights)
            light.enabled = false;
    }

    float timeDifference;
    void Update()
    {

        if (timeDifference < timeTillFire)
            timeDifference += Time.deltaTime;

        if(isFiring && isFullAuto)
        {
            if(timeDifference >= timeTillFire && currentAmmo > 0 && !lockGun)
            {
                timeDifference -= timeTillFire;
                UseBullet();
            }
        }

        SendMessageUpwards("UpdateAmmo", currentAmmo, SendMessageOptions.DontRequireReceiver);
    }

    private void UseBullet()
    {
        currentAmmo--;

        if(isShotgun)
        {
            foreach(Vector3 pellet in pelletPos)
            {
                Vector3 pelletDir = cam.transform.TransformPoint(pellet) - cam.transform.position;
                SendBullet(pelletDir);
            }
        }

        if(!isShotgun)
        {
            SendBullet(cam.transform.forward);
        }
    }

    private void SendBullet(Vector3 dir)
    {
        Physics.Raycast(cam.transform.position, dir, out rayHit, bulletMask);
        if(lightIndex == lights.Length)
            lightIndex = 0;
        StartCoroutine("MuzzleFlash", lights[lightIndex++]);
        if(rayHit.collider != null && rayHit.collider != self)
        {
            rayHit.collider.gameObject.SendMessage("DealDamage", damage, SendMessageOptions.DontRequireReceiver);
            Debug.DrawLine(cam.transform.position, rayHit.point, Color.red, 5f);
        }
    }

    public void Fire(bool fire)
    {
        isFiring = fire;
        
        if(timeDifference >= timeTillFire && currentAmmo > 0 && !lockGun && !isFullAuto && fire)
        {
            timeDifference -= timeTillFire;
            UseBullet();
        }

        if(currentAmmo == 0 && fire)
            Reload();
    }
    
    public void Reload()
    {
        if(!lockGun)
            StartCoroutine("ReloadGun");
    }

    public IEnumerator MuzzleFlash(Light light)
    {
        light.enabled = true;
        yield return new WaitForSeconds(0.1f);
        light.enabled = false;
    }

    public IEnumerator ReloadGun()
    {
        lockGun = true;
        yield return new WaitForSeconds(reloadTime);
        timeDifference = timeTillFire;
        currentAmmo = maxAmmo;
        lockGun = false;
    }
}