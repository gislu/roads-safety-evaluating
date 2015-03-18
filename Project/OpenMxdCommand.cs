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
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;


namespace Project
{
    /// <summary>
    /// Summary description for OpenMxdCommand.
    /// </summary>
    [Guid("591a81d7-c0f3-4f18-a078-8fca57647f9a")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("ArcGIS开发EngineApplication.OpenMxdCommand")]
    public sealed class OpenMxdCommand : BaseCommand
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
            ControlsCommands.Register(regKey);

        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            ControlsCommands.Unregister(regKey);

        }

        #endregion
        #endregion
        IMapControl2 pMapControl = null;
        private IHookHelper m_hookHelper;

        private ControlsSynchronizer pControlsSynchronizer = null;
        private string m_sDocumentPath = string.Empty;

        public OpenMxdCommand(ControlsSynchronizer controlsSynchronizer)
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "打开地图文档"; //localizable text
            base.m_caption = "打开地图文档";  //localizable text
            base.m_message = "打开地图文档";  //localizable text 
            base.m_toolTip = "打开地图文档";  //localizable text
            base.m_name = "打开地图文档";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

            pControlsSynchronizer = controlsSynchronizer; 
 

            try
            {
                //
                // TODO: change bitmap name if necessary
                //
                string bitmapResourceName = GetType().Name + ".bmp";
                base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }

        public OpenMxdCommand()
        {
            // TODO: Complete member initialization
        }

        #region Overridden Class Methods

        /// <summary>
        /// Occurs when this command is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            if (hook == null)
                return;
            //在这里对hook进行判断
            //if (m_hookHelper == null)
            //    m_hookHelper = new HookHelperClass();

            //m_hookHelper.Hook = hook;

            if (hook is IToolbarControl)
            {
                IToolbarControl pToolBar = hook as IToolbarControl;
                pMapControl = pToolBar.Buddy as IMapControl2;
            }
            else if (hook is IMapControl2)
            {
                pMapControl = hook as IMapControl2;
            }

          
            // TODO:  Add other initialization code
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add OpenNewMapDocument.OnClick implementation
            //launch a new OpenFile dialog
            System.Windows.Forms.OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Map Documents (*.mxd)|*.mxd";
            dlg.Multiselect = false;
            dlg.Title = "Open Map Document";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string docName = dlg.FileName;
                IMapDocument pMapDoc = new MapDocumentClass();
                if (pMapDoc.get_IsPresent(docName) && !pMapDoc.get_IsPasswordProtected(docName))
                {  
                    // 以下3.3.3.1代码
                    /* pMapControl.LoadMxFile(dlg.FileName, null, null);
                      pMapControl.ActiveView.Refresh(); 
                      pMapDoc.Close();                     */
                    // 以下3.3.3.5代码
                    pMapDoc.Open(docName, string.Empty);
                    IMap map = pMapDoc.get_Map(0);
                    pMapDoc.SetActiveView((IActiveView)map);
                    pControlsSynchronizer.PageLayoutControl.PageLayout = pMapDoc.PageLayout;
                    pControlsSynchronizer.ReplaceMap(map);
                    pMapDoc.Close();
                }
            }
        }


        }

        #endregion
    }

