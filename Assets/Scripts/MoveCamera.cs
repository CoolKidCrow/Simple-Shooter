using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MoveCamera : NetworkBehaviour
{
    [SerializeField] Transform cameraPosition;

    // Update is called once per frame
    void Update()
    {
        transform.position = cameraPosition.position;
    }
}