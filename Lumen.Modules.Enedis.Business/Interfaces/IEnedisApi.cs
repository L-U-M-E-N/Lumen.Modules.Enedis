namespace Lumen.Modules.Enedis.Business.Interfaces {
    public interface IEnedisApi {
        Task QueryConsumptionData(string cookie, string person, string prm);
    }
}
