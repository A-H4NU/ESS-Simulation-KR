using System;

namespace ESS_Simulation
{
    internal class EntityCharacteristic : IComparable<EntityCharacteristic>
    {
        public readonly EntityType Type;

        // 둘 다 동일한 행동을 할 경우 승률
        public double WinRate;

        // [행동 점수] (+): 싸움에서 승리 | (-): 패배, 시간 낭비, 부상
        public int Score;

        private EntityCharacteristic(EntityType type)
        {
            this.Type = type;
            this.WinRate = 0.5;
            this.Score = 0;
        }

        public static EntityCharacteristic GetDefault(EntityType type) => new EntityCharacteristic(type);

        public override string ToString() => $"( T: {this.Type}, WR: {this.WinRate:F2}, SC: {this.Score} )";

        int IComparable<EntityCharacteristic>.CompareTo(EntityCharacteristic other) => other.Score.CompareTo(this.Score);

        public EntityCharacteristic Clone() => new EntityCharacteristic(this.Type);
    }
}
