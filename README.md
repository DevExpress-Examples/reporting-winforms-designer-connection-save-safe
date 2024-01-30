<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/230255468/19.2.2%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T848384)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
## Report Designer - How to save reports with the connection name only and omit connection parameters

This example demonstrates how to customize the WinForms End-User Report Designer and allow end users to save only the name of a connection for reports created in the designer. 

Implement [IConnectionStorageService](https://docs.devexpress.com/CoreLibraries/DevExpress.DataAccess.Wizard.Services.IConnectionStorageService) to provide a storage for connection strings available for newly created reports. 
End users will chose a connection from a custom predefined list. 
Newly created reports will store only the name of a connection and omit accompanying connection string information (e.g. data source, user id, password). 
For this, the [connection.StoreConnectionNameOnly](https://docs.devexpress.com/CoreLibraries/DevExpress.DataAccess.Wizard.Model.IDataConnection.StoreConnectionNameOnly) option must be enabled in the [IConnectionStorageService.SaveConnection](https://docs.devexpress.com/CoreLibraries/DevExpress.DataAccess.Wizard.Services.IConnectionStorageService.SaveConnection(System.String-DevExpress.DataAccess.Wizard.Model.IDataConnection-System.Boolean)) method.

Specify whether or not to include connections from the application configuration file using the IConnectionStorageService.IncludeApplicationConnections option.
Specify whether or not to allow end users to create new connections by using the [XRDesignMdiController.DataSourceWizardSettings.SqlWizardSettings.DisableNewConnections](https://docs.devexpress.com/CoreLibraries/DevExpress.DataAccess.UI.Wizard.SqlWizardSettings.DisableNewConnections) option. 

To allow existing reports (the ones that end users open in the designer) to restore their connection by the name, implement the [IConnectionProviderService](https://docs.devexpress.com/CoreLibraries/DevExpress.DataAccess.Wizard.Services.IConnectionProviderService) interface.
