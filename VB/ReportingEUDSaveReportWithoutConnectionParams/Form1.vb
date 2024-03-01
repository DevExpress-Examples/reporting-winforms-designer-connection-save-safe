Imports DevExpress.DataAccess.Wizard.Services
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraReports.UserDesigner
Imports System
Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Drawing
Imports System.Windows.Forms

Namespace WinEUDSaveReportWithoutConnectionParams

    Public Partial Class Form1
        Inherits Form

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub simpleButton1_Click(ByVal sender As Object, ByVal e As EventArgs)
            OpenReportDesigner()
        End Sub

        Private connectionStorageService As CustomConnectionStorageService

        Private Sub OpenReportDesigner()
            Dim connectionProviderService As CustomConnectionProviderService = New CustomConnectionProviderService()
            connectionStorageService = New CustomConnectionStorageService(connectionProviderService) With {.FileName = "connections.xml", .IncludeApplicationConnections = False}
            Dim designer As ReportDesignTool = New ReportDesignTool(New XtraReport())
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
            If container.GetService(serviceType) IsNot Nothing Then container.RemoveService(serviceType)
            container.AddService(serviceType, serviceInstance)
        End Sub
    End Class
End Namespace
