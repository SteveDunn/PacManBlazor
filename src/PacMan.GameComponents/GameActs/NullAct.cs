// ReSharper disable RedundantUsingDirective

using System.Threading.Tasks;
using PacMan.GameComponents.Canvas;

namespace PacMan.GameComponents.GameActs
{
    // ReSharper disable once FunctionRecursiveOnAllPaths
    // ReSharper disable once UnusedType.Global
    public class NullAct : IAct
    {

        public string Name => "NullAct";

        public ValueTask Reset()
        {
            return default;
        }

        public ValueTask<ActUpdateResult> Update(CanvasTimingInformation timing) => 
            new ValueTask<ActUpdateResult>(ActUpdateResult.Running);

        public ValueTask Draw(CanvasWrapper session) => default;
    }
}