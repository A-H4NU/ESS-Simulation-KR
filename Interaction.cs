using System;

namespace ESS_Simulation
{
    internal static class Interaction
    {
        public static EntityBehavior Get(EntityType who, EntityType to)
        {
            switch (who)
            {
                case EntityType.Pigeon:
                    return EntityBehavior.Threaten;
                case EntityType.Hawk:
                    return EntityBehavior.Attack;
                case EntityType.Bully:
                    if (to == EntityType.Pigeon)
                        return EntityBehavior.Attack;
                    else
                        return EntityBehavior.Run;
                case EntityType.Retaliator:
                    if (to == EntityType.Bully || to == EntityType.Hawk)
                        return EntityBehavior.Attack;
                    else
                        return EntityBehavior.Threaten;
                case EntityType.Prober_Retaliator:
                    if (to == EntityType.Prober_Retaliator)
                        return EntityBehavior.Incr_Intensity;
                    else if (to == EntityType.Bully || to == EntityType.Hawk)
                        return EntityBehavior.Attack;
                    else
                        return EntityBehavior.Threaten;
            }
            throw new Exception();
        }
    }
}
