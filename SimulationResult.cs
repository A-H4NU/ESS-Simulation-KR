namespace ESS_Simulation
{
    internal struct SimulationResult
    {
        public readonly EntityType Type;
        public readonly double AvgScore;
        public readonly int Count;
        public readonly int Removed;

        public SimulationResult(EntityType type, double avgScore, int count, int removed)
        {
            this.Type = type;
            this.AvgScore = avgScore;
            this.Count = count;
            this.Removed = removed;
        }
    }
}
