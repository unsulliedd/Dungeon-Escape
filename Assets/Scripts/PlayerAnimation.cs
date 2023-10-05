using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator;

    void Start()
    {
        if (!GetComponentInChildren<Animator>())
            Debug.Log("Player's Animator is null");
        else
            _animator = GetComponentInChildren<Animator>();
    }

    public void Run(float _moveInput)
    {
        if (_moveInput == 0)
        {
            _animator.SetFloat("Move", -1);
        }
        else
        {
            _animator.SetFloat("Move", Mathf.Abs(_moveInput));
        }
    }
}
