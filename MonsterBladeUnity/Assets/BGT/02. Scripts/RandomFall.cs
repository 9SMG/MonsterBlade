using System.Collections;
using UnityEngine;

public class RandomFall : MonoBehaviour
{
    private Projector mProjector;

    [SerializeField] private float mInitFovSize;       
    [SerializeField] private float mDeltaTimeMultiply; 
    [SerializeField] private float mMaxFovSize;        
    [SerializeField] private ParticleSystem particlePrefab; 
    [SerializeField] private float mParticleSpawnYPos;    
    [SerializeField] private float destroyDelay = 2.0f; 

    private void Awake()
    {
        mProjector = GetComponent<Projector>();
        Vector3 newPosition = mProjector.transform.position;
        newPosition.y = -21;
        mProjector.transform.position = newPosition; 

        Quaternion newRotation = mProjector.transform.rotation; 
        newRotation.eulerAngles = new Vector3(-270, newRotation.eulerAngles.y, newRotation.eulerAngles.z);
        mProjector.transform.rotation = newRotation; 
    }

    private void OnEnable()
    {
        mProjector.fieldOfView = mInitFovSize; 
        StartCoroutine(SpawnParticle()); 
    }

    void Update()
    {
        mProjector.fieldOfView += Time.deltaTime * mDeltaTimeMultiply;

        if (mProjector.fieldOfView > mMaxFovSize)
        {
            Destroy(gameObject, destroyDelay);
        }
    }

    public IEnumerator SpawnParticle()
    {
        yield return new WaitForSeconds(2.5f);

        Vector3 particlePosition = new Vector3(transform.position.x, mParticleSpawnYPos, transform.position.z);
        ParticleSystem newParticle = Instantiate(particlePrefab, particlePosition, Quaternion.identity);
        newParticle.Play();
    }
}