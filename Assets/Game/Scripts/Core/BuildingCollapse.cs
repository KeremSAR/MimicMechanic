using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCollapse : MonoBehaviour
{
    public float collapseTime = 3f;
    public float explosionForce = 500f;
    public float explosionRadius = 10f;

    private List<Rigidbody> rigidbodies = new List<Rigidbody>();

    private bool isCollapsing = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCollapsing)
        {
            StartCoroutine(CollapseBuilding());
        }
    }

    private IEnumerator CollapseBuilding()
    {
        isCollapsing = true;

        // Disable the collider to prevent further collisions
        GetComponent<Collider>().enabled = false;

        // Set all the child rigidbodies to be kinematic so they don't move initially
        foreach (Transform child in transform)
        {
            Rigidbody rb = child.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rigidbodies.Add(rb);
            }
        }

        // Wait for a brief moment before starting the collapse
        yield return new WaitForSeconds(0.5f);

        // Slowly turn all the child objects black
        float timeElapsed = 0f;
        while (timeElapsed < collapseTime)
        {
            float t = timeElapsed / collapseTime;
            foreach (Transform child in transform)
            {
                Renderer renderer = child.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = Color.Lerp(renderer.material.color, Color.black, t);
                }
            }
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Apply explosion force to each rigidbody
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = false;
            rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
        }

        // Destroy the building after a short delay
        //Destroy(gameObject, 4f);
    }
}
