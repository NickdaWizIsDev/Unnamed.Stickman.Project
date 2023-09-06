using UnityEngine;

public class Damage : MonoBehaviour
{
    public float damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"));
        {   
            Damageable damageable;
            damageable = other.GetComponentInParent<Damageable>();

            damageable.Hit(damage);
        }
    }
}