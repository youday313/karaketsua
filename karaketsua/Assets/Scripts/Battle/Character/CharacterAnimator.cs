using UnityEngine;
using System.Collections;

/// <summary>
/// アニメーション一括管理だが使わなそう
/// </summary>
[RequireComponent(typeof(Character))]

public class CharacterAnimator : MonoBehaviour
{

    Character character;
    Animator animator;

    // Use this for initialization
    void Start()
    {
        character = GetComponent<Character>();
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {

    }
}
