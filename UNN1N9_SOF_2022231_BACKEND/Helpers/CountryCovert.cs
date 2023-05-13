using SpotifyWebApi.Api.Search;
using static SpotifyAPI.Web.PlayerSetRepeatRequest;

namespace UNN1N9_SOF_2022231_BACKEND.Helpers
{
    public static class CountryCovert
    {
        public static string CountryConvert(string code)
        {
            if (code.StartsWith('A'))
            {
                return SearchForA(code);
            }
            else if(code.StartsWith('B'))
            {
                return SearchForB(code);
            }
            else if (code.StartsWith('C'))
            {
                return SearchForC(code);
            }
            else if (code.StartsWith('D'))
            {
                return SearchForD(code);
            }
            else if (code.StartsWith('E'))
            {
                return SearchForE(code);
            }
            else if (code.StartsWith('F'))
            {
                return SearchForF(code);
            }
            else if (code.StartsWith('G'))
            {
                return SearchForG(code);
            }
            else if (code.StartsWith('H'))
            {
                return SearchForH(code);
            }
            else if (code.StartsWith('I'))
            {
                return SearchForI(code);
            }
            else if (code.StartsWith('J'))
            {
                return SearchForJ(code);
            }
            else if (code.StartsWith('K'))
            {
                return SearchForK(code);
            }
            else if (code.StartsWith('L'))
            {
                return SearchForL(code);
            }
            else if (code.StartsWith('M'))
            {
                return SearchForM(code);
            }
            else if (code.StartsWith('N'))
            {
                return SearchForN(code);
            }
            else if (code.StartsWith('O'))
            {
                return SearchForO(code);
            }
            else if (code.StartsWith('P'))
            {
                return SearchForP(code);
            }
            else if (code.StartsWith('Q'))
            {
                return SearchForQ(code);
            }
            else if (code.StartsWith('R'))
            {
                return SearchForR(code);
            }
            else if (code.StartsWith('S'))
            {
                return SearchForS(code);
            }
            else if (code.StartsWith('T'))
            {
                return SearchForT(code);
            }
            else if (code.StartsWith('U'))
            {
                return SearchForU(code);
            }
            else if (code.StartsWith('V'))
            {
                return SearchForV(code);
            }
            else if (code.StartsWith('W'))
            {
                return SearchForW(code);
            }
            else if (code.StartsWith('Y'))
            {
                return SearchForY(code);
            }
            else
            {
                return SearchForZ(code);
            }

        }

        private static string SearchForZ(string code)
        {
            switch (code)
            {
                case "ZA":
                    return "South Africa";
                case "ZM":
                    return "Zambia";
                case "ZW":
                    return "Zimbabwe";
                default:
                    return "Zambia";
            }
        }

        private static string SearchForY(string code)
        {
            switch (code)
            {
                case "YE":
                    return "Yemen";
                case "YT":
                    return "Mayotte";
                default:
                    return "Mayotte";
            }
        }

        private static string SearchForW(string code)
        {
            switch (code)
            {
                case "WF":
                    return "Wallis and Futuna";
                case "WS":
                    return "Samoa";
                default:
                    return "Samoa";
            }
        }

        private static string SearchForV(string code)
        {
            switch (code)
            {
                case "VA":
                    return "Holy See";
                case "VC":
                    return "Saint Vincent and the Grenadines";
                case "VE":
                    return "Venezuela, Bolivarian Republic of";
                case "VG":
                    return "Virgin Islands, British";
                case "VI":
                    return "Virgin Islands, U.S.";
                case "VN":
                    return "Viet Nam";
                case "VU":
                    return "Vanuatu";
                default:
                    return "Viet Nam";
            }
        }

        private static string SearchForU(string code)
        {
            switch (code)
            {
                case "UA":
                    return "Ukraine";
                case "UG":
                    return "Uganda";
                case "UM":
                    return "United States Minor Outlying Islands";
                case "US":
                    return "United States of America";
                case "UY":
                    return "Uruguay";
                case "UZ":
                    return "Uzbekistan";
                default:
                    return "Ukraine";
            }
        }

        private static string SearchForT(string code)
        {
            switch (code)
            {
                case "TC":
                    return "Turks and Caicos Islands";
                case "TD":
                    return "Chad";
                case "TF":
                    return "French Southern Territories";
                case "TG":
                    return "Togo";
                case "TH":
                    return "Thailand";
                case "TJ":
                    return "Tajikistan";
                case "TK":
                    return "Tokelau";
                case "TL":
                    return "Timor-Leste";
                case "TM":
                    return "Turkmenistan";
                case "TN":
                    return "Tunisia";
                case "TO":
                    return "Tonga";
                case "TR":
                    return "Turkey";
                case "TT":
                    return "Tuvalu";
                case "TW":
                    return "Taiwan, Province of China";
                case "TZ":
                    return "Tanzania, United Republic of";
                default:
                    return "Turkey";
            }
        }

