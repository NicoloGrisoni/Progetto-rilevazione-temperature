using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Rilevazione_temperature
{
    /// <summary>
    /// Logica di interazione per FinestraDettagli.xaml
    /// </summary>
    public partial class FinestraDettagli : Window
    {
        public FinestraDettagli(Temperatura temp, float media)
        {
            InitializeComponent();

            //assegno i text delle due textbox
            //la prima al valore della temperatura
            //la seconda alla data e all'ora di rilevazione della temperatura
            TextBox_Valore.Text = temp.valore.ToString() + "°C";
            TextBox_Data.Text = temp.date.ToString();

            //imposto la CheckBox.Checked a true se il valore della temperatura è sopra la media
            //altrimenti la imposto a false
            if (temp.valore > media)
                CheckBox_SopraMedia.IsChecked = true;
            else
                CheckBox_SopraMedia.IsChecked = false;
        }
    }
}
