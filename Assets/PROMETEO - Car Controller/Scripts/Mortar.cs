using UnityEngine;
using System.Collections;

public class Mortar : MonoBehaviour
{
    [SerializeField]
    public GameObject firepoint;

    [SerializeField]
    public ParticleSystem muzzleFlash;

    [SerializeField]
    public GameObject[] weaponPrefabs;

    [SerializeField]
    public float fireRate = 1f;

    private float nextFireTime;
    public bool playing = false;

    // Update is called once per frame
    void Update()
    {
        if(playing == false)
        {
            return;
        }
        else
        {
        if (Time.time > nextFireTime)
        {
            StartCoroutine(MuzzleFlash());
            // Select a random prefab from the array
            int randomIndex = Random.Range(0, weaponPrefabs.Length);
            GameObject prefabToShoot = weaponPrefabs[randomIndex];

            // Instantiate at the firing point's position with current rotation
            GameObject newBullet = Instantiate(prefabToShoot, firepoint.transform.position, firepoint.transform.rotation);
               

            // Apply velocity to move the bullet
            Rigidbody rb = newBullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = firepoint.transform.forward * 35f;
            }


            nextFireTime = Time.time + fireRate;
        }
    }

    }

    IEnumerator MuzzleFlash()
    {
        muzzleFlash.Play();
        yield return new WaitForSeconds(0.25f); // Adjust duration as needed
        muzzleFlash.Stop();
    }   
}
