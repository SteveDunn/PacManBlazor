using System;
using PacMan.GameComponents.Ghosts;
// ReSharper disable HeapView.ObjectAllocation.Evident

namespace PacMan.GameComponents
{
    // perfect game at https://www.bing.com/videos/search?q=Perfect+Pac+Man&&view=detail&mid=AE586ACE933BE975ADD9AE586ACE933BE975ADD9&FORM=VRDGAR
    // cut scene 1 at 2:47
    // cut scene 2 at 6:50
    // cut scene 3 at 12:16 (snail thing)
    // max level 21

    public class LevelStats
    {
        const int _startingAmountOfPills = 244;

        const int _maxlevel = 20;

        readonly char[] _currentMap;

        static readonly LevelProps[] _levelProps =
        {
            //                                  pn   pd  gn  gt                      pf  pfd gf
            new LevelProps(IntroCutScene.None, FruitItem.Cherry, 300, 80, 71, 80, 40, 30, 90, 15, 95, 90, 79, 50, 6, 5),
            new LevelProps(IntroCutScene.None, FruitItem.Strawberry, 300, 90, 79, 85, 45, 30, 90, 15, 95, 95, 83, 55, 5,
                5),
            new LevelProps(IntroCutScene.BigPac, FruitItem.Peach, 500, 90, 79, 85, 45, 40, 90, 20, 95, 95, 83, 55, 4,
                5),
            new LevelProps(IntroCutScene.None, FruitItem.Peach, 500, 90, 79, 85, 45, 40, 90, 20, 95, 95, 83, 55, 3, 5),
            new LevelProps(IntroCutScene.None, FruitItem.Apple, 700, 100, 87, 95, 50, 40, 100, 20, 105, 100, 87, 60, 2,
                5),
            new LevelProps(IntroCutScene.GhostSnagged, FruitItem.Apple, 700, 100, 87, 95, 50, 50, 100, 25, 105, 100, 87,
                60, 2, 5),
            new LevelProps(IntroCutScene.None, FruitItem.Grape, 1000, 100, 87, 95, 50, 50, 100, 25, 105, 100, 87, 60, 2,
                5),
            new LevelProps(IntroCutScene.None, FruitItem.Grape, 1000, 100, 87, 95, 50, 50, 100, 25, 105, 100, 87, 60, 1,
                5),
            new LevelProps(IntroCutScene.None, FruitItem.Galaxian, 2000, 100, 87, 95, 50, 60, 100, 30, 105, 100, 87, 60,
                5, 3),
            new LevelProps(IntroCutScene.TornGhostAndWorm, FruitItem.Galaxian, 2000, 100, 87, 95, 50, 60, 100, 30, 105,
                100, 87, 60, 2, 5),
            new LevelProps(IntroCutScene.None, FruitItem.Bell, 3000, 100, 87, 95, 50, 60, 100, 30, 105, 100, 87, 60, 1,
                5),
            new LevelProps(IntroCutScene.None, FruitItem.Bell, 3000, 100, 87, 95, 50, 80, 100, 40, 105, 100, 87, 60, 1,
                3),
            new LevelProps(IntroCutScene.None, FruitItem.Key, 5000, 100, 87, 95, 50, 80, 100, 40, 105, 100, 87, 60, 1,
                3),
            new LevelProps(IntroCutScene.TornGhostAndWorm, FruitItem.Key, 5000, 100, 87, 95, 50, 80, 100, 40, 105, 100,
                87, 60, 3, 5),
            new LevelProps(IntroCutScene.None, FruitItem.Key, 5000, 100, 87, 95, 50, 100, 100, 50, 105, 100, 87, 60, 1,
                3),
            new LevelProps(IntroCutScene.None, FruitItem.Key, 5000, 100, 87, 95, 50, 100, 100, 50, 105, 100, 87, 60, 1,
                3),
            new LevelProps(IntroCutScene.None, FruitItem.Key, 5000, 100, 87, 95, 50, 100, 100, 50, 105, 0, 0, 0, 0, 0),
            new LevelProps(IntroCutScene.TornGhostAndWorm, FruitItem.Key, 5000, 100, 87, 95, 50, 100, 100, 50, 105, 100,
                87, 60, 1, 3),
            new LevelProps(IntroCutScene.None, FruitItem.Key, 5000, 100, 87, 95, 50, 120, 100, 60, 105, 0, 0, 0, 0, 0),
            new LevelProps(IntroCutScene.None, FruitItem.Key, 5000, 100, 87, 95, 50, 120, 100, 60, 105, 0, 0, 0, 0, 0),
            new LevelProps(IntroCutScene.None, FruitItem.Key, 5000, 90, 79, 95, 50, 120, 100, 60, 105, 0, 0, 0, 0, 0)
        };

