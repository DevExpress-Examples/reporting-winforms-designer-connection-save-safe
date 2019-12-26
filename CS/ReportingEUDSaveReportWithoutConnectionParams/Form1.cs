using DevExpress.DataAccess.Wizard.Services;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinEUDSaveReportWithoutConnectionParams {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }
        private void simpleButton1_Click(object sender, EventArgs e) {
            OpenReportDesigner();
        }
        CustomConnectionStorageService connectionStorageService;
        private void OpenReportDesigner() {
            CustomConnectionProviderService connectionProviderService = new CustomConnectionProviderService();
            connectionStorageService = new CustomConnectionStorageService(connectionProviderService) {
                FileName = "connections.xml",
                IncludeApplicationConnections = false,                
            };
            ReportDesignTool designer = new ReportDesignTool(new XtraReport());
            //designer.DesignRibbonForm.DesignMdiController.DataSourceWizardSettings.SqlWizardSettings.DisableNewConnections = true;            
            ReplaceService(designer.DesignRibbonForm.DesignMdiController, typeof(IConnectionStorageService), connectionStorageService);
            ReplaceService(designer.DesignRibbonForm.DesignMdiController, typeof(IConnectionProviderService), connectionProviderService);

            designer.DesignRibbonForm.DesignMdiController.DesignPanelLoaded += DesignMdiControllerOnDesignPanelLoaded;
            designer.ShowRibbonDesignerDialog();
        }

        private void DesignMdiControllerOnDesignPanelLoaded(object sender, DesignerLoadedEventArgs e) {
            ReplaceService(e.DesignerHost, typeof(IConnectionStorageService), connectionStorageService);
            ReplaceService(e.DesignerHost, typeof(IConnectionProviderService), new CustomConnectionProviderService());
        }
        private void ReplaceService(IServiceContainer container, Type serviceType, object serviceInstance) {
            if (container.GetService(serviceType) != null)
                container.RemoveService(serviceType);
            container.AddService(serviceType, serviceInstance);
        }
    }
}
