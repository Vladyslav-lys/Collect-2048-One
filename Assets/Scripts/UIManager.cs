using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : BaseManager<UIManager>
{
    public GameObject swipePanel;
    public GameObject playPanel;
    public GameObject losePanel;
    public GameObject winPanel;
    public GameObject mainPanel;
    public GameObject shopPanel;
    public GameObject settingsPanel;
    public GameObject tutorialPanel;
    public GameObject creditsPanel;
    public GameObject closeVibration;
    public GameObject closeSound;
    public GameObject goNextTextObj;
    public RectTransform levelLineTransform;
    public TextMeshProUGUI startDiamondsText;
    public TextMeshProUGUI winDiamondsText;
    public TextMeshProUGUI currentLevelText;
    public TextMeshProUGUI nextLevelText;
    public Button[] skinBtns;
    public float maxLevelLineWidth;
    private float _partLength;
    private float _targetLinelength;
    private int _linePartsCount;
    private SkinManager _skinManager;

    protected override void Awake()
    {
        if (!instance)
            instance = this;
        base.Awake();
    }

    protected override void Initialize()
    {
        levelLineTransform.sizeDelta = new Vector2(0f, levelLineTransform.sizeDelta.y);
    }

    private void Start()
    {
        _linePartsCount = GameManager.instance.CurrentLevel.otherCubes.Count;
        _partLength = (float)Math.Ceiling(maxLevelLineWidth / _linePartsCount);
        _targetLinelength = _partLength;
        _skinManager = SkinManager.instance;
        skinBtns[_skinManager.SkinNum].interactable = false;
    }

    public void StartGame()
    {
        mainPanel.SetActive(false);
        playPanel.SetActive(true);
    }
    
    public void Win()
    {
        EnablePlay(false);
        winPanel.SetActive(true);
        EnableGoNextInTime(0.5f);
    }
    
    public void WinInTime(float time) => Invoke(nameof(Win), time);
    
    public void LoseInTime(float time) => Invoke(nameof(Lose), time);
    
    public void Lose()
    {
        EnablePlay(false);
        losePanel.SetActive(true);
    }

    public void OpenShop()
    {
        EnableMain(false);
        shopPanel.SetActive(true);
    }
    
    public void CloseShop()
    {
        EnableMain(true);
        shopPanel.SetActive(false);
    }

    public void EnableDisableSettings() => settingsPanel.SetActive(!settingsPanel.activeSelf);

    private void EnablePlay(bool isEnable)
    {
        swipePanel.SetActive(isEnable);
        playPanel.SetActive(isEnable);
    }

    private void EnableMain(bool isEnable)
    {
        swipePanel.SetActive(isEnable);
        mainPanel.SetActive(isEnable);
    }

    public void LevelLineUp() => StartCoroutine(LevelLineUpEnum());

    private IEnumerator LevelLineUpEnum()
    {
        while (levelLineTransform.sizeDelta.x < _targetLinelength)
        {
            levelLineTransform.sizeDelta += new Vector2(5f,0f); 
            yield return  null;
        }

        _targetLinelength += _partLength;
    }

    public void OnOffClose(GameObject closeImage) => closeImage.SetActive(!closeImage.activeSelf);

    public void EnableGoNext() => goNextTextObj.SetActive(true);
    
    public void EnableGoNextInTime(float time) => Invoke(nameof(EnableGoNext), time);

    public void ChoosenShopBtn(Button choosenBtn)
    {
        foreach (var skinBtn in skinBtns)
        {
            skinBtn.interactable = true;
        }

        choosenBtn.interactable = false;
    }

    public void SetEnableTutorial(bool isActive) => tutorialPanel.SetActive(isActive);

    public void SetEnableCredits(bool isActive) => creditsPanel.SetActive(isActive);
}
