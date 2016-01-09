using UnityEngine;
using System.Collections;

public class CharacterBaseSkill : MonoBehaviour {

    Character character;
    Animator animator;


	// Use this for initialization
	void Start () {
        character = GetComponent<Character>();
        animator = GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}



    void OnEnable()
    {
        //IT_Gesture.onDraggingEndE+=
    }
    void OnDisable()
    {
        //IT_Gesture.onDraggingE-=

    }

    public virtual void OnDragStart(DragInfo dragInfo) { }

}
