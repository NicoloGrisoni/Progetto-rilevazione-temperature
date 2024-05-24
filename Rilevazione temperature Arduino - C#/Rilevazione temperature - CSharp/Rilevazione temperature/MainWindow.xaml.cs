using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Converters;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rilevazione_temperature
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Temperatura> rilevazioni;
        SerialPort com;

        public MainWindow() {
            InitializeComponent();

            //imposto larghezza e altezza dello scrollviewer a quelle del canvas
            ScrollViewer_Scorrimento.Width = Canvas_Grafico.Width;
            //ScrollViewer_Scorrimento.Height = Canvas_Grafico.Height;


            //inizializzo la lista di temperature, la listbox e il grafico
            rilevazioni = new ObservableCollection<Temperatura>();
            ListBox_Temperature.ItemsSource = rilevazioni;
            rilevazioni.CollectionChanged += Rilevazioni_CollectionChanged;
            //disegno l'istogramma delle temperature contenute nella lista
            disegnaIstogramma();

            //richiamo il metodo che prende le temperature salvate sul file e inserisce nella lista
            temperatureSalvate();

            //visualizzo la media delle temperature tramite il contenuto della label
            Label_Media.Content = "Media: " + mediaValoriTemperature();


            //inizializzo la porta seriale
            com = new SerialPort("COM3", 9600);
            //avvio la connessione con la porta seriale richiamando il metodo Open
           // com.Open();
            //aggiungo l'evento DataReceived, sollevato quando avviene una ricezione di dati
            com.DataReceived += Com_DataReceived;
        }

        private void Rilevazioni_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            disegnaIstogramma();
        }

        private void Com_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //leggo il dato della temperatura su seriale col metodo ReadLine
            //lo inserisco in una stringa e rimpiazzo eventuali "." con ","
            String riga = com.ReadLine().Replace(".", ",");

            float temperatura;
            //provo a convertire la stringa in un dato di tipo float
            //se il metodo TryParse restituisce true, allora il codice nell'if viene eseguito
            if (float.TryParse(riga, out temperatura))
            {
                //creo una nuova temperatura
                //valore = dato convertito con successo
                //date = data e ora al momento della rilevazione
                Temperatura temp = new Temperatura(temperatura, DateTime.Now);

                //aggiungo la temperatura alla lista di temperature
                rilevazioni.Add(temp);
            }
        }

        //metodo sollevato all'evento TextChanged della TextBox_Ricerca
        //con questo metodo posso eseguire una ricerca tra le temperature rilevate
        //il risultato della ricerca viene visualizzato man mano sulla listbox
        //nulla viene rimosso dalla lista di temperature, cambiano soltanto le temperature che vengono visualizzate
        private void TextBox_Ricerca_TextChanged(object sender, TextChangedEventArgs e) {
            //rimuovo tutti gli elementi dalla listbox
            ListBox_Temperature.Items.Clear();
            //scorro tramite un ciclo for tutta la lista di temperature
            for (int i = 0; i < rilevazioni.Count; i++) {
                //se il text della textbox è presente all'interno del ToString della temperatura nella lista in indice i
                //allora la inserisco anche nella listbox
                if (rilevazioni[i].ToString().Contains(TextBox_Ricerca.Text))
                    ListBox_Temperature.Items.Add(rilevazioni[i]);
            }
        }

        //metodo sollevato all'evento Click del Button_TemperatureRilevate
        //viene richiamato il metodo per inserire le temperature nella listbox
        //viene richiamato anche il metodo per calcolare la media, e poi viene aggiornato il contenuto della label
        private void Button_TemperatureRilevate_Click(object sender, RoutedEventArgs e) {
            //aggiungiRilevazioniAListBox();
            disegnaIstogramma();

            //aggiorno il contenuto della label
            Label_Media.Content = "Media: " + mediaValoriTemperature();
        }

        //metodo sollevato all'evento Click del Button_Dettagli
        //con questo metodo posso visualizzare singolarmente la temperatura selezionata tramite la listbox
        private void Button_Dettagli_Click(object sender, RoutedEventArgs e) {
            //prendo la temperatura selezionata usando la proprietà SelectedItem della listbox
            Temperatura temp = ListBox_Temperature.SelectedItem as Temperatura;
            if (temp == null)
                return;

            //creo e apro una nuova finestra
            //passo la temperatura da visualizzare singolarmente e la media delle temperature
            FinestraDettagli finestraDettagli = new FinestraDettagli(temp, mediaValoriTemperature());
            finestraDettagli.ShowDialog();
        }

        //metodo sollevato all'evento Click del Button_Elimina
        //con questo metodo posso rimuovere la temperatura selezionata tramite la listbox
        private void Button_Elimina_Click(object sender, RoutedEventArgs e) {
            //prendo la temperatura selezionata usando la proprietà SelectedItem della listbox
            Temperatura temp = ListBox_Temperature.SelectedItem as Temperatura;
            if (temp == null)
                return;

            //rimuovo la temperatura dalla lista di temperature
            rilevazioni.Remove(temp);
            //aggiorno il contenuto della Label_Media
            Label_Media.Content = "Media: " + mediaValoriTemperature();
            //disegno di nuovo l'istogramma avendo tolto la temperatura selezionata
            disegnaIstogramma();

            //richiamo il metodo per scrivere le temperature sul file
            scriviTemperatureSuFile();

            //rimuovo l'elemento anche dalla listbox
            //aggiungiRilevazioniAListBox();
        }

        //metodo che permette di ricavare tutte le temperature salvate nel file e inserirle nuovamente nella lista di temperature
        private void temperatureSalvate() {
            //apro in lettura il file
            StreamReader sr = new StreamReader("temperature.txt");
            //inserisco l'intero contenuto del file in una stringa
            String contenuto = sr.ReadToEnd();

            //se la stringa non è vuota, il controllo viene passato ed entra nell'if
            if (contenuto.Length > 0) {
                //creo un vettore di stringhe usando il metodo split delle strighe
                //ogni stringa di questo vettore rappresenta una temperatura salvata
                //ogni temperatura è ancora da riconvertire
                String[] righe = contenuto.Split('\n');

                //scorro il vettore di stringhe
                for (int i = 0; i < righe.Length; i++) {
                    //se la riga selezionata non è vuota, entra nell'if
                    if (righe[i].Length > 0) {
                        //faccio la riconversione della riga in temperatura tramite il metodo split della classe temperatura
                        Temperatura tempDaInserire = Temperatura.Split(righe[i]);
                        //aggiungo la temperatura nella lista con controllo
                        if (tempDaInserire != null)
                            rilevazioni.Add(tempDaInserire);
                    }
                }
            }

            //chiudo la lettura del file
            sr.Close();
        }

        //metodo che calcola la media dei valori delle temperature nella lista
        public float mediaValoriTemperature() {
            float tot = 0;
            for (int i = 0; i < rilevazioni.Count; i++) {
                tot += rilevazioni[i].valore;
            }
            return tot / rilevazioni.Count;
        }

        //metodo che disegna nel canvas i rettangoli rappresentanti ciascuno il valore di una temperatura nella lista
        public void disegnaIstogramma() {
            //rimuovo tutti gli elementi presenti nel canvas
            Canvas_Grafico.Children.Clear();
            //ridimensiono la larghezza del canvas in base alla quantità di temperature della lista
            Canvas_Grafico.Width = rilevazioni.Count * 25 + rilevazioni.Count * 12;

            int x = 30;
            //scorro la lista di rilevazioni tramite il ciclo for
            for (int i = 0; i < rilevazioni.Count; i++) {
                //creo un rettangolo di larghezza 25 e di altezza variabile in base al valore della temperatura
                Rectangle rect = new Rectangle() {
                    Width = 25,
                    Height = rilevazioni[i].valore * 12,
                };

                Brush colore = rilevazioni[i].getColorTemperature();
                rect.Fill = colore;

                //imposto le coordinate del rettangolo nel canvas
                Canvas.SetLeft(rect, x);    //coordinata x
                Canvas.SetBottom(rect, 0);  //coordinata y

                //aggiungo il rettangolo al canvas
                Canvas_Grafico.Children.Add(rect);

                Label numeroTemperatura = new Label {
                    Width = rect.Width,
                    Height = 27,

                    Content = (i + 1).ToString(),
                    HorizontalContentAlignment = HorizontalAlignment.Center,

                    Foreground = Brushes.White,
                };

                Canvas.SetLeft(numeroTemperatura, x);
                Canvas.SetBottom(numeroTemperatura, 0);

                Canvas_Grafico.Children.Add(numeroTemperatura);

                x += 35;
            }
        }

        public void scriviTemperatureSuFile() {
            //apro il file il scrittura per sovrascriverne il contenuto
            StreamWriter sw = new StreamWriter("temperature.txt");
            //scorro la lista di temperature dalla quale ho rimosso quella da eliminare
            for (int i = 0; i < rilevazioni.Count; i++) {
                //scrivo riga per riga il ToString delle temperature rimaste nella lista
                sw.WriteLine(rilevazioni[i].ToString());
            }
            //chiudo la scrittura del file
            sw.Close();
        }
    }
}