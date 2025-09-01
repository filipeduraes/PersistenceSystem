namespace IdeaToGame.PersistenceSystem
{
    /// <summary>
    /// Defines a persistence strategy that combines both saving and loading functionality.
    /// </summary>
    public interface IPersistenceStrategy : ISaveStrategy, ILoadStrategy { }
}