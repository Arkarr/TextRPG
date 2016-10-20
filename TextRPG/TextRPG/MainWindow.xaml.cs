using Engine;
using Engine.Serialization;
using Engine.Text;
using Engine.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TextRPG
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //Console.WriteLine(234.567.ToString("0.", CultureInfo.InvariantCulture));

            /*GameAttribute a = new GameAttribute();
            a.Current = 4.255467346436367;
            a.Max = 100.0;
            a.Min = 0.0;
            a.FormatString = "$cur% / $max%";
            a.Abbreviation = "STR";

            AttributeDescriptor text = new AttributeDescriptor(a);
            text.SimpleText = "increase life by {$this.current * 5}";
            Console.WriteLine(text.GetResolvedText());


            Console.WriteLine(a.Text);*/

            /*DataFile file = new DataFile();
            file.bytes = System.IO.File.ReadAllBytes("pc_arch_yakuza_mansion.bin").ToList();
            file.WriteToFile(new System.IO.FileInfo("test.txt"));*/

            Debug.DebugMessagesEnabled = true;

            GameAttribute str = new GameAttribute("Strength");
            str.Abbreviation = "STR";
            str.Min = 0;
            str.Max = 100;
            str.Set(500); // trigger
            str.Set(10);
            str.Decrease(30); // trigger
            Console.WriteLine(str.Current);
            str.Increase(200); // trigger
            str.Decrease(85);
            Console.WriteLine(str.Current);

            DerivedGameAttribute crit = new DerivedGameAttribute("Critical");
            crit.DependantAttributes.Add(str);
            crit.Max = 100;
            crit.Min = 0;
            crit.ValueExpression.SimpleText = "$str * 3.552";
            crit.FormatString = "$cur.1% / $max%";
            Console.WriteLine(crit.DefaultText);

            Resource hp = new Resource("Health Points");
            hp.Abbreviation = "HP";
            hp.Max = 200;
            hp.Set(87);
            hp.FormatString = "$cur / $max || $per.1%";
            Console.WriteLine(hp.DefaultText);

        }
    }
}
