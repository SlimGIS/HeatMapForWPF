using SlimGis.MapKit.Controls;
using SlimGis.MapKit.Geometries;
using SlimGis.MapKit.Layers;
using SlimGis.MapKit.Symbologies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HeatMapForWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly string columnName = "MED_AGE";
        private ColorPaletteType colorPaletteType;

        public MainWindow()
        {
            InitializeComponent();
            ColorPaletteTypeComboBox.Items.Add(ColorPaletteType.Default);
            ColorPaletteTypeComboBox.Items.Add(ColorPaletteType.Gray);
            ColorPaletteTypeComboBox.Items.Add(ColorPaletteType.Rainbow);
            ColorPaletteTypeComboBox.Items.Add(ColorPaletteType.RedWhiteBlue);
            ColorPaletteTypeComboBox.Items.Add(ColorPaletteType.Thermal);
        }

        private void map_Loaded(object sender, RoutedEventArgs e)
        {
            map.MapUnit = GeoUnit.Meter;
            map.UseOpenStreetMapAsBaseMap();

            ShapefileLayer pointLayer = new ShapefileLayer("SampleData/cities-900913.shp");
            HeatStyle heatStyle = new HeatStyle();
            heatStyle.DataColumn = columnName;
            heatStyle.MaxIntensity = 37.7;
            heatStyle.MinIntensity = 30;
            heatStyle.Alpha = 150;
            heatStyle.Radius = (2 - HeatPointSizeComboBox.SelectedIndex) * 30 + 20;
            heatStyle.ColorPaletteType = colorPaletteType;
            pointLayer.Styles.Add(heatStyle);
            map.AddStaticLayers("PointOverlay", pointLayer);

            map.ZoomTo(pointLayer.GetBound());
        }

        private void HeatPointSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }

        private void ColorPaletteTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            colorPaletteType = (ColorPaletteType)ColorPaletteTypeComboBox.SelectedItem;
            Refresh();
        }

        private void Refresh()
        {
            ShapefileLayer pointLayer = map.FindLayer<ShapefileLayer>("cities-900913");
            if (pointLayer == null) return;
            HeatStyle heatStyle = (HeatStyle)pointLayer.Styles[0];
            heatStyle.Radius = (2 - HeatPointSizeComboBox.SelectedIndex) * 30 + 20;
            heatStyle.ColorPaletteType = colorPaletteType;

            map.Refresh(RefreshType.Redraw);
        }
    }
}
