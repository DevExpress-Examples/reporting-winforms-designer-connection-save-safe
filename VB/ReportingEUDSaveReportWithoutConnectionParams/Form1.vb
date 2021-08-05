Imports DevExpress.DataAccess.Wizard.Services
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraReports.UserDesigner
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Windows.Forms

Namespace WinEUDSaveReportWithoutConnectionParams
    Partial Public Class Form1
        Inherits Form

        Public Sub New()
            InitializeComponent()
        End Sub
        Private Sub simpleButton1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles simpleButton1.Click
            OpenReportDesigner()
        End Sub
        Private connectionStorageService As CustomConnectionStorageService
        Private Sub OpenReportDesigner()
            Dim connectionProviderService As New CustomConnectionProviderService()
            connectionStorageService = New CustomConnectionStorageService(connectionProviderService) With { _
                .FileName = "connections.xml", _
                .IncludeApplicationConnections = False _
            }
            Dim designer As New ReportDesignTool(New XtraReport())
            'designer.DesignRibbonForm.DesignMdiController.DataSourceWizardSettings.SqlWizardSettings.DisableNewConnections = true;            
            ReplaceService(designer.DesignRibbonForm.DesignMdiController, GetType(IConnectionStorageService), connectionStorageService)
            ReplaceService(designer.DesignRibbonForm.DesignMdiController, GetType(IConnectionProviderService), connectionProviderService)

            AddHandler designer.DesignRibbonForm.DesignMdiController.DesignPanelLoaded, AddressOf DesignMdiControllerOnDesignPanelLoaded
            designer.ShowRibbonDesignerDialog()
        End Sub

        Private Sub DesignMdiControllerOnDesignPanelLoaded(ByVal sender As Object, ByVal e As DesignerLoadedEventArgs)
            ReplaceService(e.DesignerHost, GetType(IConnectionStorageService), connectionStorageService)
            ReplaceService(e.DesignerHost, GetType(IConnectionProviderService), New CustomConnectionProviderService())
        End Sub
        Private Sub ReplaceService(ByVal container As IServiceContainer, ByVal serviceType As Type, ByVal serviceInstance As Object)
            If container.GetService(serviceType) IsNot Nothing Then
                container.RemoveService(serviceType)
            End If
            container.AddService(serviceType, serviceInstance)
        End Sub
    End Class
End Namespace
