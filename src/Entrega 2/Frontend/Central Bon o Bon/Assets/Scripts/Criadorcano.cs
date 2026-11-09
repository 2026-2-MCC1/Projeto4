using UnityEditor;
using UnityEngine;

// Cria um cano (cilindro) ligando dois objetos da cena.
// Como usar: na Hierarchy, selecione 2 objetos (segure Ctrl), depois clique em
// Protocolo Arcor > Criar cano entre 2 objetos.
// Dica: crie dois "Empty" (botão direito > Create Empty), coloque nos pontos
// onde o cano começa e termina, selecione os dois e rode o menu.
public class CriadorCano
{
    // espessura do cano em metros (diâmetro)
    const float espessura = 0.2f;

    [MenuItem("Protocolo Arcor/Criar cano entre 2 objetos (azul)")]
    static void CriarCanoAzul()
    {
        CriarCano("CanoAzul", new Color(0.1f, 0.3f, 0.7f), espessura);
    }

    [MenuItem("Protocolo Arcor/Criar cano entre 2 objetos (amarelo)")]
    static void CriarCanoAmarelo()
    {
        CriarCano("CanoAmarelo", new Color(0.9f, 0.7f, 0.1f), espessura * 0.75f);
    }

    static void CriarCano(string nomeMaterial, Color cor, float grossura)
    {
        // precisa de exatamente 2 objetos selecionados
        GameObject[] selecionados = Selection.gameObjects;
        if (selecionados.Length != 2)
        {
            EditorUtility.DisplayDialog("Criar cano", "Selecione exatamente 2 objetos na Hierarchy (segure Ctrl para selecionar o segundo).", "OK");
            return;
        }

        Vector3 inicio = selecionados[0].transform.position;
        Vector3 fim = selecionados[1].transform.position;

        // o comprimento do cano é a distância entre os dois pontos
        float comprimento = Vector3.Distance(inicio, fim);
        if (comprimento < 0.01f)
        {
            EditorUtility.DisplayDialog("Criar cano", "Os dois objetos estão na mesma posição.", "OK");
            return;
        }

        GameObject cano = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        cano.name = "Cano";

        // fica no meio dos dois pontos
        cano.transform.position = (inicio + fim) / 2f;

        // o cilindro do Unity é "em pé" (eixo Y). Apontamos o Y dele para o ponto final.
        cano.transform.up = fim - inicio;

        // o cilindro tem 2 m de altura com escala 1, por isso o Y é metade do comprimento
        cano.transform.localScale = new Vector3(grossura, comprimento / 2f, grossura);

        // cano de teto não precisa de colisão
        Object.DestroyImmediate(cano.GetComponent<Collider>());

        cano.GetComponent<Renderer>().sharedMaterial = PegarMaterial(nomeMaterial, cor);

        // se um dos objetos tem pai (ex.: "Galpao"), o cano entra no mesmo pai
        Transform pai = selecionados[0].transform.parent;
        if (pai != null)
        {
            cano.transform.SetParent(pai, true);
        }

        Undo.RegisterCreatedObjectUndo(cano, "Criar cano");
        Selection.activeGameObject = cano;
    }

    // pega o material da pasta (ou cria se ainda não existir)
    static Material PegarMaterial(string nome, Color cor)
    {
        if (AssetDatabase.IsValidFolder("Assets/Materials") == false)
        {
            AssetDatabase.CreateFolder("Assets", "Materials");
        }

        string caminho = "Assets/Materials/" + nome + ".mat";
        Material material = AssetDatabase.LoadAssetAtPath<Material>(caminho);

        if (material == null)
        {
            // o material padrão de um cubo já vem com o shader certo (Built-in ou URP)
            GameObject teste = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Material padrao = teste.GetComponent<Renderer>().sharedMaterial;
            Object.DestroyImmediate(teste);

            material = new Material(padrao);
            material.color = cor;
            AssetDatabase.CreateAsset(material, caminho);
            AssetDatabase.SaveAssets();
        }

        return material;
    }
}