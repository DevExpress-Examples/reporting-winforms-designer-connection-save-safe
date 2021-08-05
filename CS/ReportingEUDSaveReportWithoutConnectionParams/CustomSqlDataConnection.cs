using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Native;
using DevExpress.DataAccess.Sql;

namespace WinEUDSaveReportWithoutConnectionParams {
    class CustomSqlDataConnection : SqlDataConnection, INamedItem {
        public CustomSqlDataConnection(string name, DataConnectionParametersBase connectionParameters)
            : base(name, connectionParameters) {
        }

        string INamedItem.Name {
            get {
                return Name + " (Custom)";
            }
            set {
                Name = value;
            }
        }
    }
}

