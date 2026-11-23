using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GestorPartida : MonoBehaviour
{
    public int meta = 10; // Total de paquetes a entregar
    public int capacidadCamioneta = 5;
    public float tiempoRestante = 60f;
    public float penalizacionChoque = 1f; // Segundos perdidos al chocar con coches
    public Renderer estacionCorreosRenderer; // Para pintarla de color como aviso
    public GameObject[] todasLasCasas; // Todas las casas
    public int entregados = 0;
    public int enCamioneta = 0;
    private bool terminada = false;
    public bool MisionCumplida
    {
        get { return entregados >= meta; }
    }
    public TMP_Text textoEntregas;
    public TMP_Text textoCarga;
    public TMP_Text textoTiempo;
    public TMP_Text textoResultado;

    void Start()
    {
        if (textoResultado != null) textoResultado.gameObject.SetActive(false);
        // Al iniciar se cargan 5 paquetes y activa 5 casas
        RecargarPaquetes();
        ActualizarUI();
    }

    void Update()
    {
        if (terminada)
        {
            if (Input.GetKeyDown(KeyCode.R)) Reiniciar();
            return;
        }

        // Cuenta regresiva
        tiempoRestante -= Time.deltaTime;
        if (tiempoRestante <= 0f)
        {
            tiempoRestante = 0f;
            ActualizarUI();
            Perder();
            return;
        }
        ActualizarUI();
    }

    public void Entregar(GameObject casa)
    {
        if (terminada || enCamioneta <= 0) return;

        enCamioneta--;
        entregados++;
        casa.SetActive(false);
        Debug.Log("Paquete entregado (" + entregados + " de " + meta + ")");

        if (MisionCumplida)
        {
            Debug.Log("Todos entregados. Ve a la estación de correos.");
            if (estacionCorreosRenderer != null) estacionCorreosRenderer.material.color = Color.green;
        }
        else if (enCamioneta == 0)
        {
            Debug.Log("Vuelve a la estación para recargar.");
            if (estacionCorreosRenderer != null) estacionCorreosRenderer.material.color = Color.yellow;
        }
    }

    public void VisitarCorreos()
    {
        if (terminada) return;

        if (MisionCumplida)
        {
            Ganar();
        }
        else if (enCamioneta == 0)
        {
            RecargarPaquetes();
            if (estacionCorreosRenderer != null) estacionCorreosRenderer.material.color = Color.white;
            ActualizarUI();
        }
    }

    public void PenalizarChoque()
    {
        if (terminada) return;
        tiempoRestante -= penalizacionChoque;
        Debug.Log("¡Choque! -" + penalizacionChoque + "segs");
        if (tiempoRestante <= 0f)
        {
            tiempoRestante = 0f;
            ActualizarUI();
            Perder();
            return;
        }
        ActualizarUI();
    }

    private void RecargarPaquetes()
    {
        enCamioneta = capacidadCamioneta;

        for (int i = 0; i < todasLasCasas.Length; i++)
            todasLasCasas[i].SetActive(false);

        List<int> disponibles = new List<int>();
        for (int i = 0; i < todasLasCasas.Length; i++)
            disponibles.Add(i);

        int cantidad = Mathf.Min(capacidadCamioneta, todasLasCasas.Length);
        for (int i = 0; i < cantidad; i++)
        {
            int azar = Random.Range(0, disponibles.Count);
            todasLasCasas[disponibles[azar]].SetActive(true);
            disponibles.RemoveAt(azar);
        }
    }

    void ActualizarUI()
    {
        if (textoEntregas != null)
            textoEntregas.text = "Entregados: " + entregados + " / " + meta;

        if (textoTiempo != null)
            textoTiempo.text = "Tiempo: " + Mathf.CeilToInt(tiempoRestante) + "s";

        if (textoCarga != null)
        {
            if (MisionCumplida)
                textoCarga.text = "¡10/10! Vuelve al edificio";
            else if (enCamioneta == 0)
                textoCarga.text = "Sin paquetes: Vuelve a Correos";
            else
                textoCarga.text = "En camioneta: " + enCamioneta + " / " + capacidadCamioneta;
        }
    }

    public void Ganar()
    {
        if (terminada) return;
        terminada = true;
        Debug.Log("¡Ganaste! Presiona R para reiniciar.");

        if (textoResultado != null)
        {
            textoResultado.gameObject.SetActive(true);
            textoResultado.text = "¡Ganaste! Presiona R para reiniciar.";
            textoResultado.color = Color.green;
        }
    }

    public void Perder()
    {
        if (terminada) return;
        terminada = true;
        Debug.Log("Perdiste");
        if (estacionCorreosRenderer != null) estacionCorreosRenderer.material.color = Color.red;
        if (textoResultado != null)
        {
            textoResultado.gameObject.SetActive(true);
            textoResultado.text = "¡SE ACABÓ EL TIEMPO!";
            textoResultado.color = Color.red;
        }
        Invoke("Reiniciar", 2.0f); // Muestra mensaje y reinicia solo
    }

    void Reiniciar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}