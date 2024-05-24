using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Rilevazione_temperature
{
    public class Temperatura
    {
        float _valore;
        DateTime _date;

        //costruttore di default
        public Temperatura() {

        }

        //costruttore parametrico con controllo sul valore passato
        //valido soltanto se >= di -10 e <= 50
        public Temperatura(float v, DateTime d) {
            if (valore >= -10 && valore <= 50) {
                valore = v;
                date = d;
            }
        }

        public override string ToString() {
            return valore.ToString() + "°C " + date.ToString();
        }

        //metodo che, data una stringa, crea la corrispondente temperatura se passa i controlli
        public static Temperatura Split(string riga) {
            //cerco la posizione di "°C" e dello spazio
            int posGradi = riga.IndexOf("°C");
            int posSpazio = riga.IndexOf(" ");

            //se l'indice delle strighe cercate è > 0, allora entra nell'if
            if (posGradi > 0 && posSpazio > 0)
            {
                //creo due strighe
                //la prima rappresentante il valore (dall'inizio fino alla posGradi)
                //la seconda rappresentante data e ora (dalla posSpazio + 1 fino alla fine)
                string valore = riga.Substring(0, posGradi);
                string data = riga.Substring(posSpazio + 1);

                float temp;
                DateTime d;
                //provo a convertire le due strighe sopra create
                //se le conversioni restituiscono true, allora creo la temperatura correttamente
                //se restituiscono false, allora ne creo una senza valore
                if (float.TryParse(valore, out temp) && DateTime.TryParse(data, out d) && d != new DateTime(0001, 01, 01))
                    return new Temperatura(temp, d);
                else
                    return null;
            }
            else
                return new Temperatura();
        }

        public SolidColorBrush getColorTemperature() {
            //assegno un colore alla temperatura in base al valore
            //<= 0 --> molto freddo --> blu scuro
            //0 < valore <= 12 --> freddo --> azzurro
            //12 < valore <= 22 --> caldo --> arancione
            //valore > 22 --> molto caldo --> rosso

            if (valore <= 0) {
                return Brushes.DarkBlue;
            } else if (valore <= 12) {
                return Brushes.LightBlue;
            } else if (valore <= 22) {
                return Brushes.OrangeRed;
            } else {
                return Brushes.DarkRed;
            }
        }

        public float valore {
            get { return _valore; }
            set { _valore = value; }
        }

        public DateTime date {
            get { return _date; }
            set { _date = value; }
        }
    }
}
