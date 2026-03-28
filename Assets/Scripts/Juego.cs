using UnityEngine;
using UnityEngine.SceneManagement;

public class Juego : MonoBehaviour
{
    public static Juego Instance;

    [Header("Dialogos por secuencia")]
    // Arrastra aqui cada DialogoData desde el Project
    public DialogoData dialogoIntroduccion;   // Antes del nivel 1
    public DialogoData dialogoEntreNiveles;   // Entre nivel 1 y nivel 2
    public DialogoData dialogoFinal;          // Despues del ultimo nivel

    [Header("Puntuacion")]
    public int estrellasNivel1 = 0;
    public int estrellasNivel2 = 0;
    public int estrellasNivel3 = 0;

    // Controla en que punto del juego estamos
    // El enum hace el codigo mas legible que usar numeros
    public enum EstadoJuego
    {
        Menu,
        Dialogo,
        NivelArrastrar,
        PuntosNivel1,
        NivelClick,
        PuntosNivel2,
        NivelConectar,
        PuntosFinales
    }

    public EstadoJuego estadoActual = EstadoJuego.Menu;

    // Dialogo que se va a mostrar en la escena de dialogo
    // DialogoManager lo leera al iniciar
    [HideInInspector]  // Oculto en Inspector porque se asigna por codigo
    public DialogoData dialogoPendiente;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }
        DontDestroyOnLoad(gameObject);
    }

    // Llamado desde el boton "Jugar" en el Menu
    public void IniciarJuego()
    {
        estadoActual = EstadoJuego.Dialogo;
        dialogoPendiente = dialogoIntroduccion;
        SceneManager.LoadScene("Dialogo");
    }

    // Llamado por DialogoManager cuando termina el dialogo
    public void DialogoCompletado()
    {
        // Dialogo introduccion → nivel 1 (arrastrar)
        if (dialogoPendiente == dialogoIntroduccion)
        {
            estadoActual = EstadoJuego.NivelArrastrar;
            SceneManager.LoadScene("NivelArrastrarActividad");
        }
        // Dialogo entre niveles → nivel 2 (click)
        else if (dialogoPendiente == dialogoEntreNiveles)
        {
            estadoActual = EstadoJuego.NivelClick;
            SceneManager.LoadScene("NivelClickActividad");
        }
        // Dialogo final → nivel 3 (conectar)
        else if (dialogoPendiente == dialogoFinal)
        {
            estadoActual = EstadoJuego.NivelConectar;
            SceneManager.LoadScene("NivelConectarActividad");
        }
    }

    // Llamado cuando termina cada nivel con las estrellas obtenidas
    public void NivelCompletado(int estrellas)
    {
        switch (estadoActual)
        {
            case EstadoJuego.NivelArrastrar:
                estrellasNivel1 = estrellas;
                estadoActual = EstadoJuego.PuntosNivel1;
                SceneManager.LoadScene("Resultado");
                break;

            case EstadoJuego.NivelClick:
                estrellasNivel2 = estrellas;
                estadoActual = EstadoJuego.PuntosNivel2;
                SceneManager.LoadScene("Resultado");
                break;

            case EstadoJuego.NivelConectar:
                estrellasNivel3 = estrellas;
                estadoActual = EstadoJuego.PuntosFinales;
                SceneManager.LoadScene("Resultado");
                break;
        }
    }

    // Llamado desde la pantalla de Resultado al presionar "Continuar"
    public void ContinuarDesdePuntos()
    {
        switch (estadoActual)
        {
            // Después del nivel 1 → diálogo entre niveles
            case EstadoJuego.PuntosNivel1:
                estadoActual = EstadoJuego.Dialogo;
                dialogoPendiente = dialogoEntreNiveles;
                SceneManager.LoadScene("Dialogo");
                break;

            // Después del nivel 2 → diálogo final
            case EstadoJuego.PuntosNivel2:
                estadoActual = EstadoJuego.Dialogo;
                dialogoPendiente = dialogoFinal;
                SceneManager.LoadScene("Dialogo");
                break;

            // Después del nivel 3 → regresa al menú
            case EstadoJuego.PuntosFinales:
                estadoActual = EstadoJuego.Menu;
                SceneManager.LoadScene("Menu");
                break;
        }
    }

    public void SalirJuego()
    {
        Application.Quit();
    }
}