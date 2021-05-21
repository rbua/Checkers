using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CheckersGame.Models
{
    public class Cell : Canvas
    {
        public Position CellPosition { get; set; }
        public Border CellBorder { get; set; }
        public bool Disabled { get; set; }
        public Marker Owner { get; set; }
        public int PossibleBeatsNumber { get; set; }
        public List<Scenario> BeatScenarios { get; set; }
        public List<Scenario> BestBeatScenarios
        {
            get
            {
                return GetBestScenarios();
            } 
        }
        public int MaxDepth
        {
            get
            {
                return BeatScenarios.Select(scenario => scenario.List.Count).Max();
            }
        }

        public Cell()
        {
            BeatScenarios = new List<Scenario>();
        }

        private List<Scenario> GetBestScenarios()
        {
            List<Scenario> result = BeatScenarios.GroupBy
                        (cell => cell.List.Count,
                            (key, group) => new
                            {
                                Depth = key,
                                Scenarios = group
                            })
                            .OrderBy(c => c.Depth)
                            .SelectMany(x => x.Scenarios)
                            .ToList();
            return result;
        }

        public void AddImageMarker(MarkerColor color, bool isQueen = false)
        {
            if (color != MarkerColor.Undefined)
            {
                this.Children.Add(GetVisualHost(color, isQueen));
            }
        }

        public void RemoveImageMarker()
        {
            Visual childVisual; 

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(this); i++)
            {
                childVisual = (Visual)VisualTreeHelper.GetChild(this, i);

                if (childVisual is VisualHost)
                {
                    this.Children.Remove((UIElement)childVisual);
                }
            }
        }

        private VisualHost GetVisualHost(MarkerColor color, bool isQueen)
        {
            VisualHost visualHost = null;

            if (color != MarkerColor.Undefined)
            {
                string path = GameManager.GetMarkerPath(color, isQueen);
                DrawingVisual drawingVisual = new DrawingVisual();
                DrawingContext drawingContext = drawingVisual.RenderOpen();
                BitmapImage img = new BitmapImage(new Uri(path));
                drawingContext.DrawImage(img, new Rect(4, 4, img.PixelWidth / 6, img.PixelHeight / 6));
                drawingContext.Close();
                visualHost = new VisualHost { Visual = drawingVisual, IsHitTestVisible = false };
            }           

            return visualHost;
        }
    }
}
