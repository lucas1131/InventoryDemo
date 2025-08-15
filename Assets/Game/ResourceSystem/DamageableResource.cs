using InventoryDemo.Items;
using UnityEngine;

namespace Game.ResourceSystem
{
    // The spawn and damage logic could be separated into two different components, with the damage component only triggering a DamageTaken event
    public class DamageableResource : MonoBehaviour
    {
        [SerializeField] private int health = 10;
        [SerializeField] private int resourceModifierWhenDestroyed = 3;
        [SerializeField] private PickableItem droppedResource;
        public PickableItem DroppedResource => droppedResource;

        public void Damage(int damage)
        {
            health -= damage;
            Debug.Log($"'{name}' was hit for {damage} damage!");
            if (health <= 0)
            {
                Die();
            }
            else
            {
                SpawnResource();
            }
        }
        
        private Vector3 RandomVelocity(float min, float max)
        {
            Vector3 direction = Random.insideUnitCircle.normalized;
            return direction * Random.Range(min, max);
        }
        
        public void SpawnResource()
        {
            PickableItem newItem = Instantiate(droppedResource, transform.position, Quaternion.identity);
            newItem.Rigidbody?.AddForce(RandomVelocity(1f, 3f), ForceMode.VelocityChange); // Rb here is optional
        }

        public void Die()
        {
            for (int i = 0; i < resourceModifierWhenDestroyed; i++)
            {
                SpawnResource();
            }
            
            Debug.Log($"'{name}' is bread!");
            Destroy(gameObject);
        }
    }
}