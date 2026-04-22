using System;
using _Game.Scripts.Infrastructure.Services;

namespace _Game.Scripts.Infrastructure.Services.Score
{
    public class ScoreService : IService
    {
        public int MergedCount { get; private set; }

        public event Action<int> OnMergeCountChanged;

        public void AddMerge()
        {
            MergedCount++;
            OnMergeCountChanged?.Invoke(MergedCount);
        }

        public void AddScore(int amount)
        {
            MergedCount += amount;
            OnMergeCountChanged?.Invoke(MergedCount);
        }

        public void Reset()
        {
            MergedCount = 0;
            OnMergeCountChanged?.Invoke(MergedCount);
        }
    }
}
