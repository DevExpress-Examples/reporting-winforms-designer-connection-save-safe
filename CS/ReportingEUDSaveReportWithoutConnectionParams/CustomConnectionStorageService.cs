using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Sql;
using DevExpress.DataAccess.Wizard.Model;
using DevExpress.DataAccess.Wizard.Native;
using DevExpress.DataAccess.Wizard.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace WinEUDSaveReportWithoutConnectionParams {
    class CustomConnectionStorageService : IConnectionStorageService {
        public bool CanSaveConnection {
            get { return true; }
        }
        
        bool IConnectionStorageService.Contains(string connectionName) {            
            return IncludeApplicationConnections ?
                DefaultStorage.Contains(connectionName) || ConnectionProviderService.GetConnectionsFromXml().Any(c => string.Equals(c.Name, connectionName)) :
                ConnectionProviderService.GetConnectionsFromXml().Any(c => string.Equals(c.Name, connectionName));
        }
        IEnumerable<DevExpress.DataAccess.Sql.SqlDataConnection> IConnectionStorageService.GetConnections() {           
            return IncludeApplicationConnections 
                ? DefaultStorage.GetConnections().Union(ConnectionProviderService.GetConnectionsFromXml()) 
                : ConnectionProviderService.GetConnectionsFromXml();
        }
        void CreateDefaultConnection() {
            if (!File.Exists(FileName)) {
                try {
                    XmlDocument doc = new XmlDocument();
                    XmlElement root = doc.CreateElement(xmlRootName);
                    doc.AppendChild(root);
                    doc.Save(FileName);
                    SqlDataConnection defaultConnection = new CustomSqlDataConnection("Default Connection", new MsSqlConnectionParameters("localhost", "NORTHWND", "", "", MsSqlAuthorizationType.Windows));
                    SaveConnection(defaultConnection.Name, defaultConnection, true);
                } catch (Exception ex) {
                    MessageBox.Show(string.Format("Cannot create '{0}' file because of exception:{1}{1}{2}", FileName,
                        Environment.NewLine, ex.Message));                    
                }
            }
        }
        public void SaveConnection(string connectionName, IDataConnection connection, bool saveCredentials) {
            connection.StoreConnectionNameOnly = true;
            try {
                XmlDocument doc = new XmlDocument();
                XmlElement root = null;
                if (File.Exists(FileName)) {
                    doc.Load(FileName);
                    root = doc.DocumentElement;
                    if (root != null) {
                        if (root.Name != xmlRootName) {
                            MessageBox.Show(string.Format("Document element is '{0}', '{1}' expected",
                                root.Name, xmlRootName));
                            return;
                        }
                        if (root.SelectSingleNode(string.Format("Connection[Name = '{0}']", connectionName)) != null)
                            return;
                    }
                }
                if (root == null) {
                    root = doc.CreateElement(xmlRootName);
                    doc.AppendChild(root);
                }

                XmlElement nameElement = doc.CreateElement("Name");
                nameElement.AppendChild(doc.CreateTextNode(connectionName));

                XmlElement connectionStringElement = doc.CreateElement("ConnectionString");
                connectionStringElement.AppendChild(doc.CreateTextNode(connection.CreateConnectionString(!saveCredentials)));

                XmlElement connectionElement = doc.CreateElement("Connection");
                connectionElement.AppendChild(nameElement);
                connectionElement.AppendChild(connectionStringElement);

                root.AppendChild(connectionElement);
                doc.Save(FileName);
            } catch (Exception ex) {
                MessageBox.Show(string.Format("Cannot save connection to '{0}' because of exception:{1}{1}{2}", FileName,
                    Environment.NewLine, ex.Message));
            }
        }
        const string xmlRootName = "Connections";

        public CustomConnectionStorageService(CustomConnectionProviderService connectionProviderService) {
            CreateDefaultConnection();
            ConnectionProviderService = connectionProviderService;
        }
        public CustomConnectionProviderService ConnectionProviderService { get; }
        public string FileName { get; set; } = "connections.xml";
        public bool IncludeApplicationConnections { get; set; } = false;
        protected ConnectionStorageService DefaultStorage { get; } = new ConnectionStorageService();
    }
}

