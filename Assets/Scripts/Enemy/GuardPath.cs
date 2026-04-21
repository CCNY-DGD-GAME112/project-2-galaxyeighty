using System.Collections;
using TMPro;
using UnityEngine;

public class GuardPath : MonoBehaviour
{
    public static event System.Action OnGuardHasSpottedPlayer;

    public float speed = 4;
    public float waitTime = .4f;
    public float turnSpeed = 90;
    public float timeToDetect = .5f;

    public Light spotlight;
    public float viewDistance;
    public LayerMask viewMask;

    float viewAngle;
    float playerVisibleTimer;

    public Transform pathHolder;
    Transform player;
    Color originalSpotlightColor;

    private Animator animator;
    private CharacterController controller;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag ("Player").transform;
        viewAngle = spotlight.spotAngle;
        originalSpotlightColor = spotlight.color;

        Vector3[] points = new Vector3[pathHolder.childCount];
        for (int i = 0; i < points.Length; i++)
        {
            points [i] = pathHolder.GetChild(i).position;
            points [i] = new Vector3(points[i].x, transform.position.y, points[i].z);
        }

        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();


        StartCoroutine (FollowPath (points));   
    }
    void Update()
    {
        if (CanSeePlayer ())
        {
            playerVisibleTimer += Time.deltaTime;
        }
        else
        {
            playerVisibleTimer -= Time.deltaTime;
        }
        playerVisibleTimer = Mathf.Clamp (playerVisibleTimer, 0, timeToDetect);
        spotlight.color = Color.Lerp(originalSpotlightColor, Color.yellow, playerVisibleTimer / timeToDetect);

        if(playerVisibleTimer >= timeToDetect)
        {
            if (OnGuardHasSpottedPlayer != null)
            {
                OnGuardHasSpottedPlayer();
            }
        }
    }
    bool CanSeePlayer()
    {
        if (Vector3.Distance(transform.position, player.position) < viewDistance)
        {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);
            if (angleBetweenGuardAndPlayer < viewAngle / 2f)
            {
                if (!Physics.Raycast(transform.position, player.position, viewMask))
                {
                    return true;
                }
            }
        }
        return false;
    }

    IEnumerator FollowPath(Vector3[] points)
    {
        transform.position = points [0];

        int targetPointIndex = 1;
        Vector3 targetPoint = points [targetPointIndex];
        transform.LookAt(targetPoint);

        while (true)
        {
            transform.position = Vector3.MoveTowards (transform.position, targetPoint, speed * Time.deltaTime);
            if (transform.position == targetPoint)
            {
                targetPointIndex = (targetPointIndex + 1) % points.Length;
                targetPoint = points [targetPointIndex];
                yield return new WaitForSeconds(waitTime);
                yield return StartCoroutine (TurnToFace (targetPoint));
                animator.SetTrigger("isMoving");
            }
            yield return null;
        }
    }

    IEnumerator TurnToFace(Vector3 lookTarget)
    {
        Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.5f)
        {
            float angle = Mathf.MoveTowardsAngle (transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null; 
        }
    }

    

    void OnDrawGizmos()
    {
        Vector3 startPosition = pathHolder.GetChild(0).position;
        Vector3 previousPosition = startPosition;
        foreach (Transform point in pathHolder)
        {
            Gizmos.DrawSphere(point.position, .3f);
            Gizmos.DrawLine(previousPosition, point.position);
            previousPosition = point.position;
        }
        Gizmos.DrawLine(previousPosition, startPosition);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
    }
}
