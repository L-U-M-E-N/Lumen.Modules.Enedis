// ==UserScript==
// @name         Enedis LUMEN
// @namespace    http://elanis.eu/
// @version      2025-08-07
// @description  Sends the current session cookie to the Lumen API's Enedis Module, so it can query consumption data and save it
// @author       Axel "Elanis" Soupé
// @match        https://monespace.grdf.fr/client/particulier/consommation
// @icon         icon
// @grant        GM.cookie
// @grant        GM_xmlhttpRequest
// @homepageURL  https://github.com/L-U-M-E-N/Lumen.Modules.Enedis
// @connect      lumen.domain.tld
// ==/UserScript==

////////////////
// CONFIG
////////////////
const LUMEN_SERVER_URL = 'https://lumen.domain.tld/';
const LUMEN_SERVER_API_KEY = 'FILLME';

//// Constants
const LUMEN_SERVER_AUTH_HEADER_NAME = 'X-Api-Key';
const LUMEN_SERVER_ROUTE = 'EnedisData/queryDataFromEnedis';
const Enedis_URL = 'https://monespace.grdf.fr/client/particulier/consommation';

//// Execution
const cs = await GM.cookie.list({ url: Enedis_URL, partitionKey: {} });
const formattedCookie = cs.map((c) => c.name + '=' + c.value).join('; ');
GM.xmlHttpRequest({
  url: LUMEN_SERVER_URL + LUMEN_SERVER_ROUTE,
  method: "POST",
  data: JSON.stringify({ cookie: formattedCookie }),
  headers: {
    "Content-Type": "application/json",
    [LUMEN_SERVER_AUTH_HEADER_NAME]: LUMEN_SERVER_API_KEY
  },
}).then((res) => {
  console.log(res);
  if (res.status === 202) {
    alert('Enedis cookie has been submitted to LUMEN server and data has been updated.');
  }
}).catch(e => console.error(e));