using UnityEngine;

public class SubtractHealth : MonoBehaviour
{
    public float health = 10f;

    public void Subtract(float damage)
    {
        health -= damage;
        
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
