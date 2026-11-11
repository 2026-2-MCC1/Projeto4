using System.Collections.Generic;
using UnityEngine;

public class TortuguitaInventory : MonoBehaviour
{
    // Armazena as peças coletadas
    private static Dictionary<string, GameObject> pecasColetadas =
        new Dictionary<string, GameObject>();

    // Registra uma peça coletada
    public static void RegistrarPeca(string tag, GameObject objeto)
    {
        if (!pecasColetadas.ContainsKey(tag))
        {
            pecasColetadas.Add(tag, objeto);

            // Esconde a peça até ela ser montada
            objeto.SetActive(false);

            Debug.Log("Peça armazenada: " + tag);
        }
    }

    // Verifica se a peça já foi coletada
    public static bool PossuiPeca(string tag)
    {
        return pecasColetadas.ContainsKey(tag);
    }

    // Retorna a peça armazenada
    public static GameObject ObterPeca(string tag)
    {
        if (pecasColetadas.ContainsKey(tag))
        {
            return pecasColetadas[tag];
        }

        return null;
    }
}
