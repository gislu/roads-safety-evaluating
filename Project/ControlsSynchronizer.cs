using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using Project;

namespace Project
{
    public class ControlsSynchronizer
    {
        #region class members
        //<summary>
        /// 在构造函数中传入地图控件和布局控件
        /// </summary>
        /// <param name="_MapControl"></param>
        /// <param name="_PageLayoutControl"></param>
        private IMapControl3 pMapControl = null;
        private IPageLayoutControl2 pPageLayoutControl = null;
        private ITool pMapActiveTool = null;
        private ITool pPageLayoutActiveTool = null;
        private bool pIsMapControlactive = true;
        private ArrayList pFrameworkControls = null;
        #endregion

        #region constructor
        public ControlsSynchronizer()
        {
            //initialize the underlying ArrayList
            pFrameworkControls = new ArrayList();
        }

        /// <summary>
        //        /// Gets or sets the PageLayoutControl
        //        /// </summary>
        public IPageLayoutControl2 PageLayoutControl
        {
            get { return pPageLayoutControl; }
            set { pPageLayoutControl = value; }
        }


        /// 在构造函数中传入地图控件和布局控件
        /// </summary> 
        /// <param name="_MapControl"></param> 
        /// <param name="_PageLayoutControl"></param> 
        public ControlsSynchronizer(IMapControl3 _MapControl, IPageLayoutControl2 _PageLayoutControl) :
            this()
        {
            //assign the class members 
            pMapControl = _MapControl;
            pPageLayoutControl = _PageLayoutControl;
        }
        #endregion
        #region Methods
        /// <summary> 
        /// 激活地图控件并销毁布局控件
        /// </summary> 
        public void ActivateMap()
        {
            try
            {
                if (pPageLayoutControl == null || pMapControl == null)
                    throw new Exception("ControlsSynchronizer::ActivateMap:\r\nEither  MapControl  or PageLayoutControl are not initialized!");
                if (pPageLayoutControl.CurrentTool != null) pPageLayoutActiveTool =
             pPageLayoutControl.CurrentTool;
                pPageLayoutControl.ActiveView.Deactivate();
                pMapControl.ActiveView.Activate(pMapControl.hWnd);
                if (pMapActiveTool != null) pMapControl.CurrentTool = pMapActiveTool;
                pIsMapControlactive = true;
                this.SetBuddies(pMapControl.Object);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("ControlsSynchronizer::ActivateMap:\r\n{0}",
                ex.Message));
            }
        }

        /// <summary> 
        /// 激活布局控件并销毁地图控件
        /// </summary> 
        public void ActivatePageLayout()
        {
            try
            {
                if (pPageLayoutControl == null || pMapControl == null)
                    throw new Exception("ControlsSynchronizer::ActivatePageLayout:\r\nEither MapControl or PageLayoutControl are not initialized!");
                if (pMapControl.CurrentTool != null) pMapActiveTool = pMapControl.CurrentTool;
                pMapControl.ActiveView.Deactivate();
                pPageLayoutControl.ActiveView.Activate(pPageLayoutControl.hWnd);
                if (pPageLayoutActiveTool != null) pPageLayoutControl.CurrentTool =
             pPageLayoutActiveTool;
                pIsMapControlactive = false;
                this.SetBuddies(pPageLayoutControl.Object);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("ControlsSynchronizer::ActivatePageLayout:\r\n{0}",
                ex.Message));
            }
        }
        /// <summary> 
        /// 当激活的控件发生变化时，IToolbarControl控件和ITOCControl控件的伙伴控件也应发生变化
        /// </summary> 
        /// <param name="buddy">the active control</param> 
        /// 
        private void SetBuddies(object _buddy)
        {
            try
            {
                if (_buddy == null)
                    throw new Exception("ControlsSynchronizer::SetBuddies:\r\nTarget Buddy Control is not initialized!");

                foreach (object obj in pFrameworkControls)
                {
                    if (obj is IToolbarControl)
                    {
                        ((IToolbarControl)obj).SetBuddyControl(_buddy);
                    }
                    else if (obj is ITOCControl)
                    {
                        ((ITOCControl)obj).SetBuddyControl(_buddy);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("ControlsSynchronizer::SetBuddies:\r\n{0}", ex.Message));
            }
        }

        /// <summary> 
        /// 如果地图发生了变化，那么地图控件和布局控件的地图也应发生变化
        /// </summary> 
        /// <param name="_NewMap"></param> 
        public void ReplaceMap(IMap _NewMap)
        {
            if (_NewMap == null)
                throw new Exception("ControlsSynchronizer::ReplaceMap:\r\nNew map for replacement is not initialized!");
            if (pPageLayoutControl == null || pMapControl == null)
                throw new Exception("ControlsSynchronizer::ReplaceMap:\r\nEither  MapControl  or PageLayoutControl are not initialized!");
            IMaps pMaps = new Maps();
            pMaps.Add(_NewMap);
            bool bIsMapActive = pIsMapControlactive;
            this.ActivatePageLayout();
            pPageLayoutControl.PageLayout.ReplaceMaps(pMaps);
            pMapControl.Map = _NewMap;
            //reset the active tools 
            pPageLayoutActiveTool = null;
            pMapActiveTool = null;
            if (bIsMapActive)
            {
                this.ActivateMap();
                pMapControl.ActiveView.Refresh();
            }
            else
            {
                this.ActivatePageLayout();
                pPageLayoutControl.ActiveView.Refresh();
            }
        }
        /// <summary> 
        /// 当运行应用程序的时候，即便没有加载地图，则创建一个空的地图，让这两个控件和这个地图绑定在一起，这样就能保持一致
        /// </summary> 
        ///  <param  name="activateMapFirst">true  if  the  MapControl  supposed  to  be  activated first</param> 
        public void BindControls(bool _ActivateMapFirst)
        {
            if (pPageLayoutControl == null || pMapControl == null)
                throw new Exception("ControlsSynchronizer::BindControls:\r\nEither  MapControl  or PageLayoutControl are not initialized!");
            //创建一个地图实例
            IMap pNewMap = new MapClass();
            pNewMap.Name = "Map";
            IMaps pMaps = new Maps();
            pMaps.Add(pNewMap);
            pPageLayoutControl.PageLayout.ReplaceMaps(pMaps);
            pMapControl.Map = pNewMap;
            //reset the active tools 
            pPageLayoutActiveTool = null;
            pMapActiveTool = null;
            if (_ActivateMapFirst)
                this.ActivateMap();
            else
                this.ActivatePageLayout();
        }

        public void AddFrameworkControl(object _Control)
        {
            if (_Control == null)
                throw new Exception("ControlsSynchronizer::AddFrameworkControl:\r\nAdded control is not initialized!");

            pFrameworkControls.Add(_Control);
        }

        #endregion

    }
}
