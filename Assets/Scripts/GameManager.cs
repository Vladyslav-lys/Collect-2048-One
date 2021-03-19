using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : BaseManager<GameManager>
{
    public bool isStarted;
    public bool isWin;
    public bool isLose;
    public bool debugLevel;
    public bool isTutorial;
    public int level;
    public int diamondsCount;
    public Level[] levels;
    public Material[] levelSkyboxes;
    public Color sameColor;
    public Color sameEmissionColor;
    public Color otherColor;
    public Color otherEmissionColor;
    public PlayerManager playerManager;
    public MeshRenderer fogMesh;
    private UIManager _uiManager;
    private SoundManager _soundManager;

    public int FifthLevelPart
    {
        get { return PlayerPrefs.GetInt("FifthLevelPart", 1); }
        set { PlayerPrefs.SetInt("FifthLevelPart", value); }
    }
    
    public int Diamonds
    {
        get { return PlayerPrefs.GetInt("Diamonds", 0); }
        set { PlayerPrefs.SetInt("Diamonds", value); }
    }
    
    public int Level
    {
        get { return PlayerPrefs.GetInt("Level", 1); }
        set { PlayerPrefs.SetInt("Level", value); }
    }

    public int Tutorial
    {
        get { return PlayerPrefs.GetInt("Tutorial",1); }
        set { PlayerPrefs.SetInt("Tutorial",value); }
    }

    public Level CurrentLevel => levels[Level - 1];

    protected override void Awake()
    {
        if (!instance)
            instance = this;
        base.Awake();
    }

    protected override void Initialize()
    {
        if (debugLevel)
            Level = level;

        CurrentLevel.EnableLevel();
        RenderSettings.skybox = levelSkyboxes[FifthLevelPart - 1];
        fogMesh.material.SetColor("_FogColor", levelSkyboxes[FifthLevelPart - 1].GetColor("_SkyGradientBottom"));
        CurrentLevel.FloorMeshColorByMaterial(levelSkyboxes[FifthLevelPart - 1]);
        CurrentLevel.GroundMeshColorByMaterial(levelSkyboxes[FifthLevelPart - 1]);
    }

    private void Start()
    {
        _uiManager = UIManager.instance;
        _uiManager.winDiamondsText.text = PlayerPrefs.GetInt("Diamonds",0).ToString();
        _uiManager.currentLevelText.text = Level.ToString();
        _uiManager.nextLevelText.text = Level == levels.Length ? "1" : (Level+1).ToString();
        _uiManager.startDiamondsText.text = Diamonds.ToString();
        _soundManager = SoundManager.instance;
    }

    public void StartGame()
    {
        if(isStarted)
            return;
        
        isStarted = true;
        _soundManager.PlaySound(_soundManager.startSound);
        _uiManager.StartGame();
        ShowTutorial();
    }

    public bool CannotPlay() => isWin || isLose;
    
    public void ChangeCurrentLevelOtherCubesColor()
    {
        foreach (var otherCube in CurrentLevel.otherCubes)
        {
            if (otherCube.currentNum == playerManager.currentNum)
            {
                otherCube.ChangeColor(sameColor, sameEmissionColor);
                continue;
            }
            otherCube.ChangeColor(otherColor, otherEmissionColor);
        }
    }

    public void Win()
    {
        isWin = true;
        _uiManager.WinInTime(0.5f);
        _uiManager.winDiamondsText.text = diamondsCount.ToString();
        _soundManager.PlaySound(_soundManager.winSound);
        Diamonds += diamondsCount;
    }

    public void LoseCollided()
    {
        Lose();
        playerManager.Boom();
    }

    private void Lose()
    {
        isLose = true;
        _uiManager.LoseInTime(1f);
        _soundManager.Vibrate();
    }

    public void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    public void NextLevel()
    {
        Level++;
        if (Level > levels.Length)
        {
            Level = 1;
            FifthLevelPart = 1;
        }
            
        if (Level > (FifthLevelPart + 4) * FifthLevelPart)
            FifthLevelPart++;
        
        Restart();
    }

    public void AddDiamonds() => diamondsCount++;

    private int GetPowFromTwo(int number)
    {
        int pow = 0;
        while (number > 1)
        {
            if(number % 2 != 0)
                break;
            pow++;
            number /= 2;
        }
        return pow;
    }

    public void ShowTutorial()
    {
        if(Tutorial == 0 && !isTutorial)
            return;
        
        _uiManager.SetEnableTutorial(true);
    }
    
    public void CloseTutorial()
    {
        Tutorial = 0;
        isTutorial = isTutorial ? false : isTutorial;
        _uiManager.SetEnableTutorial(false);
    }
}
