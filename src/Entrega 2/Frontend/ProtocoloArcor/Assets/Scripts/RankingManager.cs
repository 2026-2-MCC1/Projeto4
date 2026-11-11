using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class JogadorRanking
{
    public string nome;
    public float tempo;

    public JogadorRanking(string nome, float tempo)
    {
        this.nome = nome;
        this.tempo = tempo;
    }
}

[Serializable]
public class ListaJogadores
{
    public List<JogadorRanking> jogadores = new List<JogadorRanking>();
}

public static class RankingManager
{
    private static List<float> temposFases = new List<float>();

    private const string CHAVE_RANKING = "RankingArcor";

    // Registra o tempo de uma fase concluída
    public static void RegistrarTempoFase(float tempo)
    {
        temposFases.Add(tempo);
    }

    // Calcula o tempo total da partida
    public static float CalcularTempoTotal()
    {
        float total = 0f;

        foreach (float tempo in temposFases)
        {
            total += tempo;
        }

        return total;
    }

    // Inicia uma nova partida
    public static void ReiniciarPartida()
    {
        temposFases.Clear();
    }

    // Carrega os jogadores do ranking
    public static List<JogadorRanking> CarregarRanking()
    {
        string json = PlayerPrefs.GetString(CHAVE_RANKING, "");

        ListaJogadores lista = new ListaJogadores();

        if (!string.IsNullOrEmpty(json))
        {
            lista = JsonUtility.FromJson<ListaJogadores>(json);
        }

        if (lista == null)
        {
            lista = new ListaJogadores();
        }

        lista.jogadores.Sort(
            (a, b) => a.tempo.CompareTo(b.tempo)
        );

        return lista.jogadores;
    }

    // Registra um jogador no ranking
    public static void RegistrarJogador(string nome, float tempo)
    {
        ListaJogadores lista = new ListaJogadores();

        lista.jogadores = CarregarRanking();

        lista.jogadores.Add(new JogadorRanking(nome, tempo));

        lista.jogadores.Sort(
            (a, b) => a.tempo.CompareTo(b.tempo)
        );

        string json = JsonUtility.ToJson(lista);

        PlayerPrefs.SetString(CHAVE_RANKING, json);

        PlayerPrefs.Save();
    }
}
