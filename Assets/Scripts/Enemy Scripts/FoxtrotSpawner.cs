using UnityEngine;
using UnityEngine.AI;

public class FoxtrotSpawner : MonoBehaviour
{
    private float timer;

    [SerializeField] private GameObject foxtrotPrefab;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > 15) {
            Debug.Log("<color=blue>A Foxtrot is coming for the body part!</color>");
            var foxtrot = GameObject.Instantiate(foxtrotPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
            foxtrot.GetComponent<EnemyPathing>().AddAgent(this.gameObject);
            foxtrot.GetComponent<NavMeshAgent>().SetDestination(this.transform.position);
            timer = 0f;
        }
    }
}
