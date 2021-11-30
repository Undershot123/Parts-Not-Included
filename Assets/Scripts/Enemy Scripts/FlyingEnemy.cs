using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private GameObject projectile;
    private float moveSpeed;
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = GetComponent<HealthDamage>().speed;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > Random.RandomRange(3f,5f)) {
            GetComponent<Rigidbody>().velocity = new Vector3(Random.RandomRange(-moveSpeed,moveSpeed), 0f, Random.RandomRange(-moveSpeed, moveSpeed));

            if(Vector3.Distance(transform.position, target.position) < 35) {
                GameObject newProj = Instantiate(projectile, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                newProj.GetComponent<Rigidbody>().velocity = (target.position - newProj.transform.position) / 1.5f;
            }

            timer = 0f;
        }
    }
}
