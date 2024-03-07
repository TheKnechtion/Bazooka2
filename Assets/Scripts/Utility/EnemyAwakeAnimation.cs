using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAwakeAnimation : MonoBehaviour
{
    [SerializeField] private string AnimationName;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }


    private void OnEnable()
    {
        _animator.Play(AnimationName);
    }
}
