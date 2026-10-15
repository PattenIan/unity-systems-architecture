using UnityEngine;

public sealed class UpdateOrchestra : MonoBehaviour
{
    public static UpdateOrchestra Instance { get; private set; }

    private ITickable[] tickables;
    private int count;

    private void Awake()
    {
        if (Instance != null) { Destroy(this); } else { Instance = this; }
    }

    public void Initialize(int capacity)
    {
        tickables = new ITickable[capacity];
        count = 0;
    }

    public void Register(ITickable tickable)
    {
        if (count < tickables.Length)
        {
            tickables[count++] = tickable;
        }
    }

    public void Unregister(ITickable tickable)
    {
        for (int i = 0; i < count; i++)
        {
            if (ReferenceEquals(tickables[i], tickable))
            {
                tickables[i] = tickables[count - 1];
                tickables[count - 1] = null;
                count--;
                return;
            }
        }
    }

    void Update()
    {
        float dt = Time.deltaTime;
        for (int i = 0; i < count; i++)
        {
            ITickable item = tickables[i];
            if (!ReferenceEquals(item, null))
            {
                item.Tick(dt);
            }
        }
    }
}
