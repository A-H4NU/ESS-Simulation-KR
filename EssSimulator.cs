using System;
using System.Collections.Generic;
using System.Text;

namespace ESS_Simulation
{
    internal class EssSimulator
    {

        #region ESS Simulator Constants
        public const int SCORE_WIN = 50;
        public const int SCORE_WASTE_TIME = -10;
        public const int SCORE_INJURY = -100;

        public const double WINRATE_WIN = 1.8;
        public const double WINRATE_INJURY = 0.7;
        public const double WINRATE_MEET_PR = 1.8;
        #endregion

        private readonly int _count, _generation, _simulationC, _interactionC;

        public readonly DistibutionInfo<EntityType> Distibution;

        public EssSimulator(int count, int generation, int simulationC, int interactionC)
        {
            this._count = count;
            this._generation = generation;
            this._simulationC = simulationC;
            this._interactionC = interactionC;
            this.Distibution = new DistibutionInfo<EntityType>();
        }

        public void Run()
        {

            ToFile.WriteLine($"Simulation: {this._simulationC} | Generation: {this._generation} | Interaction : {this._interactionC} | Entities: {this._count}");
            ToFile.WriteLine();
            ToFile.WriteLine();
            ToFile.WriteLine();
            ToFile.WriteLine();
            for (int i = 0; i < this._simulationC; ++i)
            {
                ToFile.WriteLine($"////////////////////////////////////////////// SIMULATION {i + 1} //////////////////////////////////////////////");
                ToFile.WriteLine();
                this.Simulate();
                ToFile.WriteLine();
                ToFile.WriteLine();
            }

            ToFile.Save();
        }

        private void Simulate()
        {
            List<EntityCharacteristic> ecs = new List<EntityCharacteristic>(this._count);
            foreach (EntityType type in typeof(EntityType).GetEnumValues())
            {
                int amnt = this.Distibution.GetDistributeOf(type, this._count);
                for (int i = 0; i < amnt; ++i)
                    ecs.Add(EntityCharacteristic.GetDefault(type));
            }
            int amount = this._generation;
            for (int l = 0; l < amount; ++l)
            {
                ToFile.WriteLine($"================== Generation {l + 1} ==================");
                for (int k = 0; k < this._interactionC; ++k)
                {
                    ecs.Shuffle();
                    for (int i = 0; i < ecs.Count / 2; ++i)
                    {
                        Interact(ecs[2 * i], ecs[2 * i + 1]);
                    }
                }
                ToFile.WriteLine();
                PrintResult(this.GetScore(ecs));
                ToFile.WriteLine();
                ToFile.WriteLine();
            }
        }

        private List<SimulationResult> GetScore(List<EntityCharacteristic> ecs)
        {
            int[] scores = new int[typeof(EntityType).GetEnumNames().Length];
            int[] counts = new int[scores.Length];
            foreach (EntityCharacteristic ec in ecs)
            {
                scores[(int)ec.Type] += ec.Score;
                counts[(int)ec.Type]++;
            }
            int[] removed = Eliminate(ecs);
            List<SimulationResult> result = new List<SimulationResult>(scores.Length);
            for (int i = 0; i < scores.Length; ++i)
                result.Add(new SimulationResult((EntityType)i, (double)scores[i] / counts[i], counts[i], removed[i]));
            return result;
        }

        private static void Interact(EntityCharacteristic ec1, EntityCharacteristic ec2)
        {
            Battle battle = new Battle(ec1, ec2);

            ToFile.Write("(" + (battle.Luck ? "F" : "S") + ") ");
            string str2 = $"[ {battle.Winner} > {battle.Loser} ]";
            ToFile.Write(String.Format("{0,-100}", str2));
            battle.Winner.Score += SCORE_WIN;
            battle.Winner.WinRate *= WINRATE_WIN;
            bool injury = (battle.WBehavior == battle.LBehavior && (battle.WBehavior == EntityBehavior.Attack || battle.WBehavior == EntityBehavior.Incr_Intensity));
            bool pr_meet = (battle.Winner.Type == EntityType.Prober_Retaliator && battle.Winner.Type == EntityType.Prober_Retaliator);
            bool waste_time = (battle.WBehavior == EntityBehavior.Threaten && battle.LBehavior == EntityBehavior.Threaten);
            if (injury)
            {
                battle.Winner.Score += SCORE_INJURY;
                battle.Loser.Score += SCORE_INJURY;
                battle.Winner.WinRate *= WINRATE_INJURY;
                battle.Loser.WinRate *= WINRATE_INJURY;
            }
            if (pr_meet)
            {
                battle.Winner.WinRate *= WINRATE_MEET_PR;
                battle.Loser.WinRate *= WINRATE_MEET_PR;
            }
            if (waste_time)
            {
                battle.Winner.Score += SCORE_WASTE_TIME;
                battle.Loser.Score += SCORE_WASTE_TIME;
            }
            ToFile.WriteLine($" => [ {battle.Winner} | {battle.Loser} ]");
        }

        private static void PrintResult(ICollection<SimulationResult> result)
        {
            StringBuilder sb = new StringBuilder();
            foreach (SimulationResult r in result)
            {
                sb.Append("[ ");
                sb.Append("Type: ");
                sb.Append(String.Format("{0,-18}", r.Type));
                sb.Append(", Avg Score: ");
                if (r.AvgScore < 0) sb.Append("-");
                else if (r.AvgScore >= 0) sb.Append("+");
                else sb.Append(" ");
                sb.Append(String.Format("{0,7}", $"{Math.Abs(r.AvgScore):F2}"));
                sb.Append(", Count: ");
                sb.Append(String.Format("{0,4}", r.Count));
                sb.Append(", Removed: ");
                sb.Append(String.Format("{0,4}", r.Removed));
                sb.Append(" ]");
                sb.Append(Environment.NewLine);
            }
            ToFile.WriteLine(sb.ToString());
        }

        private static int[] Eliminate(List<EntityCharacteristic> ecs)
        {
            int[] removed = new int[typeof(EntityType).GetEnumValues().Length];
            ecs.Sort();
            for (int i = 0; i < ecs.Count / 2; ++i)
            {
                removed[(int)ecs[i + ecs.Count / 2].Type]++;
                ecs[i].Score = 0;
                ecs[i].WinRate = 0.5;
                ecs[i + ecs.Count / 2] = ecs[i].Clone();
            }
            return removed;
        }
    }
}
