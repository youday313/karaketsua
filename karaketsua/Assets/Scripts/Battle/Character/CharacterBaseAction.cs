using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// キャラクターの移動、攻撃などのベース
/// </summary>

public class CharacterBaseAction : MonoBehaviour {

    protected Character character;
    protected Animator animator;
    protected CameraMove cameraMove;

    //行動可能時に有効化
    protected bool isEnable = false;
    public bool IsEnable
    {
        get { return isEnable; }
        set
        {
            if (isEnable == false && value == true)
            {
                Enable();
            }
            else if (isEnable == true && value == false)
            {
                Disable();
            }
            isEnable = value;
        }

    }
    [System.NonSerialized]
    public bool isNowAction = false;



	// Use this for initialization
	void Start () {
        Init();
	}
    //作成時に初期化処理
    public virtual void Init()
    {
        character = GetComponent<Character>();
        animator = GetComponent<Animator>();

    }

    public virtual void Enable()
    {
        isNowAction = false;
        cameraMove = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMove>();
    }
    public virtual void Disable()
    {

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
