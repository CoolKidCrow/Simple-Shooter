using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    [Header("Camera Settings")]
    [SerializeField] float sensMouseX = 1f;
    [SerializeField] float sensMouseY = 1f;
    [SerializeField] float sensBaseline = 90f;

    [SerializeField] private Transform cam;
    [SerializeField] private Transform orientation;

    public float mouseX;
    public float mouseY;

    private float xTurn;
    private float yTurn;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        LookControls();
    }


    private void LookControls()
    {
        //grabs mouse input
        mouseX = Input.GetAxisRaw("Mouse X") * sensMouseX * sensBaseline * Time.deltaTime;
        mouseY = Input.GetAxisRaw("Mouse Y") * sensMouseY * sensBaseline * Time.deltaTime;

        yTurn += mouseX;
        
        xTurn -= mouseY;
        xTurn = Mathf.Clamp(xTurn, -90f, 90f);

        cam.transform.rotation = Quaternion.Euler(xTurn, yTurn, 0);
        orientation.transform.rotation = Quaternion.Euler(0, yTurn, 0);
    }
}