using System.Text.Json;
using BizHawk.Client.Common;
using PokemonViewer.Memory.Global;
using PokemonViewer.PokemonData;

namespace DefaultNamespace
{
    public class GameData
    {
        public Pokemon[] Team { get; private set; }

        // private Pokemon[] boxes;
        public GameData(IMemoryApi memoryApi)
        {
            Team = new Pokemon[6];
            for (int i = 0; i < 6; i++)
                Team[i] = new Pokemon(memoryApi.ReadByteRange(GlobalAddress.EmeraldUsParty + i * 100, 100));
        }

        public string Serialize()
        {
            var options = new JsonSerializerOptions();
            options.WriteIndented = true;
            return JsonSerializer.Serialize(this,options);
        }
    }
}