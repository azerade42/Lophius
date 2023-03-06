using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class CoreAI : MonoBehaviour
{
    // This enum handles each state of behavior the AI is in. Each state can be thought of as a constant. We apply specific instructions based on the state the AI is in, and those instructions run every frame that the AI is in that state.
    private enum AIState
    {
        // Default state. AI will either roam to randomly generated points OR will roam to predetermined waypoints, depending on a bool check (_usingWaypoints).
        Passive,
        // In this state the AI will actively seek out the player. In this example project, the AI will attempt to make physical contact with the player.
        // Losing line of sight of the player for a long enough time will cause the AI to go back to the "Passive" state.
        Hostile 
    }

    // This is the current state the AI is in. We serialize the field so we can edit it in Unity's Inspector window.
    [SerializeField]
    private AIState _AIState;

    // The targetMask is the Unity Layer the object the AI wants to attack is on.
    public LayerMask targetMask;
    // The ObstructionMask is the Unity Layer that blocks the AI's ability to "see".
    public LayerMask ObstructionMask;

    // _canSeePlayer informs us if the AI can "see" the player on a specific frame.
    public bool _canSeePlayer;

    // _isChasingPlayer informs us when the AI actively knows the location of the player.
    public bool _isChasingPlayer = false;

    public bool _isDead = false;

    private bool winFlag = false;

    // _IAmWaiting informs us when the AI is holding a position
    [SerializeField]
    private bool _IAmWaiting;

    // Is the wait time randomized?
    [SerializeField]
    private bool _randomWaitTime;

    // This bool will determine if the AI waits when it reaches a random point, or if it never stops moving
    [SerializeField]
    private bool _alwaysMoving;

    // Boolean to determine is random wandering is enabled.
    [SerializeField]
    private bool _randomWander;

    //Is the AI chasing the player or fleeing?
    [SerializeField]
    private bool _fleeFromPlayer;

    // How long the AI will wait at each point if AlwaysMoving is FALSE
    [SerializeField]
    [Range(1, 7)] private int wait_time;

    // This float holds a value that we determine so we can manipulate the NavMeshAgent's built in speed variable.
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _attackSpeed;

    // This float will hold how long it has been since the AI has "seen" the player. We will use this so the AI can lose track of the player.
    private float _timeSinceSeenPlayer;

    // WalkRadius is a slider that dictates how far away a randomly generated waypoint can spawn from the AI's current position. Larger values mean a much larger roam.
    [Range(0, 500)] public float walkRadius;

    public bool playerCaught;

    // The Radius of the circle that limits each AI's vision distance.
    public float _FOVRadius = 20f;
    // The angle of the FOV cone for each AI.
    [Range(0, 360)]
    public float _FOVAngle = 90f;

    // The Radius of the circle that limits each AI's proximity reading distance.
    public float _proximityRadius = 20f;
    // The angle of the proximity cone for each AI.
    [Range(0, 360)]
    public float _proximityAngle = 90f;

    // This array holds all of our waypoints that we present on the map. The AI will only use these if _randomWander is false.
    [SerializeField]
    private Transform[] _waypoints;

    // This int value determines which waypoint we go to next.
    private int _nextWaypoint = 0;

    // A reference to the player
    //[HideInInspector]
    public GameObject _player;
    public WaypointManager waypointManager;

    // Color of the enemy for visual clarity
    public Renderer [] _materialColors;

    public Material _enemyWanderingColor;
    public Material _enemySearchingColor;
    public Material _enemyAttackingColor;
    public Material _enemyFleeingColor;
    
    public Light bulbLight;

    public Animator anim;

    private bool isCaught = false;

    public AudioSource audioSource;
    public AudioClip detectPlayerSound;
    public AudioClip marshyHitSound;
    public AudioClip marshyDeathSound;

    // stuff from Gina
    // player
    private PlayerController playerController;

    // freeze on impact of crystal for certain amount of time
    public float isFrozen = 20f;

    // Grabbing a reference to the NavMeshAgent Unity Component. The NavMeshAgent allows us to move the AI and to limit the area(s) it is allowed to enter.
    private NavMeshAgent _navMeshAgent;

    // awake
    void Awake()
    {
        // player
        playerController = GameObject.FindObjectOfType<PlayerController>();
    }

    private void Start()
    {
        // _enemyColor = GetComponent<Renderer>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        //_player = GameObject.Find("Player");
        StartCoroutine(CheckForPlayer());

        audioSource = GetComponent<AudioSource>();

        _waypoints = waypointManager._trackedWaypoints;

    }
    private void Update()
    {
        switch (_AIState)
        {
            case AIState.Passive:

                ChangeRendererColors(_enemyWanderingColor);
                _navMeshAgent.speed = _speed;
                anim.SetBool("isChasingPlayer", _isChasingPlayer);

                if (_randomWander == true)
                {
                    Wander();
                    if (_canSeePlayer == true)
                    {
                        _AIState = AIState.Hostile;
                    }

                }
                else
                {
                    if (_navMeshAgent.remainingDistance < 2f && _canSeePlayer == false)
                    {
                        GotoNextPoint();
                    }
                    if (_canSeePlayer == true)
                    {
                        _AIState = AIState.Hostile;
                    }
                }
                break;


            case AIState.Hostile:
                if (_fleeFromPlayer == true)
                {
                    FleeFromPlayer();
                    _navMeshAgent.speed = 30;
                    ChangeRendererColors(_enemyFleeingColor);
                }
                else
                {
                    ChasePlayer();

                    if (!isCaught)
                    {  
                        isCaught = true;
                        StartCoroutine(playCaughtSound());
                    } 
                    _navMeshAgent.speed = _attackSpeed;
                    anim.SetBool("isChasingPlayer", _isChasingPlayer);

                }
                if (_canSeePlayer == false)
                {
                    FieldOfViewCheck();
                }
                    break;
        }
        ProximityCheck();
    }

    IEnumerator playCaughtSound()
    {
        PlaySound(detectPlayerSound);
        yield return new WaitForSeconds(detectPlayerSound.length);
        isCaught = false;
    }

    private void Wander()
    {
        if (_navMeshAgent != null && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance && _IAmWaiting == false)
        {
            _navMeshAgent.SetDestination(RandomNavMeshLocation());
            _IAmWaiting = true;
            StartCoroutine(RandomWaitTimer());

        }
    }
    public Vector3 RandomNavMeshLocation()
    {
        Vector3 finalPosition = Vector3.zero;
        Vector3 randomPosition = Random.insideUnitSphere * walkRadius;
        randomPosition += transform.position;
        if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, walkRadius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }
    void GotoNextPoint()
    {
        if (_waypoints.Length == 0)
            return;
        _navMeshAgent.destination = _waypoints[_nextWaypoint].position;
        _nextWaypoint = (_nextWaypoint + 1) % _waypoints.Length;
        _IAmWaiting = true;

        StartCoroutine(RandomWaitTimer());
    }
    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, _FOVRadius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < _FOVAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, ObstructionMask))
                {
                    _canSeePlayer = true;
                }
                else
                {
                    _canSeePlayer = false;
                }
            }
            else
            {
                _canSeePlayer = false;

            }
        }
        else if (_canSeePlayer == false)
        {
            _canSeePlayer = false;
            _timeSinceSeenPlayer += Time.deltaTime;

            if (_timeSinceSeenPlayer >= 2f)
            {
                _canSeePlayer = false;
                _isChasingPlayer = false;
                _AIState = AIState.Passive;
                _timeSinceSeenPlayer = 0;
            }
        }
    }
    private void ProximityCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, _proximityRadius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < _proximityAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, ObstructionMask))
                {
                    _canSeePlayer = true;
                }
                else
                {
                    _canSeePlayer = false;
                }
            }
            else
            {
                _canSeePlayer = false;

            }
        }
        else if (_canSeePlayer == false)
        {
            _canSeePlayer = false;
            _timeSinceSeenPlayer += Time.deltaTime;

            if (_timeSinceSeenPlayer >= 2f)
            {
                _canSeePlayer = false;
                _isChasingPlayer = false;
                _AIState = AIState.Passive;
                _timeSinceSeenPlayer = 0;
            }
        }
    }
    private IEnumerator CheckForPlayer()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }

    }
    IEnumerator RandomWaitTimer()
    {
        if (_alwaysMoving == false && _randomWaitTime == true)
        {
            wait_time = Random.Range(1, 5);
            _navMeshAgent.speed = 0;
            yield return new WaitForSeconds(wait_time);
        }
        else if (_alwaysMoving == false && _randomWaitTime == false)
        {
            _navMeshAgent.speed = 0;
            yield return new WaitForSeconds(wait_time);
        }
        _navMeshAgent.speed = _speed;
        _IAmWaiting = false;
        }
    private void ChasePlayer()
    {
        if (!winFlag)
        {
            _isChasingPlayer = true;
        }
        else
        {
            _isChasingPlayer = false;
        }
        _navMeshAgent.destination = _player.transform.position;
        if (_canSeePlayer == true)
        {
            ChangeRendererColors(_enemyAttackingColor);
        }
        else
        {
            // ChangeRendererColors(_enemySearchingColor);
        }
        FieldOfViewCheck();
    }

    public void FleeFromPlayer()
    {
        Vector3 runTo = transform.position + ((transform.position - _player.transform.position) * 1);
        float distance = Vector3.Distance(transform.position, _player.transform.position);
        if (distance < walkRadius) _navMeshAgent.SetDestination(runTo);
    }

    private void ChangeRendererColors(Material newMat)
    {
        foreach (Renderer r in _materialColors)
        {
            r.material = newMat;
        }

        // float scaledTime = t * (float) (LENGHT - 1);
        // Color oldColor = _colors[(int) scaledTime];
        // Color newColor = _colors[(int) (scaledTime + 1f)];
        // float newT = scaledTime - Mathf.Round(scaledTime); 

        bulbLight.color = newMat.color;
    }

    /*void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !playerCaught)
        {
            playerCaught = true;
            print("OM NOM NOM NOM NOM NOM NOM");

        }
    }*/

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    // from Gina
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerCaught = true;
            //print("OM NOM NOM NOM NOM NOM NOM");
            //Debug.Log("Load: LoseScreen");
            StartCoroutine(WaitForLoseScreen());
        }

        if (collision.gameObject.tag == "beam")
        {
            Debug.Log("Hit Marshy");
            //SceneManager.LoadScene("WinScene");
            PlaySound(marshyDeathSound);
            StartCoroutine(WaitForWinScreen());
        }
    }

    IEnumerator WaitForLoseScreen()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("LoseScreen");
    }

    IEnumerator WaitForWinScreen()
    {
        _speed = 0;
        _attackSpeed = 0;

        winFlag = true;
        _isChasingPlayer = false;
        _isDead = true;
        anim.SetBool("isDead", _isDead);

        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("WinScene");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 12)
        {
            Debug.Log("Flash Collison");
            PlaySound(marshyHitSound);
            StartCoroutine(Freeze(5f));
        }

        if (other.gameObject.layer == 13)
        {
            //can't figure out will come back to later
            /*_canSeePlayer = true;
            _isChasingPlayer = true;*/
            Destroy(other.gameObject);
            Debug.Log("Distraction collison");
        }
    }

    IEnumerator Freeze(float isFrozen)
    {
        Debug.Log("Freeze started");
        _speed = 0;
        _attackSpeed = 0;
        yield return new WaitForSeconds(isFrozen);
        _speed = 7;
        _attackSpeed = 12;
        Debug.Log("Freeze ended");
    }

    void PlayerHidden()
    {
        if (playerController.hidden == true)
        {
            _isChasingPlayer = false;
            //Debug.Log("is chasing = " + _isChasingPlayer);
            _canSeePlayer = false;
            //Debug.Log("see player = " + _canSeePlayer);
        }
        if (playerController.invisibile == true)
        {
            _isChasingPlayer = false;
            //Debug.Log("is chasing = " + _isChasingPlayer);
            _canSeePlayer = false;
            //Debug.Log("see player = " + _canSeePlayer);
        }
    }
}

