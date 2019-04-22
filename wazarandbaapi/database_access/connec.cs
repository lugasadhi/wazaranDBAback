using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using wazarandbaapi.Models;

namespace wazarandbaapi.database_access
{
    public class connec
    {
        public SqlConnection serverChoice(string d)
        {
            string cn;
            switch (d)
            {
                case "app":
                    cn = "wzdba";
                    break;
                case "app-sbtc":
                    cn = "locsbtc";
                    break;
                case "app-master":
                    cn = "wzdbamaster";
                    break;
                case "app-msdb":
                    cn = "wzdbmsdb";
                    break;

                //database ho
                case "ho":
                    cn = "sbtcho";
                    break;
                case "ho-master":
                    cn = "mstho";
                    break;
                case "ho-msdb":
                    cn = "msdbho";
                    break;

                //database baha
                case "baha":
                    cn = "sbtcbaha";
                    break;
                case "baha-master":
                    cn = "mstbaha";
                    break;
                case "baha-msdb":
                    cn = "msdbbaha";
                    break;

                //qunfuda
                case "qunfuda":
                    cn = "sbtcqnf";
                    break;
                case "qunfuda-master":
                    cn = "mstqnf";
                    break;
                case "qunfuda-msdb":
                    cn = "msdbqnf";
                    break;

                //khamis
                case "khamis":
                    cn = "sbtckhm";
                    break;
                case "khamis-master":
                    cn = "mstkhm";
                    break;
                case "khamis-msdb":
                    cn = "msdbkhm";
                    break;

                //ghasem
                case "ghasem":
                    cn = "sbtcgsm";
                    break;
                case "ghasem-master":
                    cn = "mstgsm";
                    break;
                case "ghasem-msdb":
                    cn = "msdbgsm";
                    break;

                //tabuk
                case "tabuk":
                    cn = "sbtctbk";
                    break;
                case "tabuk-master":
                    cn = "msttbk";
                    break;
                case "tabuk-msdb":
                    cn = "msdbtbk";
                    break;

                //jubail
                case "jubail":
                    cn = "sbtcjbl";
                    break;
                case "jubail-master":
                    cn = "mstjbl";
                    break;
                case "jubail-msdb":
                    cn = "msdbjbl";
                    break;

                //hufuf
                case "hufuf":
                    cn = "sbtchff";
                    break;
                case "hufuf-master":
                    cn = "msthff";
                    break;
                case "hufuf-msdb":
                    cn = "msdbhff";
                    break;

                //skaka
                case "skaka":
                    cn = "sbtcskk";
                    break;
                case "skaka-master":
                    cn = "mstskk";
                    break;
                case "skaka-msdb":
                    cn = "msdbskk";
                    break;

                //kharaj
                case "kharaj":
                    cn = "sbtckrj";
                    break;
                case "kharaj-master":
                    cn = "mstkrj";
                    break;
                case "kharaj-msdb":
                    cn = "msdbkrj";
                    break;

                //taif
                case "taif":
                    cn = "sbtctf";
                    break;
                case "taif-master":
                    cn = "msttf";
                    break;
                case "taif-msdb":
                    cn = "msdbtf";
                    break;

                //yanbu
                case "yanbu":
                    cn = "sbtcyb";
                    break;
                case "yanbu-master":
                    cn = "mstyb";
                    break;
                case "yanbu-msdb":
                    cn = "msdbyb";
                    break;

                //jizan
                case "jizan":
                    cn = "sbtcjz";
                    break;
                case "jizan-master":
                    cn = "mstjz";
                    break;
                case "jizan-msdb":
                    cn = "msdbjz";
                    break;

                //hail
                case "hail":
                    cn = "sbtchl";
                    break;
                case "hail-master":
                    cn = "msthl";
                    break;
                case "hail-msdb":
                    cn = "msdbhl";
                    break;

                //madinah
                case "madinah":
                    cn = "sbtcmdn";
                    break;
                case "madinah-master":
                    cn = "mstmdn";
                    break;
                case "madinah-msdb":
                    cn = "msdbmdn";
                    break;

                //dwabmi
                case "dawabmi":
                    cn = "sbtcdwb";
                    break;
                case "dawabmi-master":
                    cn = "mstdwb";
                    break;
                case "dawabmi-msdb":
                    cn = "msdbdwb";
                    break;


                //hafrbatin
                case "hafrbatin":
                    cn = "sbtchfrbt";
                    break;
                case "hafrbatin-master":
                    cn = "msthfrbt";
                    break;
                case "hafrbatin-msdb":
                    cn = "msdbhfrbt";
                    break;


                //najran
                case "najran":
                    cn = "sbtcnjrn";
                    break;
                case "najran-master":
                    cn = "mstnjrn";
                    break;
                case "najran-msdb":
                    cn = "msdbnjrn";
                    break;

                //bisha
                case "bisha":
                    cn = "sbtcbsh";
                    break;
                case "bisha-master":
                    cn = "mstbsh";
                    break;
                case "bisha-msdb":
                    cn = "msdbbsh";
                    break;

                //jeddah
                case "jeddah":
                    cn = "sbtcjdh";
                    break;
                case "jeddah-master":
                    cn = "mstjdh";
                    break;
                case "jeddah-msdb":
                    cn = "msdbjdh";
                    break;

                //riyad
                case "riyad":
                    cn = "sbtcryd";
                    break;
                case "riyad-master":
                    cn = "mstryd";
                    break;
                case "riyad-msdb":
                    cn = "msdbryd";
                    break;

                //khobar
                case "khobar":
                    cn = "sbtckbar";
                    break;
                case "khobar-master":
                    cn = "mstkbar";
                    break;
                case "khobar-msdb":
                    cn = "msdbkbar";
                    break;

                //mekkah
                case "mekkah":
                    cn = "sbtcmkh";
                    break;
                case "mekkah-master":
                    cn = "mstmkh";
                    break;
                case "mekkah-msdb":
                    cn = "msdbmkh";
                    break;

                //dev
                case "dev":
                    cn = "sbtcdev";
                    break;
                case "dev-master":
                    cn = "mstdev";
                    break;
                case "dev-msdb":
                    cn = "msdbdev";
                    break;

                //fic ho
                case "fic-ho":
                    cn = "ficho";
                    break;
                case "fic-ho-master":
                    cn = "mstficho";
                    break;
                case "fic-ho-msdb":
                    cn = "msdbficho";
                    break;

                //fic jizan
                case "fic-jizan":
                    cn = "ficjzn";
                    break;
                case "fic-jizan-master":
                    cn = "mstficjzn";
                    break;
                case "fic-jizan-msdb":
                    cn = "msdbficjzn";
                    break;

                //fic medina
                case "fic-medina":
                    cn = "ficmdn";
                    break;
                case "fic-medina-master":
                    cn = "mstficmdn";
                    break;
                case "fic-medina-msdb":
                    cn = "msdbficmdn";
                    break;

                //fic jeddah
                case "fic-jeddah":
                    cn = "ficjdh";
                    break;
                case "fic-jeddah-master":
                    cn = "mstficjdh";
                    break;
                case "fic-jeddah-msdb":
                    cn = "msdbficjdh";
                    break;

                //fic riyad
                case "fic-riyad":
                    cn = "ficryd";
                    break;
                case "fic-riyad-master":
                    cn = "mstficryd";
                    break;
                case "fic-riyad-msdb":
                    cn = "msdbficryd";
                    break;

                //fic khobar
                case "fic-khobar":
                    cn = "fickbr";
                    break;
                case "fic-khobar-master":
                    cn = "mstfickbr";
                    break;
                case "fic-khobar-msdb":
                    cn = "msdbfickbr";
                    break;

                //fic mekah
                case "fic-mekah":
                    cn = "ficmkh";
                    break;
                case "fic-mekah-master":
                    cn = "mstficmkh";
                    break;
                case "fic-mekah-msdb":
                    cn = "msdbficmkh";
                    break;

                default:
                    cn = "wzdba";
                    break;
            }

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[cn].ConnectionString);
            return con;
        }

    }
}