using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] int value;
    [SerializeField] ParticleSystem deathParticles;

    public int Value { get { return value; } }

    public void SpawnDeathParticles()
    {
        ParticleSystem newParticles = Instantiate(deathParticles);
        newParticles.gameObject.transform.parent = null;
        newParticles.transform.position = transform.position;
    }
}
