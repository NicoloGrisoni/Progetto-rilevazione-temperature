# Progetto rilevazione temperature Arduino-C#

## 🖋️Autore:
> Nicolò Grisoni

## 📒Descrizione:
> Il DHT Sensor di Arduino dev'essere prima di tutto configurato e collegato, utilizzando la libreria e seguendo lo schema di collegamento, entrambi disponibili nella cartella relativa ad Arduino.
> Una volta fatto questo, il sensore invia ogni 20 secondi una temperatura rilevata sulla porta seriale così che il programma scritto in C# possa leggere questo dato.
> Il programma controlla la validità del dato ricevuto, e se risulta tutto corretto, salva la temperatura su una lista.
> In automatico, all'inserimento nella lista di un nuovo dato, una ListBox e un Canvas vengono aggiornati, permettendo quindi all'utente di visualizzare graficamente la nuova temperatura appena ricevuta.
> Tutte le temperature vengono salvate su un file di testo in formato csv, così che in ogni momento si possa accedere ai dati, e per fare in modo che all'avvio del programma, vengano ricaricate correttamente le temperature sulla ListBox e nel Canvas.
> Per ulteriori informazioni sullo sviluppo del progetto e sul suo funzionamento, sono disponibili una descrizione in un file di testo e una presentazione in versione powerpoint

## 🔑Licenza
> Questo progetto è rilasciato sotto la Licenza MIT. Per ulteriori informazioni, visita [MIT License](https://articles.opexflow.com/it/programming/license-github.htm).P
