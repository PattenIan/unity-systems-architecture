using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class TickingBenchmarkHarness : MonoBehaviour
{
    bool useOrchestraToggle = false;
    [SerializeField] int spawnCount = 15000;
    [SerializeField] GameObject standardPrefab;
    [SerializeField] GameObject orchestraPrefab;

    List<GameObject> activeInstances = new List<GameObject>();

    float[] frameDeltaBuffer = new float[60];
    int bufferIndex = 0;
    float smoothedMs = 0.0f;

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = -1;
    }

    private void Start()
    {
        //Moved from Awake to make sure UpdateOrchestra is instantiated
        if (UpdateOrchestra.Instance != null)
        {
            UpdateOrchestra.Instance.Initialize(spawnCount);
        }
        SpawnBenchmarkSet();
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            ToggleModeAndRespawn();
        }

        float currentMs = Time.unscaledDeltaTime * 1000.0f;
        frameDeltaBuffer[bufferIndex] = currentMs;
        bufferIndex = (bufferIndex + 1) % frameDeltaBuffer.Length;

        float sum = 0f;
        for (int i = 0; i < frameDeltaBuffer.Length; i++)
        {
            sum += frameDeltaBuffer[i];
        }
        smoothedMs = sum / frameDeltaBuffer.Length;
    }

    private void ToggleModeAndRespawn()
    {

        useOrchestraToggle = !useOrchestraToggle;
        ClearActiveInstances();
        SpawnBenchmarkSet();
    }

    private void ClearActiveInstances()
    {
        foreach (GameObject instance in activeInstances)
        {
            Destroy(instance);
        }
        activeInstances.Clear();
    }

    private void SpawnBenchmarkSet()
    {
        GameObject prefabToUse = (useOrchestraToggle == true) ? orchestraPrefab : standardPrefab;
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 randPos = new Vector3(Random.Range(-50, 50), Random.Range(-50, 50), Random.Range(-50, 50));
            GameObject instance = Instantiate(prefabToUse, randPos, Quaternion.identity);
            activeInstances.Add(instance);
        }
    }

    private void OnGUI()
    {
        string modeName = useOrchestraToggle ? "UpdateOrchestra (ITickable)" : "Standard Unity Update()";

        GUI.Box(new Rect(10, 10, 320, 110), "Benchmark Metrics");
        GUI.Label(new Rect(20, 35, 300, 20), $"Mode: {modeName}");
        GUI.Label(new Rect(20, 55, 300, 20), $"Spawn Count: {spawnCount:N0}");
        GUI.Label(new Rect(20, 75, 300, 20), $"Frame Time: {smoothedMs:F2} ms");
        GUI.Label(new Rect(20, 95, 300, 20), "Press [R] to switch mode");
    }
}
