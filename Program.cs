using System;

namespace ESS_Simulation
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (!GetIntegerInput("시뮬레이션 실행 횟수를 입력하세요: ", out int executeC, 1)) return;
            if (!GetIntegerInput("시뮬레이션 당 세대 수를 입력하세요: ", out int generation, 1)) return;
            if (!GetIntegerInput("세대 당 상호작용 수를 입력하세요: ", out int interactionC, 1)) return;
            if (!GetIntegerInput("시뮬레이션 당 개체 수를 입력하세요: ", out int entityC, 1)) return;
            EssSimulator es = new EssSimulator(entityC, generation, executeC, interactionC);
            foreach (EntityType type in typeof(EntityType).GetEnumValues())
            {
                if (!GetIntegerInput($"시뮬레이션 종 {type}의 개수(비)를 입력하세요", out int d, 0)) return;
                es.Distibution.Add(type, d);
            }
            es.Run();
            Console.ReadKey();
        }

        private static bool GetIntegerInput(string message, out int input, int min = Int32.MinValue, int max = Int32.MaxValue)
        {
            Console.WriteLine(message);
            Console.Write(" >> ");
            if (!Int32.TryParse(Console.ReadLine(), out int i))
            {
                Console.WriteLine("Invalid Input");
                input = 0;
                return false;
            }
            if (i < min || i > max)
            {
                Console.WriteLine("Invalid Input");
                input = 0;
                return false;
            }
            input = i;
            return true;
        }
    }
}
