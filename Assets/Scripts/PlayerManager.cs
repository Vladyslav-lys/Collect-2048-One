using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using Unity.Mathematics;
using UnityEngine;

public class PlayerManager : BaseManager<PlayerManager>
{
    public int currentNum;
    public TextMeshProUGUI numText;
    public float speed;
    public bool canMove;
    public bool xDir, zDir;
    public bool xNegativeDir, zNegativeDir;
    public Vector3 moveVector;
    public GameObject splash;
    public MeshRenderer playerMesh;
    public TrailRenderer trailRenderer;
    private GameManager _gm;
    private List<string> _pressedKeys;
    private Rigidbody _rb;
    private SoundManager _soundManager;
    private List<Collider> _ignoreColliders;
    private bool _canSetTransform;
    
    protected override void Awake()
    {
        if (!instance)
            instance = this;
        base.Awake();
    }

    protected override void Initialize()
    {
        numText.text = currentNum.ToString();
        _pressedKeys = new List<string>() { "a", "s", "d", "w" };
        _rb = GetComponent<Rigidbody>();
        transform.localScale = Vector3.zero;
        canMove = true;
        _canSetTransform = true;
        _ignoreColliders = new List<Collider>();
    }

    private void Start()
    {
        _gm = GameManager.instance;
        _soundManager = SoundManager.instance;
        Invoke(nameof(EnableTrail), 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        //WALL
        if (other.gameObject.layer == 3)
        {
            xDir = xNegativeDir = zDir = zNegativeDir = true;
            
            canMove = true;
            _rb.velocity = Vector3.zero;
            Wall wall = other.GetComponent<Wall>();
            float x = transform.position.x;
            float z = transform.position.z;
            
            for (int i = 0; i < _ignoreColliders.Count; i++)
            {
                _ignoreColliders[i].enabled = true;
            }
            _ignoreColliders.Clear();

            if (wall.ignoreColliders.Count > 0)
            {
                foreach (var ignoreCollider in wall.ignoreColliders)
                {
                    _ignoreColliders.Add(ignoreCollider);
                    ignoreCollider.enabled = false;
                }
            }
            
            SetWallDir(other);
            
            if(wall.change == Changer.ChangeX)
                x = (xDir || xNegativeDir) ? wall.wallTransform.position.x : x;
            if(wall.change == Changer.ChangeZ)
                z = (zDir || zNegativeDir) ? wall.wallTransform.position.z : z;

            if (_canSetTransform)
            {
                transform.position = new Vector3(x, transform.position.y, z);
                _canSetTransform = false;
                Invoke(nameof(TrueCanSetTransform),0.2f);
            }
        }

        //OTHER CUBE
        if (other.gameObject.layer == 7)
        {
            OtherCube otherCube = other.GetComponent<OtherCube>();
            
            if (otherCube.currentNum == currentNum)
            {
                _gm.AddDiamonds();
                currentNum += otherCube.currentNum;
                numText.text = currentNum.ToString();
                _gm.ChangeCurrentLevelOtherCubesColor();
                _gm.CurrentLevel.KillTouchedOtherCube(otherCube);
                _soundManager.PlaySound(_soundManager.collectSound);
                Destroy(other.gameObject);
                UIManager.instance.LevelLineUp();
                if(_gm.CurrentLevel.otherCubes.Count <= 0)
                    _gm.Win();
            }
            else
            {
                _gm.LoseCollided();
                Destroy(gameObject);
            }
        }
        
        //LEVER BTN
        if (other.gameObject.layer == 8)
        {
            other.transform.localPosition = Vector3.zero;
            other.GetComponent<LeverBtn>().enabled = true;
            other.GetComponent<Collider>().enabled = false;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        //WALL
        if (other.gameObject.layer == 3)
        {
            SetWallDir(other);
        }
    }

    private void SetWallDir(Collider other)
    {
        Wall wall = other.GetComponent<Wall>();
        if (wall.change == Changer.ChangeX)
        {
            if (wall.IsDirCancelled(DirCancelled.xDirCancelled))
            {
                xDir = false;
                //xNegativeDir = true;
            }
            if (wall.IsDirCancelled(DirCancelled.xNegativeDirCancelled))
            {
                xNegativeDir = false;
                //xDir = true;
            }
        }

        if (wall.change == Changer.ChangeZ)
        {
            if (wall.IsDirCancelled(DirCancelled.zDirCancelled))
            {
                zDir = false;
                //zNegativeDir = true;
            }
            if (wall.IsDirCancelled(DirCancelled.zNegativeDirCancelled))
            {
                zNegativeDir = false;
                //zDir = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!_gm.isStarted)
        {
            _rb.velocity = Vector3.zero;
            return;
        }
        
        MovePlayerByForce();
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x,10.4f,transform.position.z);
        
        if (_pressedKeys.Contains(Input.inputString))
            _gm.StartGame();
        
        if (!canMove || _gm.CannotPlay())
            return;
        
        switch (Input.inputString)
        {
            case "a":
                SetDirAndCondition(xNegativeDir, Vector3.left);
                break;
            case "d":
                SetDirAndCondition(xDir, Vector3.right);
                break;
            case "w":
                SetDirAndCondition(zDir, Vector3.forward);
                break;
            case "s":
                SetDirAndCondition(zNegativeDir, Vector3.back);
                break;
        }
    }

    public void SetDirAndCondition(bool condition, Vector3 dir)
    {
        if(!condition)
            return;
        canMove = false;
        moveVector = dir;
        _soundManager.PlaySound(_soundManager.swipeSound);
    }
    
    public void MovePlayerByForce() => _rb.AddForce(speed * moveVector, ForceMode.Impulse);

    public void Boom()
    {
        Destroy(Instantiate(splash,transform.position,Quaternion.identity),5f);
        _soundManager.PlaySound(_soundManager.otherCubeHitSound);
    }

    public void SetMaterial(Material mat) => playerMesh.material = mat;

    public void SetTrailMaterial(Material trialMat) => trailRenderer.material = trialMat;
    
    public void SetTrailColor(Color color)
    {
        trailRenderer.startColor = color;
        trailRenderer.endColor = new Color(color.r, color.g, color.b, 0f);
    }

    public void EnableTrail() => trailRenderer.gameObject.SetActive(true);

    public void TrueCanSetTransform() => _canSetTransform = true;
}
