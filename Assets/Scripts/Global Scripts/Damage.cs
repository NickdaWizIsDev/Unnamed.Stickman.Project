using UnityEngine;

public class Damage : MonoBehaviour
{
    public float damage;

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.CompareTag("Player"))
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                Damageable damageable;
                damageable = other.GetComponentInParent<Damageable>();

                damageable.Hit(damage);
            }
        }
        
        else if (gameObject.CompareTag("Enemy"))
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Damageable damageable;
                damageable = other.GetComponentInParent<Damageable>();

                damageable.Hit(damage);
            }
        }
    }
}