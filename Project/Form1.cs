using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.SystemUI;
using GeometryAndSR;
using ESRI.ArcGIS.DataSourcesGDB;
using System.IO;
using System.Data.OleDb;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Output;


namespace Project
{
    public partial class Form1 : Form
    {
        IFeatureLayer pFeatureLayer = null;
        public IFeatureLayer pGlobalFeatureLayer; //定义全局变量
        public ILayer player;
        private ControlsSynchronizer pMapControlsSynchronizer = null;
        private int DoQueryIndex = 0;
        private ESRI.ArcGIS.Geometry.IPointCollection pointCollection;
        private int DoBuffer = 0;
        private IMapControl3 m_mapControl = null;
        private string m_mapDocumentName = string.Empty;
        IMapDocument m_MapDocument = new MapDocumentClass();



        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            OpenMxdCommand pMxdCommand = new OpenMxdCommand();
            axToolbarControl1.AddItem(pMxdCommand, -1, 0, false, -1,
            esriCommandStyles.esriCommandStyleIconOnly);

            pMapControlsSynchronizer = new ControlsSynchronizer((IMapControl3)axMapControl1.Object,
                (IPageLayoutControl2)axPageLayoutControl1.Object);
            pMapControlsSynchronizer.BindControls(true);
            pMapControlsSynchronizer.AddFrameworkControl(axToolbarControl1.Object);
            pMapControlsSynchronizer.AddFrameworkControl(axTOCControl1.Object);
            //get the MapControl
            m_mapControl = (IMapControl3)axMapControl1.Object;

        }


        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenMXD = new OpenFileDialog();
            OpenMXD.Title = "打开地图";
            OpenMXD.InitialDirectory = @"C:\Users\Administrator\Desktop";
            OpenMXD.Filter = "Map Documents (*.mxd)|*.mxd";
            if (OpenMXD.ShowDialog() == DialogResult.OK)
            {
                string MxdPath = OpenMXD.FileName;
                axMapControl1.LoadMxFile(MxdPath);

            }

            OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\DATA.mdb");
            OleDbDataAdapter oda = new OleDbDataAdapter("select 所在路名,事故类型,驾驶员因素,车辆状况,路面状况,路段类型 ,天气条件,照明条件 from Point", conn);
            DataSet ds = new DataSet();
            oda.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0].DefaultView;
            conn.Close();
            DataTable dt = ds.Tables[0];
            RefreshLayer();

        }
        public string OpenMxd()
        {
            string MxdPath = "";
            OpenFileDialog OpenMXD = new OpenFileDialog();
            OpenMXD.Title = "打开地图";
            OpenMXD.InitialDirectory = "D:";
            OpenMXD.Filter = "Map Documents (*.mxd)|*.mxd";
            if (OpenMXD.ShowDialog() == DialogResult.OK)
            {
                MxdPath = OpenMXD.FileName;

            }
            return MxdPath;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            {
                string[] S = OpenShapeFile();
                axMapControl1.AddShapeFile(S[0], S[1]);
            }
        }
        public string[] OpenShapeFile()
        {
            string[] ShpFile = new string[2];
            OpenFileDialog OpenShpFile = new OpenFileDialog();
            OpenShpFile.Title = "打开Shape文件";
            OpenShpFile.InitialDirectory = "E:";
            OpenShpFile.Filter = "Shape文件(*.shp)|*.shp";
            if (OpenShpFile.ShowDialog() == DialogResult.OK)
            {
                string ShapPath = OpenShpFile.FileName;
                int Position = ShapPath.LastIndexOf("\\");
                string FilePath = ShapPath.Substring(0, Position);
                string ShpName = ShapPath.Substring(Position + 1);
                ShpFile[0] = FilePath;
                ShpFile[1] = ShpName;
            }
            return ShpFile;
        }

        private void axMapControl1_OnExtentUpdated(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnExtentUpdatedEvent e)
        {

            IEnvelope pEnvelope = (IEnvelope)e.newEnvelope;
            IGraphicsContainer pGraphicsContainer = axMapControl2.Map as IGraphicsContainer;
            IActiveView pActiveView = pGraphicsContainer as IActiveView;

            pGraphicsContainer.DeleteAllElements();
            IRectangleElement pRectangleEle = new RectangleElementClass();
            IElement pElement = pRectangleEle as IElement;
            pElement.Geometry = pEnvelope;
            IRgbColor pColor = new RgbColorClass();
            pColor.Red = 255;
            pColor.Green = 0;
            pColor.Blue = 0;
            pColor.Transparency = 255;
            ILineSymbol pOutline = new SimpleLineSymbolClass();
            pOutline.Width = 3;
            pOutline.Color = pColor;
            //设置颜色属性
            pColor = new RgbColorClass();
            pColor.Red = 255;
            pColor.Green = 0;
            pColor.Blue = 0;
            pColor.Transparency = 0;

            IFillSymbol pFillSymbol = new SimpleFillSymbolClass();
            pFillSymbol.Color = pColor;
            pFillSymbol.Outline = pOutline;
            IFillShapeElement pFillShapeEle = pElement as IFillShapeElement;
            pFillShapeEle.Symbol = pFillSymbol;
            pGraphicsContainer.AddElement((IElement)pFillShapeEle, 0);
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

            IPoint ll, Ur;
            ll = axMapControl1.Extent.LowerLeft;
            Ur = axMapControl1.Extent.LowerRight;
            toolStripStatusLabel1.Text = "(" + Convert.ToString(ll.X) + "," + Convert.ToString(ll.Y) + ")";

        }

        private void axMapControl1_OnMapReplaced(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMapReplacedEvent e)
        {
            if (axMapControl1.LayerCount > 0)
            {
                axMapControl2.Map = new MapClass();
                for (int i = 0; i <= axMapControl1.Map.LayerCount - 1; i++)
                {
                    axMapControl2.AddLayer(axMapControl1.get_Layer(i));
                }
                axMapControl2.Extent = axMapControl1.Extent;
                axMapControl2.Refresh();

            }

        }

        private void axMapControl2_OnMouseMove(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseMoveEvent e)
        {
            if (e.button == 1)
            {
                IPoint pPoint = new PointClass();
                pPoint.PutCoords(e.mapX, e.mapY);
                axMapControl1.CenterAt(pPoint);
                axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography,
                null, null);
            }
        }

        private void axMapControl2_OnMouseDown(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseDownEvent e)
        {
            if (axMapControl2.Map.LayerCount > 0)
            {
                if (e.button == 1)
                {
                    IPoint pPoint = new PointClass();
                    pPoint.PutCoords(e.mapX, e.mapY);
                    axMapControl1.CenterAt(pPoint);
                    axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
                }
                else if (e.button == 2)
                {
                    IEnvelope pEnv = axMapControl2.TrackRectangle();
                    axMapControl1.Extent = pEnv;
                    axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
                }
            }

        }

        private void axTOCControl1_OnMouseDown(object sender, ESRI.ArcGIS.Controls.ITOCControlEvents_OnMouseDownEvent e)
        {
            if (axMapControl1.LayerCount > 0)
            {
                esriTOCControlItem pItem = new esriTOCControlItem();
                pGlobalFeatureLayer = new FeatureLayerClass();
                IBasicMap pBasicMap = new MapClass();
                object pOther = new object();
                object pIndex = new object();
                axTOCControl1.HitTest(e.x, e.y, ref pItem, ref  pBasicMap, ref player, ref pOther, ref pIndex);
            }
            if (e.button == 2)
            {
                contextMenuStrip1.Show(axTOCControl1, e.x, e.y);
            }
            if (axMapControl1.LayerCount > 0)
            {
                esriTOCControlItem pItem = new esriTOCControlItem();
                pGlobalFeatureLayer = new FeatureLayerClass();
                IBasicMap pBasicMap = new MapClass();
                object pOther = new object();
                object pIndex = new object();
                axTOCControl1.HitTest(e.x, e.y, ref pItem, ref  pBasicMap, ref player, ref pOther, ref pIndex);
            }
            if (e.button == 2)
            {
                contextMenuStrip1.Show(axTOCControl1, e.x, e.y);
            }
        }

        private void 打开属性表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 Ft = new Form2(player as IFeatureLayer);
            Ft.Show();
        }


        private void 打开地图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ICommand pMxd = new ControlsOpenDocCommandClass();
            pMxd.OnCreate(axMapControl1.Object);
            pMxd.OnClick();
            RefreshLayer();
        }

        private void 添加数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ICommand pAddData = new ControlsAddDataCommandClass();
            pAddData.OnCreate(axMapControl1.Object);
            pAddData.OnClick();
        }

        private void 放大ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ICommand pZoomIn = new ControlsMapZoomInToolClass();
            pZoomIn.OnCreate(axMapControl1.Object);
            axMapControl1.CurrentTool = pZoomIn as ITool;
        }

        private void 缩小ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ICommand pZoomOut = new ControlsMapZoomOutToolClass();
            pZoomOut.OnCreate(axMapControl1.Object);
            axMapControl1.CurrentTool = pZoomOut as ITool;
        }
        private void 退出ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void 文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ICommand pMxd = new ControlsOpenDocCommandClass();
            pMxd.OnCreate(axMapControl1.Object);
            pMxd.OnClick();
        }

        private void 打印ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否要预览打印文档", "打印预览", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.printPreviewDialog1.UseAntiAlias = true;

                this.printPreviewDialog1.Document = this.printDocument1;

                printPreviewDialog1.ShowDialog();
            }
            else
            {


            }

        }



        private void 创建点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ICommand cmd = new CreatePointTool();
            cmd.OnCreate(axMapControl1.Object);
            axMapControl1.CurrentTool = cmd as ITool;
        }

        private void 创建线ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ICommand cmd = new CreatePolylineTool();
            cmd.OnCreate(axMapControl1.Object);
            axMapControl1.CurrentTool = cmd as ITool;
        }

        private void 创建多边形ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ICommand cmd = new CreatePolygonTool();
            cmd.OnCreate(axMapControl1.Object);
            axMapControl1.CurrentTool = cmd as ITool;
        }

        private void 创建圆ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ICommand cmd = new CreateCircleTool();
            cmd.OnCreate(axMapControl1.Object);
            axMapControl1.CurrentTool = cmd as ITool;
        }

        private void 创建矩形ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ICommand cmd = new CreateRectangleTool();
            cmd.OnCreate(axMapControl1.Object);
            axMapControl1.CurrentTool = cmd as ITool;
        }



        private void AddDataToMap(IWorkspace pWorkspace)
        {
            IEnumDataset pEnumDataset;
            pEnumDataset = pWorkspace.get_Datasets(esriDatasetType.esriDTFeatureClass);
            IDataset pDataset;
            pEnumDataset.Reset();
            pDataset = pEnumDataset.Next();
            //创建图层
            //IFeatureClass pFeatureclass = pDataset as IFeatureClass;
            //IFeatureLayer pLayer = new FeatureLayerClass();
            //pLayer.FeatureClass = pFeatureclass;
            //pLayer.Name = pDataset.Name;
            while (pDataset != null)
            {
                IFeatureClass pFeatureclass = pDataset as IFeatureClass;
                IFeatureLayer pLayer = new FeatureLayerClass();
                pLayer.FeatureClass = pFeatureclass;
                pLayer.Name = pDataset.Name;
                MessageBox.Show("添加要素类" + pDataset.Name + "!");
                axMapControl1.AddLayer(pLayer);
                pDataset = pEnumDataset.Next();
            }
        }




        private void 高亮显示(object sender, EventArgs e)
        {
            IMap pMap = axMapControl1.Map;
            IFeatureLayer pFeaturelayer = GetLayer(pMap, "Rivers_1") as IFeatureLayer;
            IFeatureSelection pFeatureSelection = pFeaturelayer as IFeatureSelection;
            IQueryFilter pQuery = new QueryFilterClass();
            pQuery.WhereClause = "FEATURE=" + "'stream'";
            pFeatureSelection.SelectFeatures(pQuery, esriSelectionResultEnum.esriSelectionResultNew, false);
            axMapControl1.ActiveView.Refresh();
        }


        private ILayer GetLayer(IMap pMap, string LayerName)
        {
            IEnumLayer pEnunLayer;
            pEnunLayer = pMap.get_Layers(null, false);
            pEnunLayer.Reset();
            ILayer pRetureLayer;
            pRetureLayer = pEnunLayer.Next();
            while (pRetureLayer != null)
            {
                if (pRetureLayer.Name == LayerName)
                {
                    break;
                }
                pRetureLayer = pEnunLayer.Next();
            }
            return pRetureLayer;
        }

        //RefreshLayer
        private void RefreshLayer()
        {
            layerComboBox.Items.Clear();
            for (int i = 0; i < axMapControl1.LayerCount; i++)
            {
                layerComboBox.Items.Add(axMapControl1.get_Layer(i).Name);
            }
            layerComboBox.Text = layerComboBox.Items[0].ToString();
        }

        private void Query_Click(object sender, EventArgs e)
        {

            IFeatureLayer pFeatureLayer = axMapControl1.get_Layer(layerComboBox.SelectedIndex) as ESRI.ArcGIS.Carto.IFeatureLayer;
            if (pFeatureLayer == null)
            {
                MessageBox.Show("选择图层不是Feature图层！");
                return;
            }
            IQueryFilter queryFilter = new ESRI.ArcGIS.Geodatabase.QueryFilterClass();
            queryFilter.WhereClause = queryFiltertextBox.Text;

            try
            {
                IFeatureCursor featureCursor = pFeatureLayer.Search(queryFilter, false);

                IFeature pFeature;
                pFeature = featureCursor.NextFeature();
                axMapControl1.FlashShape(pFeature.Shape);

            }
            catch (Exception pException)
            {
                MessageBox.Show(pException.Message);
            }
        }


        private void btRender_Click(object sender, EventArgs e)
        {

            createClassBreakRender("死亡事故");
            pictureBox1.Visible = true;
            //UniqueValueRender("督办等级");
        }





        private RgbColor GetRgbColor(int red, int green, int blue)
        {
            //颜色
            RgbColor rgbColor = new RgbColorClass();
            rgbColor.Red = red;
            rgbColor.Green = green;
            rgbColor.Blue = blue;
            return rgbColor;
        }

        //创建颜色带
        private IColorRamp CreateAlgorithmicColorRamp(int count)
        {
            //创建一个新AlgorithmicColorRampClass对象
            IAlgorithmicColorRamp algColorRamp = new AlgorithmicColorRampClass();
            IRgbColor fromColor = new RgbColorClass();
            IRgbColor toColor = new RgbColorClass();
            fromColor.Red = 255;
            fromColor.Green = 0;
            fromColor.Blue = 0;        
            toColor.Red = 255;
            toColor.Green = 255;
            toColor.Blue = 0;
            //设置AlgorithmicColorRampClass的起止颜色属性
            algColorRamp.ToColor = fromColor;
            algColorRamp.FromColor = toColor;
            //设置梯度类型
            algColorRamp.Algorithm = esriColorRampAlgorithm.esriCIELabAlgorithm;
            //设置颜色带颜色数量
            algColorRamp.Size = count;
            bool bture = true;
            algColorRamp.CreateRamp(out bture);
            return algColorRamp;
        }

        public void createClassBreakRender(string ClassField)
        {
            int classCount = 5;

            ILayer pLayer = axMapControl1.get_Layer(2);
            IFeatureLayer pFeatLayer = (IFeatureLayer)pLayer;
            IGeoFeatureLayer geoFeatureLayer = (IGeoFeatureLayer)pLayer;

            double[] classes;
            //classes = classifyGEN.ClassBreaks as double[];
            classes = new double[] { 0, 1, 2, 3,3 };

            IEnumColors enumColors = CreateAlgorithmicColorRamp(classes.Length).Colors;
            IColor color;

            IClassBreaksRenderer classBreaksRenderer = new ClassBreaksRendererClass();
            classBreaksRenderer.Field = ClassField;
            classBreaksRenderer.BreakCount = classCount;//分级数目
            classBreaksRenderer.SortClassesAscending = true;

            ISimpleLineSymbol simpleLineSymbol;
            for (int i = 0; i < classes.Length - 1; i++)
            {
                color = enumColors.Next();
                simpleLineSymbol = new SimpleLineSymbolClass();
                simpleLineSymbol.Color = color;
                simpleLineSymbol.Width = 4.0;
                simpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;

                classBreaksRenderer.set_Symbol(i, simpleLineSymbol as ISymbol);
                classBreaksRenderer.set_Break(i, classes[i]);
            }

            if (geoFeatureLayer != null)
            {
                geoFeatureLayer.Renderer = classBreaksRenderer as IFeatureRenderer;
                //刷新地图和TOOCotrol
                IActiveView pActiveView = axMapControl1.Map as IActiveView;
                pActiveView.Refresh();
                axTOCControl1.Update();
            }

        }
        public string OpenStr()
        {
            string MxdPath = "";
            OpenFileDialog OpenMXD = new OpenFileDialog();
            OpenMXD.Title = "选择保存路径";
            OpenMXD.Filter = "pdf文件(*.pdf)|*.pdf";
            OpenMXD.InitialDirectory = "D:";
            if (OpenMXD.ShowDialog() == DialogResult.OK)
            {
                MxdPath = OpenMXD.FileName;

            }
            return MxdPath;

        }


        private void pDFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportPDF();
            MessageBox.Show(@"生成成功！结果保存在 D:\ExportMAP.pdf 中");
        }


        private void ExportPDF()
        {
            IActiveView pActiveView;
            pActiveView = axPageLayoutControl1.ActiveView;
            IEnvelope pEnv;
            pEnv = pActiveView.Extent;
            IExport pExport;
            pExport = new ExportPDFClass();
            pExport.ExportFileName = @"D:\ExportMAP.pdf";
            pExport.Resolution = 30;
            tagRECT exportRECT;
            exportRECT.top = 0;
            exportRECT.left = 0;
            exportRECT.right = (int)pEnv.Width;
            exportRECT.bottom = (int)pEnv.Height;
            IEnvelope pPixelBoundsEnv;
            pPixelBoundsEnv = new EnvelopeClass();
            pPixelBoundsEnv.PutCoords(exportRECT.left, exportRECT.bottom,
            exportRECT.right, exportRECT.top);
            pExport.PixelBounds = pPixelBoundsEnv;
            int hDC;
            hDC = pExport.StartExporting();
            pActiveView.Output(hDC, (int)pExport.Resolution, ref exportRECT, null, null);
            pExport.FinishExporting();
            pExport.Cleanup();
        }


        private void axMapControl1_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {

            if (DoQueryIndex == 1)//点击查询
            {
                ESRI.ArcGIS.Carto.IFeatureLayer pFeatureLayer = axMapControl1.get_Layer(layerComboBox.SelectedIndex) as ESRI.ArcGIS.Carto.IFeatureLayer;
                ESRI.ArcGIS.Geometry.IPoint point = new ESRI.ArcGIS.Geometry.PointClass();
                point.PutCoords(e.mapX, e.mapY);

                ESRI.ArcGIS.Geodatabase.ISpatialFilter spatialFilter = new ESRI.ArcGIS.Geodatabase.SpatialFilterClass();
                spatialFilter.Geometry = point;
                spatialFilter.SpatialRel = ESRI.ArcGIS.Geodatabase.esriSpatialRelEnum.esriSpatialRelIntersects;
                ESRI.ArcGIS.Geodatabase.IFeatureCursor featureCursor = pFeatureLayer.Search(spatialFilter, false);

                ESRI.ArcGIS.Geodatabase.IFeature pFeature;
                while ((pFeature = featureCursor.NextFeature()) != null)
                {
                    axMapControl1.FlashShape(pFeature.Shape);
                }

            }
            else if (DoQueryIndex == 2)//面范围查询
            {
                ESRI.ArcGIS.Geometry.IPoint point = new ESRI.ArcGIS.Geometry.PointClass();
                point.PutCoords(e.mapX, e.mapY);

                pointCollection.AddPoints(1, ref point);

                if (pointCollection.PointCount > 1)
                {
                    DrawPolygon(pointCollection, axMapControl1);
                }
            }
        }

        private void DrawPolygon(ESRI.ArcGIS.Geometry.IPointCollection pPointCollection, ESRI.ArcGIS.Controls.AxMapControl axMapControl)
        {
            ESRI.ArcGIS.Geometry.IPolygon pPolygon;
            pPolygon = (ESRI.ArcGIS.Geometry.IPolygon)pPointCollection;
            axMapControl.DrawShape(pPolygon);
        }


        public string WsPath()
        {
            string WsFileName = "";
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Filter = "个人数据库(MDB)|*.mdb";
            DialogResult DialogR = OpenFile.ShowDialog();
            if (DialogR == DialogResult.Cancel)
            {
            }
            else
            {
                WsFileName = OpenFile.FileName;
            }
            return WsFileName;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Analy Analy = new Analy();
            Analy.Show();
        }


        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ICommand command = new ControlsSaveAsDocCommandClass();
            command.OnCreate(m_mapControl.Object);
            command.OnClick();

        }





    }
}


