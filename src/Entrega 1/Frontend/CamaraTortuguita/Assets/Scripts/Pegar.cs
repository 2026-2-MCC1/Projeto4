using UnityEngine;
using System.Collections.Generic;

public class Pegar : MonoBehaviour
{
    public Camera cam;
    public Transform mao;

    Rigidbody objeto;
    List<Collider> collidersDesativados = new List<Collider>();

    void Update()
    {
        Debug.DrawRay(cam.transform.position, cam.transform.forward * 3f, Color.red);

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (objeto == null) Pegar_();
            else Soltar();
        }
    }

    void Pegar_()
    {
        if (!Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, 3f))
            return;

        Rigidbody rb = hit.rigidbody;
        if (rb == null) return;

        // Nunca pegar algo que contenha o jogador (ou seja o próprio jogador)
        if (cam.transform.IsChildOf(rb.transform))
        {
            Debug.LogWarning("Rigidbody acertado contém o Player: " + rb.name);
            return;
        }

        objeto = rb;
        objeto.isKinematic = true;

        collidersDesativados.Clear();
        foreach (var col in objeto.GetComponentsInChildren<Collider>())
        {
            if (col is CharacterController) continue;
            if (!col.enabled) continue;

            col.enabled = false;
            collidersDesativados.Add(col);
        }

        objeto.transform.SetParent(mao);
        objeto.transform.localPosition = Vector3.zero;
        objeto.transform.localRotation = Quaternion.identity;
    }

    void Soltar()
    {
        objeto.transform.SetParent(null);

        foreach (var col in collidersDesativados)
            if (col != null) col.enabled = true;

        collidersDesativados.Clear();

        objeto.isKinematic = false;
        objeto = null;
    }
}