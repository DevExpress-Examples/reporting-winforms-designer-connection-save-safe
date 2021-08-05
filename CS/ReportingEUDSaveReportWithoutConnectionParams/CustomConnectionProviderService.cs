using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Sql;
using DevExpress.DataAccess.Wizard.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
namespace WinEUDSaveReportWithoutConnectionParams {
    public class CustomConnectionProviderService : IConnectionProviderService {
        public SqlDataConnection LoadConnection(string connectionName) {
            SqlDataConnection connection = GetConnectionFromXml(connectionName);
            var result = connection != null ? connection : GetFallbackConnection(connectionName);
            return result;
        }
        private SqlDataConnection GetFallbackConnection(string connectionName) {
            throw new Exception("Connection was not found!");
            //or provide a custom fallback connection 
            //return new SqlDataConnection(connectionName, new MsSqlConnectionParameters("localhost", "dataBaseName", "userName", "password", MsSqlAuthorizationType.Windows));
        }

        public string FileName { get; set; } = "connections.xml";

        protected SqlDataConnection GetConnectionFromXml(string connectionName) {
            SqlDataConnection result = GetConnectionsFromXml(x => x == connectionName).FirstOrDefault();
            return result;
        }
        internal IEnumerable<SqlDataConnection> GetConnectionsFromXml() {
            return GetConnectionsFromXml(x => true);
        }
        IEnumerable<SqlDataConnection> GetConnectionsFromXml(Predicate<string> action) {
            var result = new List<SqlDataConnection>();
            try {
                XmlDocument doc = new XmlDocument();
                doc.Load(FileName);
                foreach (XmlNode node in doc.SelectNodes("Connections/Connection[Name][ConnectionString]")) {
                    string connectionName = node["Name"].InnerText;
                    if (action(connectionName)) {
                        CustomSqlDataConnection connection = new CustomSqlDataConnection(connectionName,
                           new CustomStringConnectionParameters(node["ConnectionString"].InnerText));
                        connection.StoreConnectionNameOnly = true;
                        result.Add(connection);
                    }
                }

                return result;
            } catch (Exception ex) {
                MessageBox.Show(string.Format("Cannot get connections from '{0}' because of exception:{1}{1}{2}",
                    FileName, Environment.NewLine, ex.Message));
                return new SqlDataConnection[0];
            }
        }
    }
}