        private static string SearchForS(string code)
        {
            switch (code)
            {
                case "SA":
                    return "Saudi Arabia";
                case "SB":
                    return "Solomon Islands";
                case "SC":
                    return "Seychelles";
                case "SD":
                    return "Sudan";
                case "SE":
                    return "Sweden";
                case "SG":
                    return "Singapore";
                case "SH":
                    return "Saint Helena, Ascension and Tristan da Cunha";
                case "SI":
                    return "Slovenia";
                case "SJ":
                    return "Svalbard and Jan Mayen";
                case "SK":
                    return "Slovakia";
                case "SL":
                    return "Sierra Leone";
                case "SM":
                    return "San Marino";
                case "SN":
                    return "Senegal";
                case "SO":
                    return "Somalia";
                case "SR":
                    return "Suriname";
                case "SS":
                    return "South Sudan";
                case "ST":
                    return "Sao Tome and Principe";
                case "SV":
                    return "El Salvador";
                case "SX":
                    return "Sint Maarten (Dutch part)";
                case "SY":
                    return "Syrian Arab Republic";
                case "SZ":
                    return "Swaziland";
                default:
                    return "Slovenia";
            }
        }

        private static string SearchForR(string code)
        {
            switch (code)
            {
                case "RE":
                    return "Réunion";
                case "RO":
                    return "Romania";
                case "RS":
                    return "Serbia";
                case "RU":
                    return "Russian Federation";
                case "RW":
                    return "Rwanda";
                default:
                    return "Serbia";
            }
        }

        private static string SearchForQ(string code)
        {
            switch (code)
            {
                case "QA":
                    return "Qatar";
                    break;
                default:
                    return "Qatar";
            }
        }

        private static string SearchForP(string code)
        {
            switch (code)
            {
                case "PA":
                    return "Panama";
                case "PE":
                    return "Peru";
                case "PF":
                    return "French Polynesia";
                case "PG":
                    return "Papua New Guinea";
                case "PH":
                    return "Philippines";
                case "PK":
                    return "Pakistan";
                case "PL":
                    return "Poland";
                case "PM":
                    return "Saint Pierre and Miquelon";
                case "PN":
                    return "Pitcairn";
                case "PR":
                    return "Puerto Rico";
                case "PS":
                    return "Palestine, State of";
                case "PT":
                    return "Portugal";
                case "PW":
                    return "Palau";
                case "PY":
                    return "Paraguay";
                default:
                    return "Panama";
            }
        }

        private static string SearchForO(string code)
        {
            switch (code)
            {
                case "OM":
                    return "Oman";
                    break;
                default:
                    return "Oman";
            }
        }

        private static string SearchForN(string code)
        {
            switch (code)
            {
                case "NA":
                    return "Namibia";
                case "NC":
                    return "New Caledonia";
                case "NE":
                    return "Niger";
                case "NF":
                    return "Norfolk Island";
                case "NG":
                    return "Nigeria";
                case "NI":
                    return "Nicaragua";
                case "NL":
                    return "Netherlands";
                case "NO":
                    return "Norway";
                case "NP":
                    return "Nepal";
                case "NR":
                    return "Nauru";
                case "NU":
                    return "Niue";
                case "NZ":
                    return "New Zealand";
                default:
                    return "Nigeria";
            }
        }

        private static string SearchForM(string code)
        {
            switch (code)
            {
                case "MA":
                    return "Morocco";
                case "MC":
                    return "Monaco";
                case "MD":
                    return "Moldova, Republic of";
                case "ME":
                    return "Montenegro";
                case "MF":
                    return "Saint Martin (French part)";
                case "MG":
                    return "Madagascar";
                case "MH":
                    return "Marshall Islands";
                case "MK":
                    return "Macedonia, the former Yugoslav Republic of";
                case "ML":
                    return "Mali";
                case "MM":
                    return "Myanmar";
                case "MN":
                    return "Mongolia";
                case "MO":
                    return "Macao";
                case "MP":
                    return "Northern Mariana Islands";
                case "MQ":
                    return "Martinique";
                case "MR":
                    return "Mauritania";
                case "MS":
                    return "Montserrat";
                case "MT":
                    return "Malta";
                case "MU":
                    return "Mauritius";
                case "MV":
                    return "Maldives";
                case "MW":
                    return "Malawi";
                case "MX":
                    return "Mexico";
                case "MY":
                    return "Malaysia";
                case "MZ":
                    return "Mozambique";
                default:
                    return "Mozambique";
            }
        }

