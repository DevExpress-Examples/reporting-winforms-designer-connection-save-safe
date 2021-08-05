Imports DevExpress.DataAccess.ConnectionParameters
Imports DevExpress.DataAccess.Sql
Imports DevExpress.DataAccess.Wizard.Services
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Windows.Forms
Imports System.Xml
Namespace WinEUDSaveReportWithoutConnectionParams
    Public Class CustomConnectionProviderService
        Implements IConnectionProviderService

        Public Function LoadConnection(ByVal connectionName As String) As SqlDataConnection Implements IConnectionProviderService.LoadConnection
            Dim connection As SqlDataConnection = GetConnectionFromXml(connectionName)
            Dim result = If(connection IsNot Nothing, connection, GetFallbackConnection(connectionName))
            Return result
        End Function
        Private Function GetFallbackConnection(ByVal connectionName As String) As SqlDataConnection
            Throw New Exception("Connection was not found!")
            'or provide a custom fallback connection 
            'return new SqlDataConnection(connectionName, new MsSqlConnectionParameters("localhost", "dataBaseName", "userName", "password", MsSqlAuthorizationType.Windows));
        End Function

        Public Property FileName() As String = "connections.xml"

        Protected Function GetConnectionFromXml(ByVal connectionName As String) As SqlDataConnection
            Dim result As SqlDataConnection = GetConnectionsFromXml(Function(x) x = connectionName).FirstOrDefault()
            Return result
        End Function
        Friend Function GetConnectionsFromXml() As IEnumerable(Of SqlDataConnection)
            Return GetConnectionsFromXml(Function(x) True)
        End Function
        Private Function GetConnectionsFromXml(ByVal action As Predicate(Of String)) As IEnumerable(Of SqlDataConnection)
            Dim result = New List(Of SqlDataConnection)()
            Try
                Dim doc As New XmlDocument()
                doc.Load(FileName)
                For Each node As XmlNode In doc.SelectNodes("Connections/Connection[Name][ConnectionString]")
                    Dim connectionName As String = node("Name").InnerText
                    If action(connectionName) Then
                        Dim connection As New CustomSqlDataConnection(connectionName, New CustomStringConnectionParameters(node("ConnectionString").InnerText))
                        connection.StoreConnectionNameOnly = True
                        result.Add(connection)
                    End If
                Next node

                Return result
            Catch ex As Exception
                MessageBox.Show(String.Format("Cannot get connections from '{0}' because of exception:{1}{1}{2}", FileName, Environment.NewLine, ex.Message))
                Return New SqlDataConnection(){}
            End Try
        End Function
    End Class
End Namespace

