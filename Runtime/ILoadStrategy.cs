using System.Threading.Tasks;

namespace IdeaToGame.PersistenceSystem
{
    /// <summary>
    /// Defines the contract for loading persistence data from a specific slot.
    /// </summary>
    public interface ILoadStrategy
    {
        /// <summary>
        /// Loads serialized data asynchronously from the specified slot.
        /// </summary>
        /// <param name="slotIndex">The index of the save slot.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the serialized data as a string.</returns>
        Task<string> LoadAsync(int slotIndex);
    }
}