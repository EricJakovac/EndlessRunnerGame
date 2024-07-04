using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public AudioClip coinPickupSound;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (coinPickupSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(coinPickupSound);

                GetComponent<MeshRenderer>().enabled = false;
                GetComponent<Collider>().enabled = false;
                StartCoroutine(DestroyAfterSound(coinPickupSound.length));
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator DestroyAfterSound(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }
}