        private static string SearchForL(string code)
        {
            switch (code)
            {
                case "LA":
                    return "Lao People's Democratic Republic";
                case "LB":
                    return "Lebanon";
                case "LC":
                    return "Saint Lucia";
                case "LI":
                    return "Liechtenstein";
                case "LK":
                    return "Sri Lanka";
                case "LR":
                    return "Liberia";
                case "LS":
                    return "Lesotho";
                case "LT":
                    return "Lithuania";
                case "LU":
                    return "Luxembourg";
                case "LV":
                    return "Latvia";
                default:
                    return "Lebanon";
            }
        }

        private static string SearchForK(string code)
        {
            switch (code)
            {
                case "KE":
                    return "Kenya";
                case "KG":
                    return "Kyrgyzstan";
                case "KH":
                    return "Cambodia";
                case "KI":
                    return "Kiribati";
                case "KM":
                    return "Comoros";
                case "KN":
                    return "Saint Kitts and Nevis";
                case "KP":
                    return "Korea, Democratic People's Republic of";
                case "KR":
                    return "Korea, Republic of";
                case "KW":
                    return "Kuwait";
                case "KY":
                    return "Cayman Islands";
                case "KZ":
                    return "Kazakhstan";
                default:
                    return "Kenya";
            }
        }

        private static string SearchForJ(string code)
        {
            switch (code)
            {
                case "JE":
                    return "Jersey";
                case "JM":
                    return "Jamaica";
                case "JO":
                    return "Jordan";
                case "JP":
                    return "Japan";
                default:
                    return "Japan";
            }
        }

        private static string SearchForI(string code)
        {
            switch (code)
            {
                case "ID":
                    return "Indonesia";
                case "IE":
                    return "Ireland";
                case "IL":
                    return "Israel";
                case "IM":
                    return "Isle of Man";
                case "IN":
                    return "India";
                case "IO":
                    return "British Indian Ocean Territory";
                case "IQ":
                    return "Iraq";
                case "IR":
                    return "Iran, Islamic Republic of";
                case "IS":
                    return "Iceland";
                case "IT":
                    return "Italy";
                default:
                    return "Italy";
            }
        }

        private static string SearchForH(string code)
        {
            switch (code)
            {
                case "HK":
                    return "Hong Kong";
                case "HM":
                    return "Heard Island and McDonalds Islands";
                case "HN":
                    return "Honduras";
                case "HR":
                    return "Croatia";
                case "HT":
                    return "Haiti";
                case "HU":
                    return "Hungary";
                default:
                    return "Hungary";
            }
        }

        private static string SearchForG(string code)
        {
            switch (code)
            {
                case "GA":
                    return "Gabon";
                case "GB":
                    return "United Kingdom of Great Britain and Northern Ireland";
                case "GD":
                    return "Grenada";
                case "GE":
                    return "Georgia";
                case "GF":
                    return "French Guiana";
                case "GG":
                    return "Guernsey";
                case "GH":
                    return "Ghana";
                case "GI":
                    return "Gibraltar";
                case "GL":
                    return "Greenland";
                case "GM":
                    return "Gambia";
                case "GN":
                    return "Guinea";
                case "GP":
                    return "Guadeloupe";
                case "GQ":
                    return "Equatorial Guinea";
                case "GR":
                    return "Greece";
                case "GS":
                    return "South Georgia and the South Sandwich Islands";
                case "GT":
                    return "Guatemala";
                case "GU":
                    return "Guam";
                case "GW":
                    return "Guinea-Bissau";
                case "GY":
                    return "Guyana";
                default:
                    return "Guyana";
            }
        }

        private static string SearchForF(string code)
        {
            switch (code)
            {
                case "FI":
                    return "Finland";
                case "FJ":
                    return "Fiji";
                case "FK":
                    return "Falkland Islands (Malvinas)";
                case "FM":
                    return "Micronesia, Federated States of";
                case "FO":
                    return "Faroe Islands";
                case "FR":
                    return "France";
                default:
                    return "France";
            }
        }

