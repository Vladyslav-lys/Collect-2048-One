using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum Changer
{
    ChangeX, ChangeZ
}

public enum DirCancelled
{
    noneDirCancelled, xDirCancelled, zDirCancelled, xNegativeDirCancelled, zNegativeDirCancelled
}

public class Wall : MonoBehaviour
{
    public Changer change;
    public DirCancelled dirCancelled;
    public Transform wallTransform;
    public List<Collider> ignoreColliders;

    private void Awake()
    {
        if (IsDirCancelled(DirCancelled.noneDirCancelled))
            gameObject.SetActive(false);
    }
    
    public bool IsDirCancelled(DirCancelled dirCancelled) => this.dirCancelled == dirCancelled;
}