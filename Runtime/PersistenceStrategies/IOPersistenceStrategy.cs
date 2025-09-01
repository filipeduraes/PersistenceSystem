using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace IdeaToGame.PersistenceSystem.Strategies
{
    public class IOPersistenceStrategy : IPersistenceStrategy
    {
        private readonly string _mainSaveDirectory;
        private readonly string _saveSubDirectory;
        private readonly string _saveFileName;
        private readonly string _saveFileExtension;

        public IOPersistenceStrategy(string saveSubDirectory, string saveFileName = "Save", string saveFileExtension = "ideatogame")
        {
            _mainSaveDirectory = TreatPathSeparator(Application.persistentDataPath);

            saveSubDirectory = TreatPathSeparator(saveSubDirectory);
            saveSubDirectory = TrimPathSeparators(saveSubDirectory);
            _saveSubDirectory = saveSubDirectory;
            
            _saveFileName = Path.GetFileNameWithoutExtension(saveFileName);
            _saveFileExtension = saveFileExtension.Replace(".", string.Empty);
        }

        public async Task SaveAsync(int slotIndex, string data)
        {
            string path = EnsureAndGetSlotFilePath(slotIndex);

            await using StreamWriter file = new(path);
            await file.WriteAsync(data);
        }

        public async Task<string> LoadAsync(int slotIndex)
        {
            string path = EnsureAndGetSlotFilePath(slotIndex);
            
            using StreamReader file = new(path);
            return await file.ReadToEndAsync();
        }
        
        private string EnsureAndGetSlotFilePath(int slotIndex)
        {
            string saveDirectoryPath = GetSaveDirectoryPath();
            
            if (!Directory.Exists(saveDirectoryPath))
            {
                Directory.CreateDirectory(saveDirectoryPath);
            }

            string path = Path.Combine(saveDirectoryPath, $"{_saveFileName}_Slot{slotIndex:00}.{_saveFileExtension}");

            if (!File.Exists(path))
            {
                File.Create(path);
            }
            
            return path;
        }

        private string GetSaveDirectoryPath()
        {
            return Path.Combine(_mainSaveDirectory, _saveSubDirectory);
        }
        
        private static string TrimPathSeparators(string path)
        {
            if (path.StartsWith(Path.PathSeparator))
            {
                path = path.Remove(0, 1);
            }

            if (path.EndsWith(Path.PathSeparator))
            {
                path = path.Remove(path.Length - 1, 1);
            }

            return path;
        }

        private static string TreatPathSeparator(string path)
        {
            return path.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);
        }
    }
}