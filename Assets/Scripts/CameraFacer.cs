using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFacer : MonoBehaviour
{
    [SerializeField] bool lockX;
    [SerializeField] bool lockY;
    [SerializeField] bool lockZ;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = Camera.main.transform.forward;
        //transform.LookAt(Camera.main.transform);
        //Vector3 rot = transform.rotation.eulerAngles;
        ////rot.x = -rot.x;
        //if (lockX) rot.x = 0;
        //if (lockY) rot.y = 0;
        //if (lockZ) rot.z = 0;
        ////rot.y = 0;
        //transform.rotation = Quaternion.Euler(rot);
    }
}
