using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCamera : MonoBehaviour
{
    [SerializeField] float charaSlectRotX;
    [SerializeField] float waitRivalRotX;

    Vector3 charaSelectRot;
    Vector3 waitRivalRot;

    [SerializeField] bool rotFlag;
    [SerializeField] float rotSpeed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        rotFlag = true;

        charaSelectRot = new Vector3(charaSlectRotX, 0, 0);
        waitRivalRot = new Vector3(waitRivalRotX, 0, 0);

        this.transform.localRotation = new Quaternion(charaSelectRot.x,
                                                      charaSelectRot.y,
                                                      charaSelectRot.z,
                                                      0);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 thisRotate = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
        Debug.Log(thisRotate);

        //フラグに応じて角度を変える
        if(rotFlag)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(charaSelectRot), rotSpeed);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(waitRivalRot), rotSpeed);
        }
    }

    public void changeRotFlag()
    {
        rotFlag = !rotFlag;
    }
}
