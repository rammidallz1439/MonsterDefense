

public class GameConstants
{
    #region Variables
    public const int SmallDropValue = 2;
    public const int MediumDropValue = 4;
    public const int LargeDropValue = 10;
    public const int ExtraLargeDropValue = 15;
    #endregion

    #region Data
   
    #endregion

    #region Animations

    //Enemies
    public const string EnemyIdle = "EnemyIdle";
    public const string EnemyRun = "EnemyRun";
    public const string EnemyAttack = "EnemyAttack";
    public const string EnemyDeath = "EnemyDeath";
    public const string EnemyWalk = "EnemyWalk";


    //Character Animations
    public const string CharacterIdle = "CharacterIdle";
    public const string CharacterAttack = "CharacterAttack";

    //Hire 
    public const string HireEndtime = "HireEndTime";
    public const string SaveHireData = "SaveHireData";

    #endregion

    #region Firebase

    public const string RootCollection = "users";
    public const string UserDocument = "UserLoginData";

    public const string LoggedState = "UserLoggedAs";

    public const string LevelDictCollection = "LevelDataCollection";
    public const string LevelDictDocument = "LevelData";


    public const string PlayerData = "PlayerData";
    public const string PlayerDataDoc = "PlayerSavedState";

    public const string LevelData = "LevelData";
    public const string DefinedLevelData = "Data";

    public const string SkillsData = "SkillData";
    public const string SkillsDataDoc = "TotalSkillsBrought";
    #endregion
}
