using UnityEngine;

namespace Complete
{
    public class EggExplode : MonoBehaviour
    {
        public LayerMask m_TankMask;                        // Used to filter what the explosion affects, this should be set to "Players".
        //public ParticleSystem m_ExplosionParticles;         // Reference to the particles that will play on explosion.
        //public AudioSource m_ExplosionAudio;                // Reference to the audio that will play on explosion.
        public float m_Damage = 100f;                    // The amount of damage done if the explosion is centred on a tank.
        public float m_ExplosionForce = 1000f;              // The amount of force added to a tank at the centre of the explosion.
        public float m_ExplosionRadius = 5f;                // The maximum distance away from the explosion tanks can be and are still affected.
        //public GameObject m_eggPrefab;

        public GameObject m_ExplosionPrefab;
        public AudioSource m_ExplosionAudio;               // The audio source to play when the tank explodes.
        private ParticleSystem m_ExplosionParticles;        // The particle system the will play when the tank is destroyed.

        //private void Awake()
        //{
        //    m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();

        //    m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource>();

        //    // Disable the prefab so it can be activated when it's required.
        //    m_ExplosionParticles.gameObject.SetActive(false);
        //}

        private void Start()
        {
            //m_ExplosionParticles = GetComponent<ParticleSystem>();
            m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();

            m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource>();

            // Disable the prefab so it can be activated when it's required.
            m_ExplosionParticles.gameObject.SetActive(false);
        }

        
        
        private void OnCollisionEnter(Collision collision)
        {
            // Collect all the colliders in a sphere from the shell's current position to a radius of the explosion radius.
            Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);

            // Go through all the colliders...
            for (int i = 0; i < colliders.Length; i++)
            {
                // ... and find their rigidbody.

                Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();

                // If they don't have a rigidbody, go on to the next collider.
                if (!targetRigidbody)
                    continue;

                // Add an explosion force.

                targetRigidbody.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius);

                // Find the TankHealth script associated with the rigidbody.
                TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth>();

                // If there is no TankHealth script attached to the gameobject, go on to the next collider.
                if (!(targetHealth))
                    continue;

                // Calculate the amount of damage the target should take based on it's distance from the shell.


                // Deal this damage to the tank.
                targetHealth.TakeDamage(m_Damage);

                m_ExplosionParticles.transform.position = transform.position;
                m_ExplosionParticles.gameObject.SetActive(true);

                m_ExplosionParticles.Play();

                m_ExplosionAudio.Play();

                gameObject.SetActive(false);

                // Unparent the particles from the shell.
                //m_ExplosionParticles.transform.parent = null;

                // Play the particle system.
                //m_ExplosionParticles.Play();

                // Play the explosion sound effect.
                //m_ExplosionAudio.Play();

                // Once the particles have finished, destroy the gameobject they are on.
                //ParticleSystem.MainModule mainModule = m_ExplosionParticles.main;
                //Destroy(m_ExplosionParticles.gameObject, mainModule.duration);

                // Destroy the shell.
                Destroy(gameObject);
            }
        }
    }
}