using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartschoolApi
{
    internal class JSONAccount
    {
        public string voornaam { get; set; }
        public string naam { get; set; }
        public string gebruikersnaam { get; set; }
        public string internnummer { get; set; }
        public string status { get; set; }
        public string extravoornamen { get; set; }
        public string initialen { get; set; }
        public string roepnaam { get; set; }
        public string geslacht { get; set; }
        public string geboortedatum { get; set; }
        public string geboorteplaats { get; set; }
        public string geboorteland { get; set; }
        public string rijksregisternummer { get; set; }
        public string straat { get; set; }
        public string huisnummer { get; set; }
        public string busnummer { get; set; }
        public string postcode { get; set; }
        public string woonplaats { get; set; }
        public string land { get; set; }
        public string emailadres { get; set; }
        public string website { get; set; }
        public string mobielnummer { get; set; }
        public string telefoonnummer { get; set; }
        public string fax { get; set; }
        public string instantmessenger { get; set; }
        public string sorteerveld { get; set; }
        public string stamboeknummer { get; set; }
        public string koppelingsveldschoolagenda { get; set; }
        public string basisrol { get; set; }
        public string klasnummer { get; set; }
        public bool isEmailVerified { get; set; }
        public bool isAuthenticatorAppEnabled { get; set; }
        public bool isYubikeyEnabled { get; set; }

        [JsonProperty(PropertyName = "Naam verantwoordelijke leerlingbegeleiding")]
        public string NaamVerantwoordelijkeLeerlingbegeleiding { get; set; }
        [JsonProperty(PropertyName = "Telefoonnummer verantwoordelijke leerlingbegeleiding")]
        public string TelefoonnummerVerantwoordelijkeLeerlingbegeleiding { get; set; }
        [JsonProperty(PropertyName = "Naam verantwoordelijke CLB")]
        public string NaamVerantwoordelijkeCLB { get; set; }
        [JsonProperty(PropertyName = "Telefoonnummer verantwoordelijke CLB")]
        public string TelefoonnummerVerantwoordelijkeCLB { get; set; }
        [JsonProperty(PropertyName = "Naam ouder/voogd")]
        public string NaamOuderVoogd { get; set; }
        [JsonProperty(PropertyName = "Voornaam ouder/voogd")]
        public string VoornaamOuderVoogd { get; set; }
        [JsonProperty(PropertyName = "Geslacht ouder/voogd")]
        public string GeslachtOuderVoogd { get; set; }
        [JsonProperty(PropertyName = "Adres ouder/voogd")]
        public string AdresOuderVoogd { get; set; }
        [JsonProperty(PropertyName = "Huisnummer ouder/voogd")]
        public string HuisnummerOuderVoogd { get; set; }
        [JsonProperty(PropertyName = "Postcode ouder/voogd")]
        public string PostcodeOuderVoogd { get; set; }
        public string Optie { get; set; }
        public string godsdienstkeuze { get; set; }
        public string nationaliteitscode { get; set; }
        public string nationaliteit { get; set; }
        public string soortleerling { get; set; }
        public string function { get; set; }
        public string teachercardnumber { get; set; }
        public string voornaam_coaccount1 { get; set; }
        public string naam_coaccount1 { get; set; }
        public string email_coaccount1 { get; set; }
        public string telefoonnummer_coaccount1 { get; set; }
        public string mobielnummer_coaccount1 { get; set; }
        public string type_coaccount1 { get; set; }
        public string voornaam_coaccount2 { get; set; }
        public string naam_coaccount2 { get; set; }
        public string email_coaccount2 { get; set; }
        public string telefoonnummer_coaccount2 { get; set; }
        public string mobielnummer_coaccount2 { get; set; }
        public string type_coaccount2 { get; set; }
        public string voornaam_coaccount3 { get; set; }
        public string naam_coaccount3 { get; set; }
        public string email_coaccount3 { get; set; }
        public string telefoonnummer_coaccount3 { get; set; }
        public string mobielnummer_coaccount3 { get; set; }
        public string type_coaccount3 { get; set; }
        public string voornaam_coaccount4 { get; set; }
        public string naam_coaccount4 { get; set; }
        public string email_coaccount4 { get; set; }
        public string telefoonnummer_coaccount4 { get; set; }
        public string mobielnummer_coaccount4 { get; set; }
        public string type_coaccount4 { get; set; }
        public string voornaam_coaccount5 { get; set; }
        public string naam_coaccount5 { get; set; }
        public string email_coaccount5 { get; set; }
        public string telefoonnummer_coaccount5 { get; set; }
        public string mobielnummer_coaccount5 { get; set; }
        public string type_coaccount5 { get; set; }
        public string voornaam_coaccount6 { get; set; }
        public string naam_coaccount6 { get; set; }
        public string email_coaccount6 { get; set; }
        public string telefoonnummer_coaccount6 { get; set; }
        public string mobielnummer_coaccount6 { get; set; }
        public string type_coaccount6 { get; set; }
        public string last_successful_login { get; set; }
        public string last_successful_login_coaccount1 { get; set; }
        public string last_successful_login_coaccount2 { get; set; }

        public List<JSONGroup> groups { get; set; }
    }
}
