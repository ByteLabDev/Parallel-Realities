using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPlayer : MonoBehaviour
{
    public float speed = 1.0f;
    public Transform target;
    public int damp = 5;

    // Update is called once per frame
    void Update()
    {
        if (!target) return;
        var step = speed * Time.deltaTime;
        if(Vector3.Distance(transform.position, target.position) >= 5.5f) transform.position = Vector3.MoveTowards(transform.position, target.position, step);


        var rotationAngle = Quaternion.LookRotation(new Vector3(target.position.x - transform.position.x, transform.position.y, target.position.z - transform.position.z)); // we get the angle has to be rotated
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationAngle, Time.deltaTime * damp); // we rotate the rotationAngle 
    }
}
