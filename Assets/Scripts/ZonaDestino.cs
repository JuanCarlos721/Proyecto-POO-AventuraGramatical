using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;  // Para IDropHandler
using TMPro;
using System.Collections;

// IDropHandler hace que Unity llame a OnDrop() cuando un objeto
// arrastrable se suelta encima de este GameObject
public class ZonaDestino : MonoBehaviour, IDropHandler
{
    [Header("Datos de la zona")]
    // La categoría que acepta esta zona. Ej: "Verbo"
    // Debe coincidir EXACTAMENTE con la categoria de PalabraArrastrable
    public string categoriaAceptada;

    [Header("Referencias UI")]
    public TextMeshProUGUI textoFeedback;  // Texto "¡Correcto!" / "Incorrecto"

    [Header("Colores")]
    public Color colorCorrecto = Color.green;
    public Color colorIncorrecto = Color.red;
    public Color colorNormal;   // El color original de la zona

    // Image es el componente visual del fondo de la zona
    private Image imagenZona;

    // Referencia al ArrastrarActividad para notificarle cuando
    // se completa una palabra
    private ArrastrarActividad actividadPadre;

    void Awake()
    {
        imagenZona = GetComponent<Image>();
        colorNormal = imagenZona.color;

        // GetComponentInParent busca ArrastrarActividad en este
        // objeto o en cualquiera de sus padres en la jerarquía
        actividadPadre = GetComponentInParent<ArrastrarActividad>();
    }

    // OnDrop() es llamado por Unity automáticamente cuando
    // un objeto con IDragHandler se suelta sobre esta zona
    public void OnDrop(PointerEventData eventData)
    {
        // eventData.pointerDrag es el GameObject que se estaba arrastrando
        // Intentamos obtener el componente PalabraArrastrable de ese objeto
        PalabraArrastrable palabra = eventData.pointerDrag
            .GetComponent<PalabraArrastrable>();

        // Si el objeto soltado no tiene PalabraArrastrable, ignoramos
        if (palabra == null) return;

        // Comparamos la categoría de la palabra con la que acepta esta zona
        bool esCorrecta = palabra.categoria == categoriaAceptada;

        if (esCorrecta)
        {
            // Colocamos la palabra visualmente dentro de la zona
            palabra.Colocar(transform.position);

            // Notificar a ArrastrarActividad que hubo un acierto
            actividadPadre.RegistrarRespuesta(true, palabra.palabra);

            MostrarFeedback(true);
        }
        else
        {
            // notificar error la palabra se destruira con feedback
            actividadPadre.RegistrarRespuesta(false, palabra.palabra);

            MostrarFeedback(false);

            // Destruimos la palabra incorrecta después de mostrar el feedback
            // Destroy(objeto, segundos) lo elimina después de X segundos
            Destroy(eventData.pointerDrag, 1f);
        }
    }

    void MostrarFeedback(bool correcto)
    {
        textoFeedback.text = correcto ? "Correcto! " : "Incorrecto ";
        textoFeedback.color = correcto ? colorCorrecto : colorIncorrecto;

        // Cambiamos el color de fondo de la zona brevemente
        imagenZona.color = correcto ? colorCorrecto : colorIncorrecto;

        // Iniciamos una coroutine para restaurar el color después de 1 segundo
        StartCoroutine(RestaurarColor());
    }

    IEnumerator RestaurarColor()
    {
        yield return new WaitForSeconds(1f);

        // Regresar al color original de la zona
        imagenZona.color = colorNormal;
        textoFeedback.text = "";
    }
}