using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidMovement : MonoBehaviour
{
    [SerializeField] private GameObject liquid;
    [SerializeField] private GameObject liquidMesh;

    private int SloshSpeed = 60;
    private int rotateSpeed = 5;
    private int difference = 6;

    private void Update()
    {
        Slosh();

        // Assign the rotation with the y axis (with my obj) with the speed through delta time, then rotate only via self
        liquidMesh.transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.Self);
    }

    private void Slosh()
    {
        // Inverse cup rotation
        Quaternion inverseRotation = Quaternion.Inverse(transform.localRotation);

        // Rotate towards certain angle
        Vector3 finalRotation = Quaternion.RotateTowards(liquid.transform.localRotation, inverseRotation, SloshSpeed * Time.deltaTime).eulerAngles;

        // clamp, allowing it to rotate based on the y axis, so it's set on itself and won't just leave the parent item
        finalRotation.z = ClampRotationValue(finalRotation.z, difference);
        finalRotation.x = ClampRotationValue(finalRotation.x, difference);

        liquid.transform.localEulerAngles = finalRotation;
    }

    private float ClampRotationValue(float value, float difference)
    {
        float val = 0.0f;

        // making sure the rotation doesn't go over the euler angle causing the rotation to screw up
        if (value > 180)
        {
            val = Mathf.Clamp(value, 360 - difference, 360);
        }
        else
        {
            val = Mathf.Clamp(value, 0, difference);
        }

        return val;
    }
}
