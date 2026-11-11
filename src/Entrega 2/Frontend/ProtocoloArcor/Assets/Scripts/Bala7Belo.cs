using UnityEngine;

/// <summary>
/// Guarda se o jogador pegou a Bala 7 Belo na Fase Bônus (Central Bon o Bon).
/// Não vai em nenhum objeto: é só um "recado" que passa de uma fase pra outra.
/// Volta a ser false quando o jogo é aberto de novo.
/// </summary>
public static class Bala7Belo
{
    public static bool Coletada;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void Resetar()
    {
        Coletada = false;
    }
}
