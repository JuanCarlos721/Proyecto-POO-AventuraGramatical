using UnityEngine;
using UnityEngine.SceneManagement;

public class PantallaCreditos : MonoBehaviour
{
    // Regresa al menu principal
    public void Regresar()
    {
        SceneManager.LoadScene("Menu");
    }
}