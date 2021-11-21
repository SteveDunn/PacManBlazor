namespace PacMan.GameComponents;

public static class Pnrg
{
    static int _pnrg;

    public static void ResetPnrg()
    {
        _pnrg = 0;
    }

    public static int Value => _pnrg;

    public static void Update()
    {
        ++_pnrg;
    }
}