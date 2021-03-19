using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinManager : BaseManager<SkinManager>
{
    public Material[] skinMaterials;
    public Material[] trailMaterials;
    public Color[] trailColors;
    private PlayerManager _playerManager;
    
    protected override void Awake()
    {
        if (!instance)
            instance = this;
        base.Awake();
    }

    private void Start()
    {
        _playerManager = PlayerManager.instance;
        _playerManager.SetMaterial(skinMaterials[SkinNum]);
        _playerManager.SetTrailMaterial(trailMaterials[SkinNum]);
        _playerManager.SetTrailColor(trailColors[SkinNum]);
    }

    public int SkinNum
    {
        get { return PlayerPrefs.GetInt("SkinNum", 0); }
        set { PlayerPrefs.SetInt("SkinNum", value); }
    }

    public void SetSkin(int skinNum)
    {
        SkinNum = skinNum;
        _playerManager.SetMaterial(skinMaterials[skinNum]);
        _playerManager.SetTrailMaterial(trailMaterials[skinNum]);
        _playerManager.SetTrailColor(trailColors[skinNum]);
    }
}
