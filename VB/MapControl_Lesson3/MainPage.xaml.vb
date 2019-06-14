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

'INSTANT VB NOTE: The field shipwrecks was renamed since Visual Basic does not allow fields to have the same name as other class members:
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
				Dim location As New GeoPoint() With {
					.Latitude = Convert.ToDouble(element.Element("Latitude").Value, CultureInfo.InvariantCulture),
					.Longitude = Convert.ToDouble(element.Element("Longitude").Value, CultureInfo.InvariantCulture)
				}
				Dim e As New MapCustomElement() With {
					.Location = location,
					.ContentTemplate = TryCast(Me.Resources("itemTemplate"), DataTemplate)
				}
				e.Attributes.Add(New MapItemAttribute() With {
					.Name = "Description",
					.Value = element.Element("Description").Value
				})
				e.Attributes.Add(New MapItemAttribute() With {
					.Name = "Name",
					.Value = element.Element("Name").Value
				})
				e.Attributes.Add(New MapItemAttribute() With {
					.Name = "Year",
					.Value = element.Element("Year").Value
				})

				shipwrecks_Renamed.Add(e)
			Next element
		End Sub
	End Class

End Namespace
