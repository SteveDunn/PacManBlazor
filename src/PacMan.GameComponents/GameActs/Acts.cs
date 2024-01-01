﻿namespace PacMan.GameComponents.GameActs;

public class Acts : IActs
{
    private readonly Dictionary<string, IAct> _acts;

    public Acts(IEnumerable<IAct> acts)
    {
        _acts = acts.ToDictionary(k => k.Name, v => v);
    }

    public IAct GetActNamed(string name)
    {
        IAct act = _acts[name];

        act.Reset();

        return act;
    }
}