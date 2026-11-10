// Este script só guarda os tempos do jogador.
// "static" quer dizer que os valores NÃO se perdem quando troca de cena.
public static class Tempos
{
    // Quanto tempo o jogador ficou em cada fase (em segundos)
    public static float tempoFase1;
    public static float tempoFase2;
    public static float tempoFase3;
    public static float tempoFase4;

    // O jogador passou da fase? 
    public static bool passouFase1;
    public static bool passouFase2;
    public static bool passouFase3;
    public static bool passouFase4;


    public static void Zerar()
    {
        tempoFase1 = 0;
        tempoFase2 = 0;
        tempoFase3 = 0;
        tempoFase4 = 0;

        passouFase1 = false;
        passouFase2 = false;
        passouFase3 = false;
        passouFase4 = false;
    }
}