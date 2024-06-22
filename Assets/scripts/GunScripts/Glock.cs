using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glock : Pistol
{
    private LineRenderer lineRenderer;
    public float maxRange = 1000f;

    public float shootTime = 0.2f;
    public float timesincelastshot = 0f;

    public Camera mainCamera;

    void Start()
    {
        try
        {
            // Check if LineRenderer is already attached
            lineRenderer = GetComponent<LineRenderer>();
            if (lineRenderer != null)
            {
                //Debug.LogWarning("LineRenderer is already attached to the GameObject.");
                return;
            }

            // Attempt to add LineRenderer if not already attached
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            if (lineRenderer == null)
            {
                //Debug.LogError("LineRenderer could not be added to the GameObject. GameObject name: " + gameObject.name);
                return;
            }

            lineRenderer.startWidth = 0.05f;
            lineRenderer.endWidth = 0.05f;
            lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
            if (lineRenderer.material == null)
            {
                //Debug.LogError("Material for LineRenderer could not be found.");
                return;
            }

            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;
            lineRenderer.positionCount = 2;
            lineRenderer.enabled = false; // Initially disable the line renderer

            //Debug.Log("LineRenderer initialized successfully.");
        }
        catch (System.Exception e)
        {
            //Debug.LogError("Exception in Start(): " + e.Message);
        }
    }

    void Update()
    {
        
        // Check if the right mouse button is held down
        if (Input.GetMouseButton(1))
        {
            Shoot();
            timesincelastshot += Time.deltaTime;
        }
    }

    // Override Shoot method specific to this type of pistol
    public override void Shoot()
    {
        base.Shoot();
        // Additional logic for Glock shooting
        Debug.Log("Glock specific shooting logic executed.");
        
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera reference is not assigned.");
            return;
        }

        // Get the position of the Glock
        Vector3 glockPosition = transform.position;

        // Calculate direction towards the center of the screen (crosshair)
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        if(timesincelastshot > fireRate){
            timesincelastshot = 0f;
            Ray ray = mainCamera.ScreenPointToRay(screenCenter);
            RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxRange))
        {
            Debug.Log("Hit: " + hit.transform.name);
            
            // Ensure the line renderer is properly set before using it
            if (lineRenderer != null)
            {
                // Draw the line renderer from the Glock position to the hit point
                lineRenderer.SetPosition(0, glockPosition);
                lineRenderer.SetPosition(1, hit.point);
            }
        }
        else
        {
            // Ensure the line renderer is properly set before using it
            if (lineRenderer != null)
            {
                // Draw the line renderer from the Glock position in the ray direction to max range
                lineRenderer.SetPosition(0, glockPosition);
                lineRenderer.SetPosition(1, glockPosition + ray.direction * maxRange);
            }
        }

        StartCoroutine(ShowLineRenderer());
            
        }
    
    }

    private IEnumerator ShowLineRenderer()
    {
        if (lineRenderer != null)
        {
            lineRenderer.enabled = true;
            //Debug.Log("LineRenderer enabled.");
            yield return new WaitForSeconds(0.1f); // Show the line for 0.1 seconds
            lineRenderer.enabled = false;
            //Debug.Log("LineRenderer disabled.");
        }
    }
}
