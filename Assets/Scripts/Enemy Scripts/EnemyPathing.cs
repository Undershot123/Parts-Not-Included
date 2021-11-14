using UnityEngine;
using UnityEngine.AI;

public class EnemyPathing : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// This target can be any GameObject that the enemy with path to.
    /// Can change at runtime.
    /// </summary>
    [Tooltip("Input what object you want the enemy to path to.")]
    [SerializeField] private GameObject target;

    [Tooltip("AI agression timer for how quickly it paths to its target.")]
    [SerializeField] private int agressionTimer;

    private NavMeshAgent agent;
    private HealthDamage healthDamage;

    /// <summary>
    /// Used for finite state machine.
    /// </summary>
    public bool isActive = true;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(target.transform.position);

        healthDamage = GetComponent<HealthDamage>();
    }

    // Update is called once per frame
    void Update()
    {
        // If the object is added into the target field
        if (target) {
            // If the enemy is within range, the enemy will stop to allow them to attack the player
            if (Vector3.Distance(this.transform.position, target.transform.position) < healthDamage.range) agent.isStopped = true;
            else agent.isStopped = false;

            if (!agent.isStopped) {
                if (Time.frameCount % agressionTimer == 0) {
                    agent.SetDestination(target.transform.position);
                }
            }
        }
    }

    /// <summary>
    /// Allows other scripts to add a new target for the AI at runtime.
    /// </summary>
    /// <param name="newTarget"></param>
    public void AddAgent(GameObject newTarget) {
        target = newTarget;
    }
}
