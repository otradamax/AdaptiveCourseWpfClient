﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AdaptiveCourseClient.RenderObjects
{
    public class IOElement : Element
    {
        public Polygon? Body;

        protected readonly double _contactWidth = 20;
        protected readonly double _textSize = 14;

        public IOElement(Canvas canvas) : base(canvas)
        {
            _canvas = canvas;
        }

        protected void Input_MouseMove(object sender, MouseEventArgs e)
        {
            Polygon input = (Polygon)sender;
            if (input != null)
            {
                input.Stroke = Brushes.Red;
            }
        }

        protected void Input_MouseLeave(object sender, MouseEventArgs e)
        {
            Polygon input = (Polygon)sender;
            if (input != null)
            {
                input.Stroke = Brushes.Black;
            }
        }

        public override bool HasConnection(Point point)
        {
            return false;
        }

        public override void MakeConnection(ConnectionLine connectionLine)
        {
        }

        public override void CreateNodes(ConnectionLine connectionLine)
        {
        }

        public override bool HasNegationOnContact(Point point) => false;
    }
}
