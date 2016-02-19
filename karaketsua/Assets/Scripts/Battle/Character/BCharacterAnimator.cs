using UnityEngine;
using System.Collections;

public class BCharacterAnimator : MonoBehaviour {


    Animator animator;
    //移動時のアニメーション時間
    public float moveTime = 1.6f;
    
    // Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}

    public void SetMove(bool isEnable)
    {
        animator.SetBool("Move", isEnable);
    }
    public void SetSingleAttack(int number)
    {
        animator.SetTrigger("TapAttack" + number.ToString());
    }


	
}
