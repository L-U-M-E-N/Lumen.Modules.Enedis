using Lumen.Modules.Enedis.Business.APIDto;
using Lumen.Modules.Enedis.Business.Interfaces;
using Lumen.Modules.Enedis.Common.Models;
using Lumen.Modules.Enedis.Data;

using System.Text.Json;

namespace Lumen.Modules.Enedis.Business.Implementations {
    public class EnedisApi(EnedisContext context, IHttpClientFactory httpClientFactory) : IEnedisApi {
        public async Task<APIResult> GetDataFromAPI(string cookie, string person, string prm) {
            var client = httpClientFactory.CreateClient();
            var request = new HttpRequestMessage {
                Method = HttpMethod.Get,
                RequestUri = new Uri(
                    $"https://alex.microapplications.enedis.fr/mes-mesures-prm/api/private/v2/personnes/{person}/prms/{prm}/donnees-energetiques?mesuresTypeCode=ENERGIE&mesuresCorrigees=false&typeDonnees=CONS&segments=C5"),
                Headers =
                {
                    { "Accept", "application/json" },
                    { "Cookie", cookie },
                },
            };
            using var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<APIResult>(body)!;
        }

        public async Task QueryConsumptionData(string cookie, string person, string prm) {
            var data = await GetDataFromAPI(cookie, person, prm);

            context.Enedis.AddRange(data.cons.aggregats["jour"].donnees.Where(d => !context.Enedis.Any(e => e.DateDebut == d.dateDebut.ToUniversalTime())).Select((r) => {
                return new EnedisPointInTime {
                    DateDebut = r.dateDebut.ToUniversalTime(),
                    DateFin = r.dateFin.ToUniversalTime(),
                    Consumption = r.valeur,
                };
            }));

            await context.SaveChangesAsync();
        }
    }
}
