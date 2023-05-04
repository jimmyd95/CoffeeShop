using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PouringLiquid : MonoBehaviour
{

    public int pourThreshold = 45;
    public Transform origin = null;
    public GameObject streamPrefab = null;

    private bool isPouring = false;
    private Stream currentStream = null;

    private void Update()
    {
        // calculate the pouring angle
        bool pourCheck = CalculatePourXAngle() < pourThreshold || CalculatePourZAngle() < pourThreshold; // if less then threshold, no pouring

        if (isPouring != pourCheck)
        {
            isPouring = pourCheck;

            if (isPouring)
            {
                StartPouring();
            }
            else
            {
                EndPouring();
            }
        }
    }

    private void StartPouring()
    {
        print("start");
        currentStream = CreateStream();
        currentStream.Begin();
    }
    
    private void EndPouring()
    {
        Debug.Log("end");
        currentStream.End();
        currentStream = null; // set it to null so it won't accidentally cause issue
    }

    private float CalculatePourXAngle()
    {
        //Debug.Log("Testing pour" + transform.forward.y * Mathf.Rad2Deg);
        // I am using the y axis, but it might be wrong for different models
        //return transform.forward.z * Mathf.Rad2Deg; // check the tile angle via math
        return transform.right.x * Mathf.Rad2Deg; // check the tile angle via math
    }

    private float CalculatePourZAngle()
    {
        //Debug.Log("Testing pour" + transform.forward.y * Mathf.Rad2Deg);
        // I am using the y axis, but it might be wrong for different models
        return transform.forward.z * Mathf.Rad2Deg; // check the tile angle via math
    }

    private Stream CreateStream()
    {
        GameObject streamObj = Instantiate(streamPrefab, origin.position, Quaternion.identity, transform);
        return streamObj.GetComponent<Stream>();
    }

}
