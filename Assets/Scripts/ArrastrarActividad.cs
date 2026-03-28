using UnityEngine;
using TMPro;

// Esta es la clase principal que coordina toda la actividad
public class ArrastrarActividad : MonoBehaviour
{
    [Header("Palabras en juego")]
    // Asigna desde el Inspector todos los GameObjects de palabras
    public PalabraArrastrable[] palabras;

    [Header("UI General")]
    public TextMeshProUGUI textoProgreso;  // Ej: "3 / 6 palabras"

    // Contadores internos
    private int palabrasCorrectas = 0;
    private int palabrasRespondidas = 0;
    private int totalPalabras;

    void Start()
    {
        // Length devuelve el numero de cuantas palabras hay
        totalPalabras = palabras.Length;
        ActualizarProgreso();
    }

    // Las ZonaDestino llaman a este metodo cada vez que se suelta
    // una palabra, ya sea correcta o incorrecta
    public void RegistrarRespuesta(bool correcta, string palabraUsada)
    {
        palabrasRespondidas++;

        if (correcta)
            palabrasCorrectas++;

        ActualizarProgreso();

        // Si ya se respondieron todas las palabras, evaluar el resultado
        if (palabrasRespondidas >= totalPalabras)
        {
            int estrellas = 0;
            if (palabrasCorrectas == totalPalabras) estrellas = 3;
            else if (palabrasCorrectas >= totalPalabras / 2) estrellas = 2;
            else if (palabrasCorrectas > 0) estrellas = 1;

            Juego.Instance.NivelCompletado(estrellas);
        }
    }

    void ActualizarProgreso()
    {
        // directamente dentro del texto con {}
        textoProgreso.text = $"{palabrasCorrectas} / {totalPalabras} palabras";
    }

    // Devuelve true si el jugador acerto mas de la mitad
    public bool Evaluar(object respuesta)
    {
        int correctas = (int)respuesta;  // (int) convierte el objeto a entero
        return correctas >= totalPalabras / 2;
    }


}