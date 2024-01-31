<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/230255468/23.2.3%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T848384)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
# Reporting for WinForms - How to Enable the Report Designer to Save Only Connection Name with Reports and Exclude Connection Parameters

This example shows how to customize the list of available connections in the WinForms end-user report designer to allow users to save only the connection name for newly created reports.

This example implements the [IConnectionStorageService](https://docs.devexpress.com/CoreLibraries/DevExpress.DataAccess.Wizard.Services.IConnectionStorageService) to create a storage for connection strings available for the newly created reports. 

A user can select a connection from a custom predefined list. Newly created report stores only the connection name and exclude sensitive information (data source name, username, password). For this, the [connection.StoreConnectionNameOnly](https://docs.devexpress.com/CoreLibraries/DevExpress.DataAccess.Wizard.Model.IDataConnection.StoreConnectionNameOnly) option is enabled in the [IConnectionStorageService.SaveConnection](https://docs.devexpress.com/CoreLibraries/DevExpress.DataAccess.Wizard.Services.IConnectionStorageService.SaveConnection(System.String-DevExpress.DataAccess.Wizard.Model.IDataConnection-System.Boolean)) method.

In the code, you can use the `IConnectionStorageService.IncludeApplicationConnections' option to specify whether the Data Source Wizard should include the connections that are contained in the application configuration file.

Use the [XRDesignMdiController.DataSourceWizardSettings.SqlWizardSettings.DisableNewConnections](https://docs.devexpress.com/CoreLibraries/DevExpress.DataAccess.UI.Wizard.SqlWizardSettings.DisableNewConnections) option to specify whether the users are allowed to create new connections. 

Create a service that implements the [IConnectionProviderService](https://docs.devexpress.com/CoreLibraries/DevExpress.DataAccess.Wizard.Services.IConnectionProviderService) interface. The service allows users to open a saved report with a connection specified by name and restore the connection to view, print, and export the report.

## Files to Review

* [Form1.cs](CS\ReportingEUDSaveReportWithoutConnectionParams\Form1.cs) (VB: [Form1.vb](VB\ReportingEUDSaveReportWithoutConnectionParams\Form1.vb))
* [CustomSqlDataConnection.cs](CS\ReportingEUDSaveReportWithoutConnectionParams\CustomSqlDataConnection.cs) (VB: [CustomSqlDataConnection.vb](VB\ReportingEUDSaveReportWithoutConnectionParams\CustomSqlDataConnection.vb))
* [CustomConnectionProviderService.cs](CS\ReportingEUDSaveReportWithoutConnectionParams\CustomConnectionProviderService.cs) (VB: [CustomConnectionProviderService.vb](VB\ReportingEUDSaveReportWithoutConnectionParams\CustomConnectionProviderService.vb))
* [CustomConnectionStorageService.cs](CS\ReportingEUDSaveReportWithoutConnectionParams\CustomConnectionStorageService.cs) (VB: [CustomConnectionStorageService.vb](VB\ReportingEUDSaveReportWithoutConnectionParams\CustomConnectionStorageService.vb))

## Documentation

* [Customize Data Connections in the Data Source Wizard (WinForms)](https://docs.devexpress.com/XtraReports/403352/desktop-reporting/winforms-reporting/end-user-report-designer-for-winforms/api-and-customization/customize-data-connections)
* [Data Source Wizard - Connect to a Database](https://docs.devexpress.com/XtraReports/4241/visual-studio-report-designer/data-source-wizard/connect-to-a-database)

## More Examples

* [Reporting for WinForms - How to Store Connections Available in the Data Source Wizard](https://github.com/DevExpress-Examples/reporting-winforms-wizard-data-connections)
