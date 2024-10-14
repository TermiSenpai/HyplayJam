using UnityEngine;
using System.IO;

public class ScreenshotCapture : MonoBehaviour
{
    // Nombre del archivo de la captura de pantalla
    public string screenshotName = "screenshot";

    // Ruta personalizada para guardar las capturas de pantalla
    public string screenshotFolder = "Screenshots";

    void Start()
    {
        // Crear la carpeta si no existe
        if (!Directory.Exists(screenshotFolder))
        {
            Directory.CreateDirectory(screenshotFolder);
            Debug.Log("Carpeta de capturas creada en: " + screenshotFolder);
        }
    }

    void Update()
    {
        // Captura la pantalla cuando el usuario presiona la tecla P
        if (Input.GetKeyDown(KeyCode.P))
        {
            // Genera el nombre del archivo con una marca de tiempo para evitar sobreescribir
            string fileName = screenshotName + "_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";

            // Combina la ruta de la carpeta y el nombre del archivo
            string fullPath = Path.Combine(screenshotFolder, fileName);

            // Captura la pantalla y la guarda en la ruta especificada
            ScreenCapture.CaptureScreenshot(fullPath);
            Debug.Log("Captura de pantalla guardada en: " + fullPath);
        }
    }
}
