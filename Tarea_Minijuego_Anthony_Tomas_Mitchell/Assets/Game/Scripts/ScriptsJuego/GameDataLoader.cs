using UnityEngine;
using System.IO;

public class GameDataLoader : MonoBehaviour
{
    public static GameDataLoader Instance;
    public ListaDatos datos;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CargarDatos();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void CargarDatos()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "FruitsData.json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            datos = JsonUtility.FromJson<ListaDatos>(json);
            Debug.Log("JSON cargado correctamente");
        }
        else
        {
            Debug.LogError("No se encontró el JSON en: " + path);
        }
    }
}