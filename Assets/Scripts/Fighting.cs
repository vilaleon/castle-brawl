using System;

namespace Fighting
{
    public enum Move
    {
        None,
        LegKick,
        BodyKick,
        HeadKick,
        LegPunch,
        BodyPunch,
        HeadPunch
    }

    public enum Block
    {
        None,
        LegBlock,
        BodyBlock,
        HeadBlock
    }

    public class Stats
    {
        public class Move
        {
            public Fighting.Move name;
            public Block block;
            public string trigger;
            public float damage;
            public float stamina;
            public float height;
        }

        public Move[] moves =
        {
        new Move { name = Fighting.Move.LegKick, block = Block.LegBlock, trigger = "LegDamage", damage = 20, stamina = 35, height = .25f },
        new Move { name = Fighting.Move.BodyKick, block = Block.BodyBlock, trigger = "BodyDamage", damage = 22, stamina = 35, height = .75f },
        new Move { name = Fighting.Move.HeadKick, block = Block.HeadBlock, trigger = "HeadDamage", damage = 24, stamina = 35, height = 1.25f },
        new Move { name = Fighting.Move.LegPunch, block = Block.LegBlock, trigger = "BodyDamage", damage = 20, stamina = 10, height = .75f },
        new Move { name = Fighting.Move.BodyPunch, block = Block.BodyBlock, trigger = "HeadDamage", damage = 18, stamina = 10, height = 1.25f },
        new Move { name = Fighting.Move.HeadPunch, block = Block.HeadBlock, trigger = "HeadDamage", damage = 22, stamina = 15, height = 1.25f }
        };

        public Move Get(Fighting.Move move)
        {
            return Array.Find(moves, m => m.name == move);
        }

        public void AdjustDamage(float strength)
        {
            foreach (var move in moves)
            {
                move.damage *= strength;
            }
        }
    }
}