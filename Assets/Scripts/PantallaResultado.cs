using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PantallaResultado : MonoBehaviour
{
    [Header("Referencias UI")]
    public TextMeshProUGUI textoTitulo;
    public TextMeshProUGUI textoPuntos;
    public Button botonContinuar;

    [Header("Estrellas")]
    // Los 3 objetos Image de las estrellas
    public Image[] imagenesEstrellas = new Image[3];
    // Sprite de estrella llena (asignar en Inspector)
    public Sprite estrellaLlena;
    // Sprite de estrella vacía (asignar en Inspector)
    public Sprite estrellaVacia;

    void Start()
    {
        botonContinuar.onClick.AddListener(AlPresionarContinuar);
        MostrarResultado();
    }

    void MostrarResultado()
    {
        int estrellas = 0;

        switch (Juego.Instance.estadoActual)
        {
            case Juego.EstadoJuego.PuntosNivel1:
                estrellas = Juego.Instance.estrellasNivel1;
                break;
            case Juego.EstadoJuego.PuntosNivel2:
                estrellas = Juego.Instance.estrellasNivel2;
                break;
            case Juego.EstadoJuego.PuntosFinales:
                estrellas = Juego.Instance.estrellasNivel3;
                break;
        }

        // Título según puntaje
        if (estrellas >= 3) textoTitulo.text = "¡Excelente!";
        else if (estrellas == 2) textoTitulo.text = "¡Muy bien!";
        else if (estrellas == 1) textoTitulo.text = "¡Buen intento!";
        else textoTitulo.text = "¡Sigue practicando!";

        // Activar estrella llena o vacía según el puntaje
        // i=0 es la primera estrella, i=1 la segunda, i=2 la tercera
        for (int i = 0; i < imagenesEstrellas.Length; i++)
        {
            imagenesEstrellas[i].sprite = (i < estrellas) ? estrellaLlena : estrellaVacia;
        }

        textoPuntos.text = $"{estrellas} de 3 estrellas";
    }

    void AlPresionarContinuar()
    {
        Juego.Instance.ContinuarDesdePuntos();
    }
}