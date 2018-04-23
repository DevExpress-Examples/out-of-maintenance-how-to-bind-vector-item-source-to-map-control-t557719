Imports System.Globalization
Imports DevExpress.UI.Xaml.Map

Public NotInheritable Class MainPage
    Inherits Page

    Private Const filepath As String = "Assets\Ships.xml"

    Private pvtShipwrecks As New ObservableCollection(Of MapItem)()
    Public ReadOnly Property Shipwrecks() As ObservableCollection(Of MapItem)
        Get
            Return pvtShipwrecks
        End Get
    End Property


    Private Sub Page_Loaded(sender As Object, e As RoutedEventArgs)
        LoadShipwrecks()
        Me.DataContext = Me
    End Sub

    Private Sub LoadShipwrecks()
        Dim xmlFilepath As String = Path.Combine(Package.Current.InstalledLocation.Path, filepath)
        Dim document As XDocument = XDocument.Load(xmlFilepath)
        For Each element As XElement In document.Element("Ships").Elements()
            Dim location As New GeoPoint() With {
                .Latitude = Convert.ToDouble(element.Element("Latitude").Value, CultureInfo.InvariantCulture),
                .Longitude = Convert.ToDouble(element.Element("Longitude").Value, CultureInfo.InvariantCulture)
            }
            Dim e As New MapCustomElement() With {
                .Location = location,
                .ContentTemplate = TryCast(Me.Resources("itemTemplate"), DataTemplate)
            }
            e.Attributes.Add(New MapItemAttribute() With {.Name = "Description", .Value = element.Element("Description").Value})
            e.Attributes.Add(New MapItemAttribute() With {.Name = "Name", .Value = element.Element("Name").Value})
            e.Attributes.Add(New MapItemAttribute() With {.Name = "Year", .Value = element.Element("Year").Value})

            pvtShipwrecks.Add(e)
        Next element
    End Sub
End Class
