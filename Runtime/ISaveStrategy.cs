using System.Threading.Tasks;

namespace IdeaToGame.PersistenceSystem
{
    /// <summary>
    /// Defines the contract for saving persistence data to a specific slot.
    /// </summary>
    public interface ISaveStrategy
    {
        /// <summary>
        /// Saves serialized data asynchronously into the specified slot.
        /// </summary>
        /// <param name="slotIndex">The index of the save slot.</param>
        /// <param name="data">The serialized data to save.</param>
        Task SaveAsync(int slotIndex, string data);
    }
}