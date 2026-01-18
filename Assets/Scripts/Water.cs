using Unity.VisualScripting;
using UnityEngine;

public class Water : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetComponent<AudioSource>()?.Play();
    }

}
