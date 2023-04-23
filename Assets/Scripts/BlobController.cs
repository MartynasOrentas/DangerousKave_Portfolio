using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobController : MonoBehaviour
{
    private Animator _animator;
    private CharacterController2D _character;

    // Start is called before the first frame update
    void Start()
    {
        _character = GetComponent<CharacterController2D>();
        _animator = GetComponent<Animator>();
        _character.OnJumping += OnBlobJumping;
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetFloat("Speed", Mathf.Abs(_character.Velocity.x));
    }

    void OnBlobJumping(bool isJumping)
    {
        _animator.SetBool("IsJumping", isJumping);
    }
}
