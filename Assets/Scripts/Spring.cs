using Unity.VisualScripting;
using UnityEngine;

public class Spring : MonoBehaviour
{
    [SerializeField] private Sprite _sprung;
    private AudioSource _audiosource;
    private Sprite _defaultSprite;
    private ContactPoint2D contact;
    SpriteRenderer _spriteRenderer;
    private void Awake()
    {
        _audiosource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultSprite = _spriteRenderer.sprite;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        contact = collision.contacts[0];

        if (collision.collider.CompareTag("Player") && contact.normal.y < -0.5)
        {
            _spriteRenderer.sprite = _sprung;
            _audiosource.pitch = 0.7f;
            _audiosource.Play();
        }
    } 

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
            _spriteRenderer.sprite = _defaultSprite;
    }
}

