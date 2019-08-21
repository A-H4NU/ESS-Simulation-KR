using System;

namespace ESS_Simulation
{
    internal class Battle
    {
        private readonly Random _random = new Random();

        public EntityCharacteristic Winner, Loser;
        public EntityBehavior? WBehavior, LBehavior;

        public bool Luck;

        public Battle(EntityCharacteristic ec1, EntityCharacteristic ec2)
        {
            EntityBehavior eb1 = Interaction.Get(ec1.Type, ec2.Type);
            EntityBehavior eb2 = Interaction.Get(ec2.Type, ec1.Type);
            this.Luck = false;
            if (eb1 == eb2)
            {
                bool firstwon = this._random.NextDouble() < (ec1.WinRate / (ec1.WinRate + ec2.WinRate));
                this.Winner = firstwon ? ec1 : ec2;
                this.Loser = firstwon ? ec2 : ec1;
                this.Luck = true;
            }
            else if (eb1 == EntityBehavior.Run || eb2 == EntityBehavior.Run)
            {
                this.Winner = (eb1 == EntityBehavior.Run) ? ec2 : ec1;
                this.Loser = (eb1 == EntityBehavior.Run) ? ec1 : ec2;
            }
            else if (eb1 != eb2)
            {
                this.Winner = (eb1 == EntityBehavior.Attack) ? ec1 : ec2;
                this.Loser = (eb1 == EntityBehavior.Attack) ? ec2 : ec1;
            }
            if (this.Winner != null) this.WBehavior = Interaction.Get(this.Winner.Type, this.Loser.Type); else this.WBehavior = null;
            if (this.Loser != null) this.LBehavior = Interaction.Get(this.Loser.Type, this.Winner.Type); else this.LBehavior = null;
        }
    }
}
