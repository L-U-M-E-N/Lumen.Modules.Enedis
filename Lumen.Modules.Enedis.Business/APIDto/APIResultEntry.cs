using Lumen.Modules.Enedis.Business.Helpers;

using System.Text.Json.Serialization;

namespace Lumen.Modules.Enedis.Business.APIDto {
    public class ConsumptionEntry {
        public DateTime dateDebut { get; set; }
        public DateTime dateFin { get; set; }
        [JsonConverter(typeof(StringToFloatConverter))]
        public float valeur { get; set; }
    }

    public class ConsumptionAggregate {
        public IList<ConsumptionEntry> donnees { get; set; }
        public string unite { get; set; }
    }

    public class APIResultCons {
        public Dictionary<string, ConsumptionAggregate> aggregats { get; set; }
        public string grandeurMetier { get; set; }
        public string grandeurPhysique { get; set; }
        public DateOnly dateDebut { get; set; }
        public DateOnly dateFin { get; set; }
    }

    public class APIResult {
        public APIResultCons cons { get; set; }
    }
}
