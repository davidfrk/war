using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController cameraController;
    private RaycastHit raycastHit;

    void Awake()
    {
        cameraController = this;
    }

    public bool MouseRaycast(out Territory territory)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out raycastHit))
        {
            Territory selection = raycastHit.transform.GetComponent<Territory>();
            territory = selection;
            return selection != null;
        }
        else
        {
            territory = null;
            return false;
        }
    }
}
