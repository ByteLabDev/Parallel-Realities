using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ForceLog : MonoBehaviour
{
    public TextMesh textMesh;
    void Update()
    {
        textMesh.text = transform.position.ToString();
    }
}
