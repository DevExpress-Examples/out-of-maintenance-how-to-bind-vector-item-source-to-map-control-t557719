Imports DevExpress.UI.Xaml.Map
Imports System
Imports System.Collections.ObjectModel
Imports System.Globalization
Imports System.IO
Imports System.Xml.Linq
Imports Windows.ApplicationModel
Imports Windows.UI.Xaml
Imports Windows.UI.Xaml.Controls

Namespace MapControl_Lesson3
    Public NotInheritable Partial Class MainPage
        Inherits Page

        Private Const filepath As String = "Assets\ships.xml"


        Private shipwrecks_Renamed As New ObservableCollection(Of MapItem)()
        Public ReadOnly Property Shipwrecks() As ObservableCollection(Of MapItem)
            Get
                Return shipwrecks_Renamed
            End Get
        End Property

        Public Sub New()
            Me.InitializeComponent()
            LoadShipwrecks()
            Me.DataContext = Me
        End Sub

        Private Sub LoadShipwrecks()
            Dim xmlFilepath As String = Path.Combine(Package.Current.InstalledLocation.Path, filepath)
            Dim document As XDocument = XDocument.Load(xmlFilepath)
            For Each element As XElement In document.Element("Ships").Elements()
                Dim location As New GeoPoint() With { _
                    .Latitude = Convert.ToDouble(element.Element("Latitude").Value, CultureInfo.InvariantCulture), _
                    .Longitude = Convert.ToDouble(element.Element("Longitude").Value, CultureInfo.InvariantCulture) _
                }
                Dim e As New MapCustomElement() With { _
                    .Location = location, _
                    .ContentTemplate = TryCast(Me.Resources("itemTemplate"), DataTemplate) _
                }
                e.Attributes.Add(New MapItemAttribute() With { _
                    .Name = "Description", _
                    .Value = element.Element("Description").Value _
                })
                e.Attributes.Add(New MapItemAttribute() With { _
                    .Name = "Name", _
                    .Value = element.Element("Name").Value _
                })
                e.Attributes.Add(New MapItemAttribute() With { _
                    .Name = "Year", _
                    .Value = element.Element("Year").Value _
                })

                shipwrecks_Renamed.Add(e)
            Next element
        End Sub
    End Class

End Namespace