        private static string SearchForE(string code)
        {
            switch (code)
            {
                case "EC":
                    return "Ecuador";
                case "EE":
                    return "Estonia";
                case "EG":
                    return "Egypt";
                case "EH":
                    return "Western Sahara";
                case "ER":
                    return "Eritrea";
                case "ES":
                    return "Spain";
                case "ET":
                    return "Ethiopia";
                default:
                    return "Ecuador";
            }
        }

        private static string SearchForD(string code)
        {
            switch(code)
            {
                case "DE":
                    return "Germany";
                    break;
                case "DJ":
                    return "Djibouti";
                    break;
                case "DK":
                    return "Denmark";
                    break;
                case "DM":
                    return "Dominica";
                    break;
                case "DO":
                    return "Dominican Republic";
                    break;
                case "DZ":
                    return "Algeria";
                    break;
                default:
                    return "Germany";
            }
        }

        private static string SearchForC(string code)
        {
            switch(code)
            {
                case "CA":
                    return "Canada";
                    break;
                case "CC":
                    return "Cocos (Keeling) Islands";
                    break;
                case "CD":
                    return "Congo, the Democratic Republic of";
                    break;
                case "CF":
                    return "Central African Republic";
                    break;
                case "CG":
                    return "Congo";
                    break;
                case "CH":
                    return "Switzerland";
                    break;
                case "CI":
                    return "Côte d'Ivoire";
                    break;
                case "CK":
                    return "Cook Islands";
                    break;
                case "CL":
                    return "Chile";
                    break;
                case "CM":
                    return "Cameroon";
                    break;
                case "CN":
                    return "China";
                    break;
                case "CO":
                    return "Colombia";
                    break;
                case "CR":
                    return "Costa Rica";
                    break;
                case "CU":
                    return "Cuba";
                    break;
                case "CV":
                    return "Cabo Verde";
                    break;
                case "CW":
                    return "Curaçao";
                    break;
                case "CX":
                    return "Christmas Island";
                    break;
                case "CY":
                    return "Cyprus";
                    break;
                case "CZ":
                    return "Czech Republic";
                    break;
                default:
                    return "Canada";
            }
        }

        private static string SearchForB(string code)
        {
            switch(code)
            {
                case "BA":
                    return "Bosnia and Herzegovina";
                    break;
                case "BB":
                    return "Barbados";
                    break;
                case "BD":
                    return "Bangladesh";
                    break;
                case "BE":
                    return "Belgium";
                    break;
                case "BF":
                    return "Burkina Faso";
                    break;
                case "BG":
                    return "Bulgaria";
                    break;
                case "BH":
                    return "Bahrain";
                    break;
                case "BI":
                    return "Burundi";
                    break;
                case "BJ":
                    return "Benin";
                    break;
                case "BL":
                    return "Saint Barthélemy";
                    break;
                case "BM":
                    return "Bermuda";
                    break;
                case "BN":
                    return "Brunei Darussalam";
                    break;
                case "BO":
                    return "Bolivia, Plurinational State of";
                    break;
                case "BQ":
                    return "Bonaire, Sint Eustatius and Saba";
                    break;
                case "BR":
                    return "Brazil";
                    break;
                case "BS":
                    return "Bahamas";
                    break;
                case "BT":
                    return "Bhutan";
                    break;
                case "BV":
                    return "Bouvet Island";
                    break;
                case "BW":
                    return "Botswana";
                    break;
                case "BY":
                    return "Belarus";
                    break;
                case "BZ":
                    return "Belize";
                    break;
                default:
                    return "Belgium";
            }
        }

        private static string SearchForA(string code)
        {
            switch (code)
            {
                case "AD":
                    return "Andorra";
                    break;
                case "AE":
                    return "United Arab Emirates";
                    break;
                case "AF":
                    return "Afghanistan";
                    break;
                case "AG":
                    return "Antigua and Barbuda";
                    break;
                case "AI":
                    return "Anguilla";
                    break;
                case "AL":
                    return "Albania";
                    break;
                case "AM":
                    return "Armenia";
                    break;
                case "AO":
                    return "Angola";
                    break;
                case "AQ":
                    return "Antarctica";
                    break;
                case "AR":
                    return "Argentina";
                    break;
                case "AS":
                    return "American Samoa";
                    break;
                case "AT":
                    return "Austria";
                    break;
                case "AU":
                    return "Australia";
                    break;
                case "AW":
                    return "Aruba";
                    break;
                case "AX":
                    return "Åland Islands";
                    break;
                case "AZ":
                    return "Azerbaijan";
                    break;
                default:
                    return "Andorra";
            }
        }
    }
}
