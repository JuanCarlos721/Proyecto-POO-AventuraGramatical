using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ElementoConectable : MonoBehaviour
{
    [Header("Datos")]
    public string contenido; // texto en pantalla
    public string idConexion; // id de la pareja
    public bool esColumnaA; // la columna izuierda es false, la derecha es true

    [Header("Referencias")]
    public TextMeshProUGUI texto;
    public Image fondoImagen;

    // colores de feedback al seleccionar elementos
    [Header("Colores")]
    public Color colorNormal;
    public Color colorSeleccionado = Color.yellow;
    public Color colorCorrecto = Color.green;
    public Color colorIncorrecto = Color.red;

    // true si la conexion es correcta
    private bool conectado = false;

    // referencia para el ConectarActividad
    private ConectarActividad manager;

    void Start()
    {
        texto.text = contenido;
    }

    private void Awake()
    {
        fondoImagen = GetComponent<Image>();
        colorNormal = fondoImagen.color;
        manager = FindFirstObjectByType<ConectarActividad>();
    }

    public void OnClick()
    {
        // si ya esta conectado, no hacer nada
        if (conectado){
            return; 
        }

        // Avisar al manager que este elemento fue clicado
        manager.RegistrarClick(this);
    }

    // cambia el estado del elemento segun si se selecciona, acierta o falla
    public void SetEstado(string estado)
    {
        switch (estado)
        {
            case "seleccionado":
                fondoImagen.color = colorSeleccionado;
                break;
            case "correcto":
                fondoImagen.color = colorCorrecto;
                conectado = true;
                break;
            case "incorrecto":
                fondoImagen.color = colorIncorrecto;
                // se resetea poco despues
                break;
            case "normal":
                fondoImagen.color = colorNormal;
                break;
        }
    }

    // devuelve la posicion del centro del elemento en la pantalla, para ubicar mejor la conexion
    public Vector3 GetPosicion()
    {
        return transform.position;
    }

}
