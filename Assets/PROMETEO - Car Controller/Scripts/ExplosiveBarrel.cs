using UnityEngine;
using FMODUnity;
[RequireComponent(typeof(StudioEventEmitter))]
public class Barrel : MonoBehaviour
{
    public float explosionRadius = 0f;
    public float explosionPower = 0f;
    public float upwardsModifier = 0f;
    public GameObject explosionEffect; // Prefab for explosion VFX
    public GameObject barrelMesh;
    public float destroyDelay = .5f;


    public int barrelHealth = 6;

    private bool isExploded = false;
    private StudioEventEmitter emitter;
    void Start()
    {
        emitter = AudioManager.instance.InitializeEventEmitter(FMODEvents.instance.barrelExplosion, this.gameObject);
        emitter.Stop();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isExploded) return;

        // Trigger explosion on collision
        if (collision.collider.CompareTag("Car"))
        {
            barrelHealth = 0;
            Explode();
        }
        else if (collision.collider)
        {
            if(barrelHealth <= 0)
            {
                Explode();
            }
            else
            {
                barrelHealth -= 1;
            }
            
        }
        
    }

    private void Explode()
    {
        isExploded = true;

        // Apply explosion force to nearby rigidbodies
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in colliders)
        {
            if (hit.attachedRigidbody != null)
            {
                hit.attachedRigidbody.AddExplosionForce(explosionPower, transform.position, explosionRadius, upwardsModifier, ForceMode.Impulse);
                explosionEffect.SetActive(true);
                explosionEffect.GetComponent<ParticleSystem>().Play();
                emitter.Play();
                barrelMesh.SetActive(false);
            }
        }

        if(isExploded)
        {
            GetComponent<Rigidbody>().isKinematic = true;   
        }

        // Destroy the barrel after a delay
        Destroy(gameObject, destroyDelay);
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize explosion radius in the editor
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}   