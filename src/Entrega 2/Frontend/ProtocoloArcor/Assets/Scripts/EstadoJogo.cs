using UnityEngine;

public static class EstadoJogo
{
    const string chaveBala = "TemBala7Belo";

    // começa uma nova jornada sem a bala.
    public static void NovoJogo()
    {
        PlayerPrefs.SetInt(chaveBala, 0);
        PlayerPrefs.Save();
    }

    // quando o jogador concluir a fase bônus Central Bon o Bon.
    public static void GanharBala()
    {
        PlayerPrefs.SetInt(chaveBala, 1);
        PlayerPrefs.Save();
    }

    // consulta se o jogador tem a Bala 7 Belo.
    public static bool TemBala()
    {
        if (PlayerPrefs.GetInt(chaveBala, 0) == 1)
        {
            return true;
        }
        return false;
    }
}