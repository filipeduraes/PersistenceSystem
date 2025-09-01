using System.Collections.Generic;
using System.Threading.Tasks;
using IdeaToGame.PersistenceSystem.Strategies;
using Newtonsoft.Json;

namespace IdeaToGame.PersistenceSystem
{
    /// <summary>
    /// Provides a static persistence system to store, retrieve, save, and load game data 
    /// using configurable save and load strategies.
    /// </summary>
    public static class Persistence
    {
        /// <summary>
        /// Gets the index of the currently active save slot. 
        /// Returns -1 if no slot has been loaded.
        /// </summary>
        public static int CurrentSlotIndex { get; private set; } = -1;
        
        private static readonly IPersistenceStrategy DefaultPersistenceStrategy = new IOPersistenceStrategy("PersistenceSystem");
        private static ISaveStrategy saveStrategy = DefaultPersistenceStrategy;
        private static ILoadStrategy loadStrategy = DefaultPersistenceStrategy;
        
        private static Dictionary<string, object> loadedSlotData = new();

        /// <summary>
        /// Determines whether any slot data has been loaded.
        /// </summary>
        /// <returns>True if data is loaded, otherwise false.</returns>
        public static bool HasLoadedData()
        {
            return CurrentSlotIndex != -1;
        }

        /// <summary>
        /// Sets both the save and load strategy using a single persistence strategy implementation.
        /// </summary>
        /// <param name="newPersistenceStrategy">The persistence strategy implementing both save and load.</param>
        public static void SetupPersistenceStrategy(IPersistenceStrategy newPersistenceStrategy)
        {
            saveStrategy = newPersistenceStrategy;
            loadStrategy = newPersistenceStrategy;
        }
        
        /// <summary>
        /// Sets independent save and load strategies.
        /// </summary>
        /// <param name="newSaveStrategy">The save strategy implementation.</param>
        /// <param name="newLoadStrategy">The load strategy implementation.</param>
        public static void SetupPersistenceStrategy(ISaveStrategy newSaveStrategy, ILoadStrategy newLoadStrategy)
        {
            saveStrategy = newSaveStrategy;
            loadStrategy = newLoadStrategy;
        }

        /// <summary>
        /// Stores a data entry into the currently loaded slot data.
        /// If the key already exists, its value is replaced.
        /// </summary>
        /// <typeparam name="T">The type of data to store.</typeparam>
        /// <param name="key">The unique identifier for the data.</param>
        /// <param name="data">The data object to store.</param>
        public static void StoreData<T>(string key, T data)
        {
            loadedSlotData[key] = data;
        }
        
        /// <summary>
        /// Retrieves a data entry from the currently loaded slot data.
        /// </summary>
        /// <typeparam name="T">The expected type of the data.</typeparam>
        /// <param name="key">The unique identifier for the data.</param>
        /// <param name="defaultData">The value returned if the key does not exist.</param>
        /// <returns>The stored data if found; otherwise, the default value.</returns>
        public static T GetData<T>(string key, T defaultData = default)
        {
            if (loadedSlotData.TryGetValue(key, out object data))
            {
                return (T) data;
            }

            return defaultData;
        }

        /// <summary>
        /// Saves the currently loaded data to the active slot.
        /// </summary>
        public static async Task SaveAtCurrentSlotAsync()
        {
            await SaveAtSlotAsync(CurrentSlotIndex);
        }
        
        /// <summary>
        /// Saves the currently loaded data to the specified slot.
        /// </summary>
        /// <param name="slotIndex">The index of the save slot.</param>
        public static async Task SaveAtSlotAsync(int slotIndex)
        {
            CurrentSlotIndex = slotIndex;
            
            string jsonData = JsonConvert.SerializeObject(loadedSlotData);
            await saveStrategy.SaveAsync(slotIndex, jsonData);
        }

        /// <summary>
        /// Loads data from the specified slot and replaces the currently loaded data.
        /// </summary>
        /// <param name="slotIndex">The index of the save slot to load from.</param>
        public static async Task LoadFromSlotAsync(int slotIndex)
        {
            CurrentSlotIndex = slotIndex;
            
            string jsonData = await loadStrategy.LoadAsync(slotIndex);
            Dictionary<string,object> newData = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonData);
            newData ??= new Dictionary<string, object>();

            loadedSlotData = newData;
        }
    }
}