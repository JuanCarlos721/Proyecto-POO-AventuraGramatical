using JetBrains.Annotations;
using UnityEngine;

public class Juego : MonoBehaviour
{
    public static Juego Instance;

    public int nivelActual = 0;
    public int estrellasObtenidas = 0;


    // Awake se ejecuta antes que Start, para determinar que se esta en el nivel 0 (menu principal) y evitar errores de mas de 1 instancia de Juego
    private void Awake()
    {
        // si no hay instancia de Juego, establecer esta instancia
        if (Instance == null)
        {
            Instance = this;
        }

        else Destroy(gameObject); // Si habia una instancia de Juego, destruirla
        DontDestroyOnLoad(gameObject); // No destruir esta la nueva instancia al cambiar de escena
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    public void SelleccionarNivel(int id)
    {
        nivelActual = id;
    }

    public void GuardarEstrellas(int estrellas)
    {
        estrellasObtenidas += estrellas;

        // PlayerPrefs guarda datos en el disco, para guardar la partida
        // con SetInt se escribe un entero con el nombre "TotalEstrellas"
        PlayerPrefs.SetInt("TotalEstrellas", estrellasObtenidas);
    }

}
