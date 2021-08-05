# WinForms Report Designer - How to Save Only Connection Name with Reports and Exclude Connection Parameters

This example demonstrates how to customize the list of available connections in the WinForms End-User Report Designer and allow users to save only the connection name for newly created reports. 

This example implements the [IConnectionStorageService](https://docs.devexpress.com/CoreLibraries/DevExpress.DataAccess.Wizard.Services.IConnectionStorageService) to create a storage for connection strings available for the newly created reports. 

The users select a connection from a custom predefined list. Newly created report stores only the connection name and exclude sensitive information (data source name, username, password). For this, the [connection.StoreConnectionNameOnly](https://docs.devexpress.com/CoreLibraries/DevExpress.DataAccess.Wizard.Model.IDataConnection.StoreConnectionNameOnly) option is enabled in the [IConnectionStorageService.SaveConnection](https://docs.devexpress.com/CoreLibraries/DevExpress.DataAccess.Wizard.Services.IConnectionStorageService.SaveConnection(System.String-DevExpress.DataAccess.Wizard.Model.IDataConnection-System.Boolean)) method.

In code you can use the `IConnectionStorageService.IncludeApplicationConnections` option to specify whether the Data Source Wizard includes connections contained in the application configuration file.

Use the [XRDesignMdiController.DataSourceWizardSettings.SqlWizardSettings.DisableNewConnections](https://docs.devexpress.com/CoreLibraries/DevExpress.DataAccess.UI.Wizard.SqlWizardSettings.DisableNewConnections) option to specify whether the users are allowed to create new connections. 

Create a service that implements the [IConnectionProviderService](https://docs.devexpress.com/CoreLibraries/DevExpress.DataAccess.Wizard.Services.IConnectionProviderService) interface. The service allows users to open a saved report with a data source whose connection is specified by a name, and restore the connection to view, print, and export the report.

