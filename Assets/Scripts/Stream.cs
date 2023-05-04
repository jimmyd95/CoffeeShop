using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stream : MonoBehaviour
{
    private LineRenderer lineRenderer = null;
    private ParticleSystem splashParticle = null;
    private Coroutine pourRoutine = null;
    private Vector3 targetPosition = Vector3.zero;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        splashParticle = GetComponentInChildren<ParticleSystem>(); // it's an object to the child of the prefab
    }

    private void Start()
    {
        MoveToPosition(0, transform.position);
        MoveToPosition(1, transform.position);
    }

    public void Begin()
    {
        StartCoroutine(UpdateParticle());
        pourRoutine = StartCoroutine(BeginPour());
    }

    private IEnumerator BeginPour()
    {
        // if the pour check is active, it will keep pouring
        while (gameObject.activeSelf)
        {
            targetPosition = FindEndPoint();

            MoveToPosition(0, transform.position); // starting position
            //MoveToPosition(1, targetPosition); // ending position
            AnimateToPosition(1, targetPosition); // set animation instead of just a line

            yield return null;
        }
    }

    public void End()
    {
        StopCoroutine(pourRoutine);
        pourRoutine = StartCoroutine(EndPour()); // for consistency, so not setting it to null
    }

    private IEnumerator EndPour()
    {
        // destory obj when it reaches to the endpoint, else it will destory obj instantly and make it less "animated"
        while (!HasReachedPosition(0, targetPosition))
        {
            // animate both start and end position and have a nice continuous stream if suddenly stopped
            AnimateToPosition(0, targetPosition);
            AnimateToPosition(1, targetPosition);
            yield return null;
        }

        Destroy(gameObject); // destroy the animation cause if we still have it, there will be too much object created
    }

    private Vector3 FindEndPoint()
    {
        // called from begin pour when reached threshold, find the endpoint of the linerenderer
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);

        // pass the ray to check the distance and where it hits
        Physics.Raycast(ray, out hit, 2.0f);
        // check where the end point is, if it didn't hit anything after x meter
        // it set the x as the end point
        Vector3 endPoint = hit.collider ? hit.point : ray.GetPoint(2.0f);
        return endPoint;
    }

    // don't animate the begin, just start at the item pouring tip
    private void MoveToPosition(int index, Vector3 targetPosition)
    {
        // make things very simple from pouring to reaching to the bottom
        lineRenderer.SetPosition(index, targetPosition);
    }

    // similar to moveToPosition but we are animating this
    private void AnimateToPosition(int index, Vector3 targetPosition)
    {
        Vector3 currentPoint = lineRenderer.GetPosition(index);
        Vector3 newPosition = Vector3.MoveTowards(currentPoint, targetPosition, Time.deltaTime * 1.75f); // current position of the index of the line renderer
        // have a decently smooth look in the coroutine
        lineRenderer.SetPosition(index, newPosition);
    }

    // testing the line position has met the animated position
    private bool HasReachedPosition(int index, Vector3 targetPosition)
    {
        // current position of index and compare it to target position
        Vector3 currentPosition = lineRenderer.GetPosition(index); // check if the line renderer meet the target position
        return currentPosition == targetPosition;
    }

    private IEnumerator UpdateParticle()
    {
        while (gameObject.activeSelf)
        {
            splashParticle.gameObject.transform.position = targetPosition;
            bool isHit = HasReachedPosition(1, targetPosition); // only show if the ending point reached the ground
            splashParticle.gameObject.SetActive(isHit); // on seperate obj so you can set them active and nonactive whenever needed
            yield return null;
        }
    }
}
