using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OtherCube : MonoBehaviour
{
    public int currentNum;
    public TextMeshProUGUI numText;
    public MeshRenderer otherCubeMesh;

    private void Awake()
    {
        numText.text = currentNum.ToString();
        transform.localScale = Vector3.zero;
    } 

    public void ChangeColor(Color mainColor, Color emissionColor)
    {
        otherCubeMesh.material.color = mainColor;
        otherCubeMesh.material.SetColor("_EmissionColor", emissionColor);
    }
}