        //todo: move to another class (and related properties)
        static readonly char[] _map = (
            // 0,0                     29,0
            "                             " +
            " oooooooooooo  oooooooooooo  " +
            " o    o     o  o     o    o  " +
            " *    o     o  o     o    *  " +
            " o    o     o  o     o    o  " +
            " oooooooooooooooooooooooooo  " +
            " o    o  o        o  o    o  " +
            " o    o  o        o  o    o  " +
            " oooooo  oooo  oooo  oooooo  " +
            "      o     +  +     o       " +
            "      o     +  +     o       " +
            "      o  ++++++++++  o       " +
            "      o  +        +  o       " +
            "      o  +        +  o       " +
            "++++++o+++        +++o+++++++" +
            "      o  +        +  o       " +
            "      o  +        +  o       " +
            "      o  ++++++++++  o       " +
            "      o  +        +  o       " +
            "      o  +        +  o       " +
            " oooooooooooo  oooooooooooo  " +
            " o    o     o  o     o    o  " +
            " o    o     o  o     o    o  " +
            " *oo  ooooooo++ooooooo  oo*  " +
            "   o  o  o        o  o  o    " +
            "   o  o  o        o  o  o    " +
            " oooooo  oooo  oooo  oooooo  " +
            " o          o  o          o  " +
            " o          o  o          o  " +
            " oooooooooooooooooooooooooo  " +
            "                             ").ToCharArray();
        // 0,30                   //27,29


        public LevelStats(int levelNumber)
        {
            LevelNumber = levelNumber;

            PillsRemaining = _startingAmountOfPills;
            _currentMap = (char[])_map.Clone();

            FruitSession = new FruitSession();
        }

        public FruitSession FruitSession { get; }

        public int PillsRemaining { get; private set; }

        public int PillsEaten => _startingAmountOfPills - PillsRemaining;

        public int LevelNumber { get; }

        public LevelProps GetLevelProps()
        {
            var index = Math.Min(LevelNumber, _maxlevel);
            return _levelProps[index];
        }

        public static LevelProps GetLevelProps(int level) => _levelProps[level];

        public GhostsLevelPatternProperties GetGhostPatternProperties()
        {
            //debug:
            // if (this.levelNumber === 0) {
            //     p.Scatter1 = 111117;
            //     p.Chase1 = 20;
            //     p.Scatter2 = 7;
            //     p.Chase2 = 20;
            //     p.Scatter3 = 5;
            //     p.Chase3 = 20;
            //     p.Scatter4 = 5;
            //     p.Chase4 = Number.MAX_VALUE;

            //     return p;
            // }

            if (LevelNumber == 0)
            {
                return new GhostsLevelPatternProperties
                {
                    Scatter1 = 7,
                    Chase1 = 20,
                    Scatter2 = 7,
                    Chase2 = 20,
                    Scatter3 = 5,
                    Chase3 = 20,
                    Scatter4 = 5,
                    Chase4 = int.MaxValue
                };
            }

            if (LevelNumber >= 1 && LevelNumber <= 3)
            {
                return new GhostsLevelPatternProperties
                {
                    Scatter1 = 7,
                    Chase1 = 20,
                    Scatter2 = 7,
                    Chase2 = 20,
                    Scatter3 = 5,
                    Chase3 = 1033,
                    Scatter4 = 0,
                    Chase4 = int.MaxValue
                };
            }

            return new GhostsLevelPatternProperties
            {
                Scatter1 = 5,
                Chase1 = 20,
                Scatter2 = 7,
                Chase2 = 20,
                Scatter3 = 5,
                Chase3 = 1037,
                Scatter4 = 0,
                Chase4 = int.MaxValue
            };
        }

        public void PillEaten(CellIndex cellPosition)
        {
            FruitSession.PillEaten();

            --PillsRemaining;

            int index = cellPosition.Y * 29 + cellPosition.X;

            _currentMap[index] = '+';
        }

        static int getArrayIndex(CellIndex point) => point.Y * 29 + point.X;

        public char GetCellContent(CellIndex point) => _currentMap[getArrayIndex(point)];
    }
}