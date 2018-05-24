using DevExpress.UI.Xaml.Map;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Xml.Linq;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MapControl_Lesson3 {
    public sealed partial class MainPage : Page {
        const string filepath = "Assets\\ships.xml";

        ObservableCollection<MapItem> shipwrecks = new ObservableCollection<MapItem>();
        public ObservableCollection<MapItem> Shipwrecks { get { return shipwrecks; } }

        public MainPage() {
            this.InitializeComponent();
            LoadShipwrecks();
            this.DataContext = this;
        }

        void LoadShipwrecks() {
            string xmlFilepath = Path.Combine(Package.Current.InstalledLocation.Path, filepath);
            XDocument document = XDocument.Load(xmlFilepath);
            foreach (XElement element in document.Element("Ships").Elements()) {
                GeoPoint location = new GeoPoint() {
                    Latitude = Convert.ToDouble(element.Element("Latitude").Value, CultureInfo.InvariantCulture),
                    Longitude = Convert.ToDouble(element.Element("Longitude").Value, CultureInfo.InvariantCulture)
                };
                MapCustomElement e = new MapCustomElement() {
                    Location = location,
                    ContentTemplate = this.Resources["itemTemplate"] as DataTemplate
                };
                e.Attributes.Add(new MapItemAttribute() {
                    Name = "Description",
                    Value = element.Element("Description").Value
                });
                e.Attributes.Add(new MapItemAttribute() {
                    Name = "Name",
                    Value = element.Element("Name").Value
                });
                e.Attributes.Add(new MapItemAttribute() {
                    Name = "Year",
                    Value = element.Element("Year").Value
                });

                shipwrecks.Add(e);
            }
        }
    }

}
