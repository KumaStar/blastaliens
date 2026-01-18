using UnityEngine;
using System;
public class Player : MonoBehaviour
{
    private float _JumpEndTime;

    [SerializeField] private float _HorizontelVelocity = 5f;
    [SerializeField] private float _JumpVelocity = 5f;
    [SerializeField] private float _JumpDuration = 0.5f;
    [SerializeField] private Sprite _JumpSprite;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _footOffset = 0.1f;
    private float _horizontal;
    private int _jumpRemaining;
    private AudioSource _audioSource;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    public bool IsGrounded;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
    }
    private void OnDrawGizmos()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Gizmos.color = Color.red;

        Vector2 origin = new Vector2(transform.position.x, transform.position.y - spriteRenderer.bounds.extents.y);
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.1f);

        //draw left foot ray
        origin = new Vector2(origin.x - _footOffset, origin.y);
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.1f);

        //draw right foot ray
        origin = new Vector2(origin.x + 2*_footOffset, origin.y); // origin.x was already moved left by _footOffset
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.1f);
    }

    void Update()
    {
        updateGrounding();

        // Movement
        _horizontal = Input.GetAxis("Horizontal");
        Debug.Log(_horizontal);
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        var vertical = rb.linearVelocity.y;

        if (Input.GetButtonDown("Fire1") && _jumpRemaining > 0)
        {
            _JumpEndTime = Time.time + _JumpDuration;
            _jumpRemaining--;
            _audioSource.pitch = (_jumpRemaining > 0) ? 1f : 1.2f;
            _audioSource.Play();
        }
        if (Input.GetButton("Fire1") && (_JumpEndTime > Time.time))
        {
            vertical = _JumpVelocity;
        }
        _horizontal *= _HorizontelVelocity;
        rb.linearVelocity = new Vector2(_horizontal, vertical);
        updateSprite();
    }

    private void updateGrounding()
    {
        IsGrounded = false;
        // Ground check
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - _spriteRenderer.bounds.extents.y);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 0.1f, _layerMask);

        if (hit.collider)
            IsGrounded = true;

        // Left foot
        origin = new Vector2(transform.position.x - _footOffset, transform.position.y - _spriteRenderer.bounds.extents.y);
        hit = Physics2D.Raycast(origin, Vector2.down, 0.1f, _layerMask);

        if (hit.collider)
            IsGrounded = true;

        // Right foot
        origin = new Vector2(transform.position.x + _footOffset, transform.position.y - _spriteRenderer.bounds.extents.y);
        hit = Physics2D.Raycast(origin, Vector2.down, 0.1f, _layerMask);

        if (hit.collider)
            IsGrounded = true;

        if(IsGrounded && GetComponent<Rigidbody2D>().linearVelocity.y <= 0)
        {
            _jumpRemaining = 2;
        }
       
    }

    private void updateSprite()
    {
        _animator.SetBool("IsGrounded" , IsGrounded);
        _animator.SetFloat("HorizontalVelocity", Math.Abs(_horizontal));

        if (_horizontal < 0)
        {
            _spriteRenderer.flipX = true;
        }
        else if (_horizontal > 0)
        {
            _spriteRenderer.flipX = false;
        }
    }
}
