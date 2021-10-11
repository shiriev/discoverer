using System;
using System.Collections.Immutable;

namespace Discoverer.Logic.GameContract
{
    public record MarkerSet(int? ImproperCellForPlayer, ImmutableHashSet<int> PossibleCellForPlayers)
    {
        public virtual bool Equals(MarkerSet? other)
        {
            if (other is null)
                return false;
            var (improperCellForPlayer, possibleCellForPlayers) = other!;

            return ImproperCellForPlayer == improperCellForPlayer && PossibleCellForPlayers.SetEquals(possibleCellForPlayers);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ImproperCellForPlayer, PossibleCellForPlayers);
        }
    }
}