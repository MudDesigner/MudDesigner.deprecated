using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudDesigner.Engine.Game
{
    public interface IStatFactory
    {
        IStat CreateStat(string name, string abbreviation, int baseScore);

        IStat CreateStat(string name, string abbreviation, int baseScore, int scoreModifier);
    }
}
