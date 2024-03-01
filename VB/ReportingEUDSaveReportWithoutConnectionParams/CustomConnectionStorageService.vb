Imports DevExpress.DataAccess.ConnectionParameters
Imports DevExpress.DataAccess.Sql
Imports DevExpress.DataAccess.Wizard.Model
Imports DevExpress.DataAccess.Wizard.Native
Imports DevExpress.DataAccess.Wizard.Services
Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Windows.Forms
Imports System.Xml

Namespace WinEUDSaveReportWithoutConnectionParams

    Friend Class CustomConnectionStorageService
        Implements IConnectionStorageService

        Public ReadOnly Property CanSaveConnection As Boolean Implements IConnectionStorageService.CanSaveConnection
            Get
                Return True
            End Get
        End Property

        Private Function Contains(ByVal connectionName As String) As Boolean Implements IConnectionStorageService.Contains
            Return If(IncludeApplicationConnections, DefaultStorage.Contains(connectionName) OrElse ConnectionProviderService.GetConnectionsFromXml().Any(Function(c) String.Equals(c.Name, connectionName)), ConnectionProviderService.GetConnectionsFromXml().Any(Function(c) String.Equals(c.Name, connectionName)))
        End Function

        Private Function GetConnections() As IEnumerable(Of SqlDataConnection) Implements IConnectionStorageService.GetConnections
            Return If(IncludeApplicationConnections, DefaultStorage.GetConnections().Union(ConnectionProviderService.GetConnectionsFromXml()), ConnectionProviderService.GetConnectionsFromXml())
        End Function

        Private Sub CreateDefaultConnection()
            If Not File.Exists(FileName) Then
                Try
                    Dim doc As XmlDocument = New XmlDocument()
                    Dim root As XmlElement = doc.CreateElement(xmlRootName)
                    doc.AppendChild(root)
                    doc.Save(FileName)
                    Dim defaultConnection As SqlDataConnection = New CustomSqlDataConnection("Default Connection", New MsSqlConnectionParameters("localhost", "NORTHWND", "", "", MsSqlAuthorizationType.Windows))
                    SaveConnection(defaultConnection.Name, defaultConnection, True)
                Catch ex As Exception
                    MessageBox.Show(String.Format("Cannot create '{0}' file because of exception:{1}{1}{2}", FileName, Environment.NewLine, ex.Message))
                End Try
            End If
        End Sub

        Public Sub SaveConnection(ByVal connectionName As String, ByVal connection As IDataConnection, ByVal saveCredentials As Boolean) Implements IConnectionStorageService.SaveConnection
            connection.StoreConnectionNameOnly = True
            Try
                Dim doc As XmlDocument = New XmlDocument()
                Dim root As XmlElement = Nothing
                If File.Exists(FileName) Then
                    doc.Load(FileName)
                    root = doc.DocumentElement
                    If root IsNot Nothing Then
                        If Not Equals(root.Name, xmlRootName) Then
                            MessageBox.Show(String.Format("Document element is '{0}', '{1}' expected", root.Name, xmlRootName))
                            Return
                        End If

                        If root.SelectSingleNode(String.Format("Connection[Name = '{0}']", connectionName)) IsNot Nothing Then Return
                    End If
                End If

                If root Is Nothing Then
                    root = doc.CreateElement(xmlRootName)
                    doc.AppendChild(root)
                End If

                Dim nameElement As XmlElement = doc.CreateElement("Name")
                nameElement.AppendChild(doc.CreateTextNode(connectionName))
                Dim connectionStringElement As XmlElement = doc.CreateElement("ConnectionString")
                connectionStringElement.AppendChild(doc.CreateTextNode(connection.CreateConnectionString(Not saveCredentials)))
                Dim connectionElement As XmlElement = doc.CreateElement("Connection")
                connectionElement.AppendChild(nameElement)
                connectionElement.AppendChild(connectionStringElement)
                root.AppendChild(connectionElement)
                doc.Save(FileName)
            Catch ex As Exception
                MessageBox.Show(String.Format("Cannot save connection to '{0}' because of exception:{1}{1}{2}", FileName, Environment.NewLine, ex.Message))
            End Try
        End Sub

        Const xmlRootName As String = "Connections"

        Public Sub New(ByVal connectionProviderService As CustomConnectionProviderService)
            CreateDefaultConnection()
            Me.ConnectionProviderService = connectionProviderService
        End Sub

        Public ReadOnly Property ConnectionProviderService As CustomConnectionProviderService

        Public Property FileName As String = "connections.xml"

        Public Property IncludeApplicationConnections As Boolean = False

        Protected ReadOnly Property DefaultStorage As ConnectionStorageService = New ConnectionStorageService()
    End Class
End Namespace
