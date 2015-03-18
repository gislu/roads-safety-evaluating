using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;

namespace GeometryAndSR
{
    /// <summary>
    /// Summary description for ModifyVertex.
    /// </summary>

    public sealed class Move1VertexOfGeometry : BaseTool
    {
        #region COM Registration Function(s)
        [ComRegisterFunction()]
        [ComVisible(false)]
        static void RegisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryRegistration(registerType);

            //
            // TODO: Add any COM registration code here
            //
        }

        [ComUnregisterFunction()]
        [ComVisible(false)]
        static void UnregisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryUnregistration(registerType);

            //
            // TODO: Add any COM unregistration code here
            //
        }

        #region ArcGIS Component Category Registrar generated code
        /// <summary>
        /// Required method for ArcGIS Component Category registration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryRegistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommands.Register(regKey);
            ControlsCommands.Register(regKey);
        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommands.Unregister(regKey);
            ControlsCommands.Unregister(regKey);
        }

        #endregion
        #endregion
        
        IHookHelper m_hookHelper = null;
        IActiveView m_activeView = null;
        IMap m_map = null; 
        IEngineEditProperties m_engineEditor = null;

        IPoint m_activePoint = null;
        IFeature m_selectedFeature = null;

        public Move1VertexOfGeometry()
        {
            base.m_category = "GeometryAndSR";
            base.m_caption = "移动要素顶点";
            base.m_message = "移动要素顶点";
            base.m_toolTip = "移动要素顶点";
            base.m_name = "Modify1Vertex";   
            m_engineEditor = new EngineEditorClass();
        }

        #region Overriden Class Methods
        public override void OnCreate(object hook)
        {
            try
            {
                m_hookHelper = new HookHelperClass();
                m_hookHelper.Hook = hook;
                if (m_hookHelper.ActiveView == null)
                {
                    m_hookHelper = null;
                }
            }
            catch
            {
                m_hookHelper = null;
            }

            if (m_hookHelper == null)
                base.m_enabled = false;
            else
                base.m_enabled = true;           

            m_activePoint = new PointClass();
        }

        public override void OnClick()
        { 
            ILayer layer = m_engineEditor.TargetLayer;
            if (layer == null)
            {
                MessageBox.Show("请先启动编辑！！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        public override bool Deactivate()
        {
            //Release object references.
            m_selectedFeature = null;
            m_activePoint = null;

            return true;
        }

        public override void OnKeyDown(int keyCode, int Shift)
        {
            // If the Escape key is used, throw away the calculated point.
            if (keyCode == (int)Keys.Escape)
            {                
                m_map.ClearSelection();
                m_activeView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, m_activeView.Extent);
            }
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            if (Button != (int)Keys.LButton) return;
            ILayer layer = m_engineEditor.TargetLayer;
            if (layer == null)
            {
                MessageBox.Show("请先启动编辑！！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            m_activeView = m_hookHelper.ActiveView;
            m_map = m_hookHelper.FocusMap; 
            if (m_map == null || m_activeView == null) return;

            m_activePoint = m_activeView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
            GetSelectedFeature();
            if (m_selectedFeature == null)
            {
                MessageBox.Show("请选择要素（在要移动的顶点处点选）！！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            IGeometryCollection geometry = m_selectedFeature.Shape as IGeometryCollection;
            IPointCollection4 polylinePoints= modify1VertexOfAPolyline(geometry, 1, 5, 5);
            if (polylinePoints!=null) UpdateFeature(m_selectedFeature, polylinePoints);
            m_activeView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, m_activeView.Extent);
           
        }        
        #endregion

        private void UpdateFeature(IFeature selectedFeature, IPointCollection4 polylinePoints)
        {
            IPointCollection4 geometry;
            esriGeometryType geometryType=selectedFeature.Shape.GeometryType;
            switch (geometryType)
            {
                case esriGeometryType.esriGeometryMultipoint:
                    geometry = new MultipointClass();
                    break;
                case esriGeometryType.esriGeometryPolyline:
                    geometry = new PolylineClass();
                    break;
                case esriGeometryType.esriGeometryPolygon:
                    geometry = new PolygonClass();
                    break;
                default:
                    geometry = null;
                    break;
            }
            if (geometry == null) return;           
            geometry.AddPointCollection(polylinePoints);
            IFeatureClass featureClass = selectedFeature.Class as IFeatureClass;
            IDataset dataset = featureClass as IDataset;
            IWorkspaceEdit workspaceEdit = dataset.Workspace as IWorkspaceEdit;
            if (!(workspaceEdit.IsBeingEdited())) return;

            try
            {
                workspaceEdit.StartEditOperation();
                selectedFeature.Shape = geometry as IGeometry;
                selectedFeature.Store();
                workspaceEdit.StopEditOperation();
            }
            catch (Exception ex)
            {
                workspaceEdit.AbortEditOperation();
                MessageBox.Show("移动要素顶点失败！！" + ex.Message, "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        public IPointCollection4 modify1VertexOfAPolyline(IGeometryCollection geo, Double searchRadius, Double offsetX, Double offsetY)
        {
            IPoint queryPoint = m_activePoint;
            IPoint hitPoint = new PointClass();
            Double hitDistance = 0; Int32 hitPartIndex = 0;
            Int32 hitSegmentIndex = 0;
            Boolean rightSide = false;
            IHitTest hitTest = (IHitTest)geo;
            Boolean foundGeometry = hitTest.HitTest(queryPoint, searchRadius, esriGeometryHitPartType.esriGeometryPartVertex, hitPoint, ref hitDistance, ref hitPartIndex, ref hitSegmentIndex, ref  rightSide);
            if (foundGeometry == true)
            {
                IGeometry geometry = geo.get_Geometry(hitPartIndex);
                IPointCollection4 pointCollection = (IPointCollection4)geometry;
                IPoint transformPoint = pointCollection.get_Point(hitSegmentIndex);
                ITransform2D transform2D = (ITransform2D)transformPoint;
                transform2D.Move(offsetX, offsetY);
                pointCollection.UpdatePoint(hitSegmentIndex, transformPoint);
                return pointCollection;
            }
            return null;
        }


        private void GetSelectedFeature()
        {
            if (m_activePoint == null) return;
            IPoint mousePoint = m_activePoint;
            m_selectedFeature = SelctFeatureBasedMousePoint(mousePoint);            
        }

      

        private IFeature SelctFeatureBasedMousePoint(IPoint pPoint)
        {
            //对点对象做缓冲区运算
            ITopologicalOperator pTopo = pPoint as ITopologicalOperator;
            IGeometry pBuffer = pTopo.Buffer(0.5);
            IGeometry pGeometry = pBuffer.Envelope;
            SetAllPolylinePolygonLayersSelectable();
            ISelectionEnvironment selEnvironment = new SelectionEnvironmentClass();
            selEnvironment.CombinationMethod = esriSelectionResultEnum.esriSelectionResultNew;
            m_map.SelectByShape(pGeometry, selEnvironment, true);
            IEnumFeature SelectedFeatures = m_map.FeatureSelection as IEnumFeature;
            SelectedFeatures.Reset();
            IFeature extendFeature = SelectedFeatures.Next();

            SetAllLayersSelectable();

            m_activeView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, m_activeView.Extent);

            if (extendFeature!=null && extendFeature.Shape.GeometryType == esriGeometryType.esriGeometryPolyline) return extendFeature;
            return null;
        }

        private void SetAllPolylinePolygonLayersSelectable()
        {
            IEnumLayer layers = GetLayers();
            layers.Reset();
            ILayer layer = null;
            while ((layer = layers.Next()) != null)
            {
                IFeatureLayer featureLayer = layer as IFeatureLayer;
                if (featureLayer == null) continue;
                if (featureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline
                    || featureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon
                    || featureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryMultipoint)
                    featureLayer.Selectable = true;
                else
                    featureLayer.Selectable = false;
            }
        }

        private void SetAllLayersSelectable()
        {
            IEnumLayer layers = GetLayers();
            layers.Reset();
            ILayer layer = null;
            while ((layer = layers.Next()) != null)
            {
                IFeatureLayer featureLayer = layer as IFeatureLayer;
                if (featureLayer == null) continue;
                featureLayer.Selectable = true;
            }
        }

        #region "GetLayers"
        private IEnumLayer GetLayers()
        {
            UID uid = new UIDClass();
            //uid.Value = "{40A9E885-5533-11d0-98BE-00805F7CED21}";// IFeatureLayer
            uid.Value = "{E156D7E5-22AF-11D3-9F99-00C04F6BC78E}";  // IGeoFeatureLayer
            //uid.Value = "{6CA416B1-E160-11D2-9F4E-00C04F6BC78E}";  // IDataLayer
            if (m_map.LayerCount != 0)
            {
                IEnumLayer layers = m_map.get_Layers(uid, true);
                return layers;
            }
            return null;
        }
        #endregion

        private void FlashGeometry(IGeometry geometry, int flashCount, int interval)
        {
            IScreenDisplay display = m_activeView.ScreenDisplay;
            ISymbol symbol = CreateSimpleSymbol(geometry.GeometryType);
            display.StartDrawing(0, (short)esriScreenCache.esriNoScreenCache);
            display.SetSymbol(symbol);

            for (int i = 0; i < flashCount; i++)
            {
                switch (geometry.GeometryType)
                {
                    case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint:
                        display.DrawPoint(geometry);
                        break;
                    case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryMultipoint:
                        display.DrawMultipoint(geometry);
                        break;
                    case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline:
                        display.DrawPolyline(geometry);
                        break;
                    case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon:
                        display.DrawPolygon(geometry);
                        break;
                    default:
                        break;
                }
                System.Threading.Thread.Sleep(interval);
            }
            display.FinishDrawing();
        }

        private ISymbol CreateSimpleSymbol(esriGeometryType geometryType)
        {
            ISymbol symbol = null;
            switch (geometryType)
            {
                case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint:
                case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryMultipoint:
                    ISimpleMarkerSymbol markerSymbol = new SimpleMarkerSymbolClass();
                    markerSymbol.Color = getRGB(255, 128, 128);
                    markerSymbol.Size = 2;
                    symbol = markerSymbol as ISymbol;
                    break;

                case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline:
                case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPath:
                    ISimpleLineSymbol lineSymbol = new SimpleLineSymbolClass();
                    lineSymbol.Color = getRGB(255, 128, 128);
                    lineSymbol.Width = 4;
                    symbol = lineSymbol as ISymbol;
                    break;

                case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon:
                case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryRing:
                    ISimpleFillSymbol fillSymbol = new SimpleFillSymbolClass();
                    fillSymbol.Color = getRGB(255, 128, 128);
                    symbol = fillSymbol as ISymbol;
                    break;
                default:
                    break;
            }
            symbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;

            return symbol;
        }

        public IColor getRGB(int yourRed, int yourGreen, int yourBlue)
        {
            IRgbColor pRGB = new RgbColorClass();
            pRGB.Red = yourRed;
            pRGB.Green = yourGreen;
            pRGB.Blue = yourBlue;
            pRGB.UseWindowsDithering = true;
            return pRGB;
        }
        
    }

}