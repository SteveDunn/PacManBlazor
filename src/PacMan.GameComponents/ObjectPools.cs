using System.Text;

namespace PacMan.GameComponents
{
    public static class ObjectPools
    {
        public static ObjectPool<StringBuilder> StringBuilders { get; } = new ObjectPool<StringBuilder>(()=>new StringBuilder());

    }
}