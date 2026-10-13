using UnityEngine;

public class OrchestratedEntity : MonoBehaviour, ITickable
{
    void OnEnable()
    {
        UpdateOrchestra.Instance.Register(this);
    }

    void OnDisable()
    {
        UpdateOrchestra.Instance.Unregister(this);
    }
    public void Tick(float deltaTime)
    {

    }

}
