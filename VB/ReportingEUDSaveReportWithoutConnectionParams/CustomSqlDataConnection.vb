Imports DevExpress.DataAccess.ConnectionParameters
Imports DevExpress.DataAccess.Native
Imports DevExpress.DataAccess.Sql

Namespace WinEUDSaveReportWithoutConnectionParams
    Friend Class CustomSqlDataConnection
        Inherits SqlDataConnection
        Implements INamedItem

        Public Sub New(ByVal name As String, ByVal connectionParameters As DataConnectionParametersBase)
            MyBase.New(name, connectionParameters)
        End Sub

        Private Property INamedItem_Name() As String Implements INamedItem.Name
            Get
                Return Name & " (Custom)"
            End Get
            Set(ByVal value As String)
                Name = value
            End Set
        End Property
    End Class
End Namespace

