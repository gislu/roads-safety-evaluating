using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace Project
{
    public partial class Form2 : Form
    {
        IFeatureLayer pFeaturelayer = null;//申明一个 pFeaturelayer变量
        public Form2(IFeatureLayer featureLayer)
        {
            InitializeComponent();
            pFeaturelayer = featureLayer;
            Itable2Dtable();
        }
        public static string ParseFieldType(esriFieldType fieldType)//将EsriType 转换为String
        {
            switch (fieldType)
            {
                case esriFieldType.esriFieldTypeBlob:
                    return "System.String";
                case esriFieldType.esriFieldTypeDate:
                    return "System.DateTime";
                case esriFieldType.esriFieldTypeDouble:
                    return "System.Double";
                case esriFieldType.esriFieldTypeGeometry:
                    return "System.String";
                case esriFieldType.esriFieldTypeGlobalID:
                    return "System.String";
                case esriFieldType.esriFieldTypeGUID:
                    return "System.String";
                case esriFieldType.esriFieldTypeInteger:
                    return "System.Int32";
                case esriFieldType.esriFieldTypeOID:
                    return "System.String";
                case esriFieldType.esriFieldTypeRaster:
                    return "System.String";
                case esriFieldType.esriFieldTypeSingle:
                    return "System.Single";
                case esriFieldType.esriFieldTypeSmallInteger:
                    return "System.Int32";
                case esriFieldType.esriFieldTypeString:
                    return "System.String";
                default:
                    return "System.String";
            }
        }
       
        public void Itable2Dtable()
        {
            IFields pFields;
            pFields = pFeaturelayer.FeatureClass.Fields;
            dataGridView1.ColumnCount = pFields.FieldCount;
            for (int i = 0; i < pFields.FieldCount; i++)
            {
                string fldName = pFields.get_Field(i).Name;
                dataGridView1.Columns[i].Name = fldName;
                dataGridView1.Columns[i].ValueType = System.Type.GetType(ParseFieldType(pFields.get_Field(i).Type));
            }
            IFeatureCursor pFeatureCursor;
            pFeatureCursor = pFeaturelayer.FeatureClass.Search(null, false);
            IFeature pFeature;
            pFeature = pFeatureCursor.NextFeature();
            while (pFeature != null)
            {
                string[] fldValue = new string[pFields.FieldCount];
                for (int i = 0; i < pFields.FieldCount; i++)
                {
                    string fldName;
                    fldName = pFields.get_Field(i).Name;
                    if (fldName == pFeaturelayer.FeatureClass.ShapeFieldName)
                    {
                        fldValue[i] = Convert.ToString(pFeature.Shape.GeometryType);
                    }
                    else
                        fldValue[i] = Convert.ToString(pFeature.get_Value(i));
                }
                dataGridView1.Rows.Add(fldValue);
                pFeature = pFeatureCursor.NextFeature();
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
