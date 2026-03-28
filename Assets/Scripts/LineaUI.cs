using UnityEngine;
using UnityEngine.UI;

// Dibuja una linea visual entre dos puntos usando una Image estirada y rotada
public class LineaUI : MonoBehaviour
{
    // La imagen que representa la linea
    private Image imagen;
    private RectTransform rectTransform;

    // Los dos puntos que conecta
    private Vector3 puntoA;
    private Vector3 puntoB;

    void Awake()
    {
        imagen = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
    }

    // Inicializa la linea con color y posiciones
    public void Inicializar(Vector3 desde, Vector3 hasta, Color color)
    {
        puntoA = desde;
        puntoB = hasta;
        imagen.color = color;
        ActualizarLinea();
    }

    void ActualizarLinea()
    {
        // Vector entre los dos puntos
        Vector3 diferencia = puntoB - puntoA;

        // La longitud de la linea es la distancia entre los dos puntos
        float distancia = diferencia.magnitude;

        // Posicionamos la linea en el punto medio entre A y B
        rectTransform.position = (puntoA + puntoB) / 2f;

        // El ancho de la linea es la distancia, el alto es fijo (grosor)
        rectTransform.sizeDelta = new Vector2(distancia, 6f);

        // Atan2 calcula el angulo en radianes entre los dos puntos
        // Mathf.Rad2Deg lo convierte a grados para Unity
        float angulo = Mathf.Atan2(diferencia.y, diferencia.x) * Mathf.Rad2Deg;

        // Rotamos la imagen para que apunte de A hacia B
        rectTransform.rotation = Quaternion.Euler(0, 0, angulo);
    }

    // Cambia el color de la linea
    public void SetColor(Color color)
    {
        imagen.color = color;
    }
}