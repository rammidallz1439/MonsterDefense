
using Vault;

public interface IEnemy
{
    public void Excute();
}

#region Enemies

public class MinionEnemy : IEnemy
{
    private Enemy Enemy;

    public MinionEnemy(Enemy enemy)
    {
        Enemy = enemy;
    }
    public void Excute()
    {
       
    }
}


#endregion