using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Player") {
            collision.gameObject.GetComponent<HealthDamage>().TakeDamage(GetComponent<HealthDamage>().attackDamage);
        }

        Destroy(this.gameObject);
    }
}
