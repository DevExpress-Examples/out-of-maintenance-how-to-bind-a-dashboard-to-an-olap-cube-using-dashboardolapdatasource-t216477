using DevExpress.XtraBars.Ribbon;
using DevExpress.DashboardCommon;
using DevExpress.DataAccess.ConnectionParameters;

namespace Dashboard_OlapDataProvider {
    public partial class Form1 : RibbonForm {
        public Form1() {
            InitializeComponent();

            #region #OLAPDataSource
            OlapConnectionParameters olapParams = new OlapConnectionParameters();
            olapParams.ConnectionString = @"provider=MSOLAP;
                                  data source=http://demos.devexpress.com/Services/OLAP/msmdpump.dll;
                                  initial catalog=Adventure Works DW Standard Edition;
                                  cube name=Adventure Works;";

            DashboardOlapDataSource olapDataSource = new DashboardOlapDataSource(olapParams);
            olapDataSource.Fill();

            dashboardDesigner1.Dashboard.DataSources.Add(olapDataSource);
            #endregion #OLAPDataSource

            InitializeDashboardItems();
        }

        void InitializeDashboardItems() {
            IDashboardDataSource olapDataSource = dashboardDesigner1.Dashboard.DataSources[0];

            PivotDashboardItem pivot = new PivotDashboardItem(); 
            pivot.DataSource = olapDataSource;
            pivot.Values.Add(new Measure("[Measures].[Sales Amount]"));
            pivot.Columns.Add(new Dimension("[Sales Channel].[Sales Channel].[Sales Channel]"));
            pivot.Rows.AddRange(
                new Dimension("[Sales Territory].[Sales Territory].[Group]", 1), 
                new Dimension("[Sales Territory].[Sales Territory].[Country]", 1), 
                new Dimension("[Sales Territory].[Sales Territory].[Region]", 1));
            pivot.AutoExpandRowGroups = true;

            ChartDashboardItem chart = new ChartDashboardItem(); 
            chart.DataSource = olapDataSource;
            chart.Arguments.Add(new Dimension("[Sales Territory].[Sales Territory].[Country]"));
            chart.Panes.Add(new ChartPane());
            SimpleSeries salesAmountSeries = new SimpleSeries(SimpleSeriesType.Bar);
            salesAmountSeries.Value = new Measure("[Measures].[Sales Amount]");
            chart.Panes[0].Series.Add(salesAmountSeries);

            dashboardDesigner1.Dashboard.Items.AddRange(pivot, chart);
        }
    }
}
