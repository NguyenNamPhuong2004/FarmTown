using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float dragSpeed = 2;
    private Vector3 dragOrigin;

    void Update()
    {
       
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        
        if (Input.GetMouseButton(0))
        {
            Vector3 difference = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            Vector3 move = new Vector3(-difference.x * dragSpeed, -difference.y * dragSpeed, 0);

            transform.Translate(move, Space.World);
            dragOrigin = Input.mousePosition;
        }
    }
}
