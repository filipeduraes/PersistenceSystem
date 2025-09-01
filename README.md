# PersistenceSystem

A flexible persistence system for saving and loading game data in **C#**, built with the Unity Package Manager (UPM).  
This package provides a slot-based persistence API with customizable strategies for saving and loading data.

Repository: [PersistenceSystem](https://github.com/filipeduraes/PersistenceSystem)

---

## Features

- Slot-based persistence system.
- Store and retrieve arbitrary key-value data.
- Async save and load operations.
- Strategy pattern with `ISaveStrategy`, `ILoadStrategy`, and `IPersistenceStrategy`.
- JSON serialization with [Newtonsoft.Json](https://www.newtonsoft.com/json).

---

## Installation

### Using UPM (Unity Package Manager)

1. In Unity, open **Window > Package Manager**.
2. Click **+** > **Add package from git URL...**.
3. Enter: https://github.com/filipeduraes/PersistenceSystem.git
4. Unity will fetch and install the package directly.

### Using `manifest.json`

Alternatively, add this entry to your `Packages/manifest.json`:

```json
{
  "dependencies": {
   "com.filipeduraes.persistencesystem": "https://github.com/filipeduraes/PersistenceSystem.git"
  }
}
```

## Usage

### Setup a Persistence Strategy

```csharp
// Using a combined strategy
Persistence.SetupPersistenceStrategy(new IOPersistenceStrategy("PersistenceSystem"));

// Or independent save/load strategies
Persistence.SetupPersistenceStrategy(new CustomSaveStrategy(), new CustomLoadStrategy());
```

### Store and Retrieve Data

```csharp
Persistence.StoreData("PlayerName", "Alice");
Persistence.StoreData("PlayerLevel", 5);

string playerName = Persistence.GetData("PlayerName", "Unknown");
int playerLevel = Persistence.GetData("PlayerLevel", 1);
```

### Save Data

```csharp
await Persistence.SaveAtSlotAsync(0); // Save to slot 0
await Persistence.SaveAtCurrentSlotAsync(); // Save to the current slot
```

### Load Data

```csharp
await Persistence.LoadFromSlotAsync(0); // Load from slot 0
int level = Persistence.GetData("PlayerLevel", 1);
```

---

## Interfaces

- **ISaveStrategy**: Contract for saving data
- **ILoadStrategy**: Contract for loading data
- **IPersistenceStrategy**: Combines both saving and loading

This enables multiple storage backends (files, PlayerPrefs, cloud, databases).

---

## Example: File-based Strategy

```csharp
public class IOPersistenceStrategy : IPersistenceStrategy
{
    private readonly string folder;

    public IOPersistenceStrategy(string folder)
    {
        this.folder = folder;
        Directory.CreateDirectory(folder);
    }

    public async Task SaveAsync(int slotIndex, string data)
    {
        string path = Path.Combine(folder, $"slot_{slotIndex}.json");
        await File.WriteAllTextAsync(path, data);
    }

    public async Task<string> LoadAsync(int slotIndex)
    {
        string path = Path.Combine(folder, $"slot_{slotIndex}.json");
        return File.Exists(path) ? await File.ReadAllTextAsync(path) : "{}";
    }
}
```

---

## License

MIT License\
See [LICENSE](LICENSE) for details.
