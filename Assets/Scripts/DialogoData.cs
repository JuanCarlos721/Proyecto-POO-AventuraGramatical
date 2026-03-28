using UnityEngine;

// script para crear dialogos en el menu de assets
// menu del asset
[CreateAssetMenu(fileName = "NuevoDialogo", menuName = "Dialogo / Datos del dialogo")]

public class DialogoData : ScriptableObject
{
    // arreglo de lineas de dialogo que forman el dialogo completo
    public LineaDialogo[] lineas;
}

//System.Serializable hace que unity muestre esta clase
//como un bloque editable en el inspector

[System.Serializable]
public class LineaDialogo
{
    public string nombrePersonaje;
    public Sprite spritePersonaje;
    public AudioClip audioLinea; // sonido mientras se despliega su dialogo

    // el texto aparece con efecto de maquina de escribir
    // [TextArea] muestra un campo de texto grande en el inspector
    [TextArea(2, 5)]
    public string texto;
}
