using System;

public enum FighterMove
{
    None,
    LegKick,
    BodyKick,
    HeadKick,
    LegPunch,
    BodyPunch,
    HeadPunch
}

public enum FighterBlock
{
    None,
    LegBlock,
    BodyBlock,
    HeadBlock
}

public class FighterMoves
{
    public class Move
    {
        public FighterMove name;
        public FighterBlock block;
        public string trigger;
        public float damage;
        public float stamina;
        public float height;
    }

    public Move[] moves =
    {
        new Move {name = FighterMove.LegKick, block = FighterBlock.LegBlock, trigger = "LegDamage", damage = 20, stamina = 35, height = 0.25f },
        new Move {name = FighterMove.BodyKick, block = FighterBlock.BodyBlock, trigger = "BodyDamage", damage = 22, stamina = 35, height = 0.75f },
        new Move {name = FighterMove.HeadKick, block = FighterBlock.HeadBlock, trigger = "HeadDamage", damage = 24, stamina = 35, height = 1.25f },
        new Move {name = FighterMove.LegPunch, block = FighterBlock.BodyBlock, trigger = "BodyDamage", damage = 20, stamina = 10, height = 0.75f },
        new Move {name = FighterMove.BodyPunch, block = FighterBlock.HeadBlock, trigger = "HeadDamage", damage = 18, stamina = 10, height = 1.25f },
        new Move {name = FighterMove.HeadPunch, block = FighterBlock.HeadBlock, trigger = "HeadDamage", damage = 22, stamina = 15, height = 1.25f }
    };

    public Move Get(FighterMove move)
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
