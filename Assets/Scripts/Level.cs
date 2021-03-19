using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public float coef;
    public MeshRenderer[] floorMeshes;
    public MeshRenderer[] groundMeshes;
    public List<OtherCube> otherCubes;
    public Transform startTransform;
    private Transform _playerTransform;
    
    private void Start()
    {
        GameManager.instance.ChangeCurrentLevelOtherCubesColor();
        _playerTransform = PlayerManager.instance.transform;
        _playerTransform.position = startTransform.position;
        foreach (var floorMesh in floorMeshes)
        {
            floorMesh.material.mainTextureScale =
                new Vector2(floorMesh.transform.localScale.x * coef, floorMesh.transform.localScale.z * coef);
        }
    }

    public void KillTouchedOtherCube(OtherCube otherCube) => otherCubes.Remove(otherCube);

    public void EnableLevel() => gameObject.SetActive(true);

    public void FloorMeshColorByMaterial(Material material)
    {
        foreach (var floorMesh in floorMeshes)
        {
            floorMesh.material.color = material.GetColor("_SkyGradientTop");
        }
    }

    public void GroundMeshColorByMaterial(Material material)
    {
        foreach (var groundMesh in groundMeshes)
        {
            groundMesh.material.color = material.GetColor("_SkyGradientBottom");
        }
    }
}
