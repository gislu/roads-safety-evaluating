using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesGDB;

namespace Project
{
    class Search
    {

        /// <summary>
        /// 打开shpfile文件

        public IFeatureClass GetFeatureClass(string FilePath, string LayerName)
        {

            IWorkspaceFactory pWks = new ShapefileWorkspaceFactoryClass();

            IFeatureWorkspace pFwk = pWks.OpenFromFile(FilePath, 0) as IFeatureWorkspace;

            IFeatureClass pRtClass = pFwk.OpenFeatureClass(LayerName);

            return pRtClass;
        }
        /// 输出结果为一个张表，这张表有3个字段，其中面ID为面要素数据的FID
        /// 个数用于记录这个面包含的点的个数    
        /// 
        public ITable CreateTable(string _TablePath, string _TableName)
        {
            IWorkspaceFactory pWks = new ShapefileWorkspaceFactoryClass();

            IFeatureWorkspace pFwk = pWks.OpenFromFile(_TablePath, 0) as IFeatureWorkspace;

            //用于记录面中的ID;

            IField pFieldID = new FieldClass();

            IFieldEdit pFieldIID = pFieldID as IFieldEdit;

            pFieldIID.Type_2 = esriFieldType.esriFieldTypeInteger;

            pFieldIID.Name_2 = "面ID";

            //用于记录个数的;
            IField pFieldCount = new FieldClass();

            IFieldEdit pFieldICount = pFieldCount as IFieldEdit;

            pFieldICount.Type_2 = esriFieldType.esriFieldTypeInteger;
            pFieldICount.Name_2 = "个数";

            //用于添加表中的必要字段
            IObjectClassDescription objectClassDescription = new ObjectClassDescriptionClass();

            IFields pTableFields = objectClassDescription.RequiredFields;

            IFieldsEdit pTableFieldsEdit = pTableFields as IFieldsEdit;

            pTableFieldsEdit.AddField(pFieldID);

            pTableFieldsEdit.AddField(pFieldCount);

            ITable pTable = pFwk.CreateTable(_TableName, pTableFields, null, null, "");

            return pTable;

        }

        /// 第一个参数为面数据，第二个参数为点数据，第三个为输出的表
        public void StatisticPointCount(IFeatureClass _pPolygonFClass, IFeatureClass _pPointFClass, ITable _pTable)
        {

            IFeatureCursor pPolyCursor = _pPolygonFClass.Search(null, false);

            IFeature pPolyFeature = pPolyCursor.NextFeature();

            while (pPolyFeature != null)
            {

                IGeometry pPolGeo = pPolyFeature.Shape;

                int Count = 0;

                ISpatialFilter spatialFilter = new SpatialFilterClass();

                spatialFilter.Geometry = pPolGeo;
                //过滤条件为空间包含（SpatialRelContains）
                spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;

                //属性过滤


                IFeatureCursor pPointCur = _pPointFClass.Search(spatialFilter, false);

                if (pPointCur != null)
                {
                    IFeature pPointFeature = pPointCur.NextFeature();

                    while (pPointFeature != null)
                    {
                        pPointFeature = pPointCur.NextFeature();
                        Count++;
                    }

                }

                if (Count != 0)
                {

                    IRow pRow = _pTable.CreateRow();
                    pRow.set_Value(1, pPolyFeature.get_Value(0));
                    pRow.set_Value(2, Count);
                    pRow.Store();
                }
                pPolyFeature = pPolyCursor.NextFeature();
            }
            MessageBox.Show("查询表已经创建成功");
        }

    }
}
