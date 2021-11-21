using System;

namespace PacMan.GameComponents;

public class DemoKeyPresses
{
    // 01234567890123456789012345678901234567890123456789012345678901234567890123456789
    const string _presses = "ldrdrruluruluuulllllddlllddldlul";

    int _index;

    public DemoKeyPresses() => _index = 0;

    public void Reset() => _index = 0;

    public Direction Next()
    {
        if (_index >= _presses.Length)
        {
            // ReSharper disable once HeapView.ObjectAllocation.Evident
            throw new InvalidOperationException("Used up all key presses!");
        }

        return _presses[_index++] switch
        {
            'u' => Direction.Up,
            'd' => Direction.Down,
            'l' => Direction.Left,
            'r' => Direction.Right,
            // ReSharper disable once HeapView.BoxingAllocation
            _ => throw new InvalidOperationException($"Don't know what direction ${_presses[_index]} is!")
        };
    }
}