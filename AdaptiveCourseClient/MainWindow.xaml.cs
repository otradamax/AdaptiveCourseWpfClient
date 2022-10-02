﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using AdaptiveCourseClient.RenderObjects;

namespace AdaptiveCourseClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UIElement? currentUIElement;
        private UIElementGroup? logicElement;
        private Point logicElementOffset;
        private List<UIElementGroup> logicElements;
        private List<UIElement> rightInputs;
        private UIElementGroup Inputs;

        private int InputsNum = 4;
        private int InputHeight = 10; 

        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;

            logicElements = new List<UIElementGroup>();
            rightInputs = new List<UIElement>();
            Inputs = new UIElementGroup();

            for (int i = 0; i < 3; i++)
            {
                ElementAND elementAND = new ElementAND();
                logicElements.Add(elementAND.AddAND(bodyCanvas));
                rightInputs.Add(elementAND.AddRightInputAround(bodyCanvas));
                elementAND.MoveLogicBlock(LogicElementAND_PreviewMouseLeftButtonDown, LogicElement_PreviewMouseMove,
                    LogicElement_PreviewMouseLeftButtonUp);
                elementAND.ChangeInputsOutputs();
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < InputsNum; i++)
            {
                Polygon leftInput = new Polygon();
                leftInput.Fill = Brushes.White;
                leftInput.Stroke = Brushes.Black;
                leftInput.StrokeThickness = 3;
                PointCollection leftPoints = new PointCollection();
                leftPoints.Add(new Point(Toolbox.ActualWidth,
                    this.Height * ((double)(i + 1) / (InputsNum + 1)) - InputHeight));
                leftPoints.Add(new Point(Toolbox.ActualWidth,
                    this.Height * ((double)(i + 1) / (InputsNum + 1)) + InputHeight));
                leftPoints.Add(new Point(Toolbox.ActualWidth + 40,
                    this.Height * ((double)(i + 1) / (InputsNum + 1))));
                leftInput.Points = leftPoints;
                leftInput.MouseMove += LeftInput_MouseMove;
                leftInput.MouseLeave += LeftInput_MouseLeave;
                bodyCanvas.Children.Add(leftInput);
                Inputs.Add(leftInput);
            }

            Polygon rightInput = new Polygon();
            rightInput.Fill = Brushes.White;
            rightInput.Stroke = Brushes.Black;
            rightInput.StrokeThickness = 3;
            PointCollection points = new PointCollection();
            points.Add(new Point(bodyCanvas.ActualWidth,
                this.Height / 2 - InputHeight));
            points.Add(new Point(bodyCanvas.ActualWidth,
                this.Height / 2 + InputHeight));
            points.Add(new Point(bodyCanvas.ActualWidth - 40,
                this.Height / 2));
            rightInput.Points = points;
            bodyCanvas.Children.Add(rightInput);
        }

        private void LeftInput_MouseLeave(object sender, MouseEventArgs e)
        {
            Polygon input = (Polygon)sender;
            if (input != null)
            {
                input.Stroke = Brushes.Black;
            }
        }

        private void LeftInput_MouseMove(object sender, MouseEventArgs e)
        {
            Polygon input = (Polygon)sender;
            if (input != null)
            {
                input.Stroke = Brushes.Red;
            }
        }

        private void LogicElementAND_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            currentUIElement = (UIElement)sender;
            foreach(UIElementGroup logicBlock in logicElements)
            {
                if (logicBlock.Contains(currentUIElement))
                {
                    logicElement = logicBlock;
                }
            }
            logicElementOffset = e.GetPosition(bodyCanvas);
            logicElementOffset.Y -= Canvas.GetTop(currentUIElement);
            logicElementOffset.X -= Canvas.GetLeft(currentUIElement);
            if (logicElement != null)
            {
                foreach (UIElement uIElement in logicElement)
                {
                    Panel.SetZIndex(uIElement, 1);
                }
            }
            currentUIElement.CaptureMouse();
        }

        private void LogicElement_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (logicElement == null)
                return;

            // Logic element movement
            Point cursorPosition = e.GetPosition(sender as Canvas);

            double positionXMain = Canvas.GetLeft(logicElement[0]);
            double positionYMain = Canvas.GetTop(logicElement[0]);

            foreach (UIElement uIElement in logicElement)
            {
                double Y = cursorPosition.Y - logicElementOffset.Y - positionYMain;
                double X = cursorPosition.X - logicElementOffset.X - positionXMain;
                if (uIElement is Line)
                {
                    Line line = (Line)uIElement;
                    double positionX = line.X1;
                    double positionY = line.Y1;
                    Y += positionY;
                    X += positionX;
                    line.X1 = X;
                    line.Y1 = Y;
                    line.X2 = X + ElementAND.circleDiameter / 2;
                    line.Y2 = Y;
                }
                else
                {
                    double positionX = Canvas.GetLeft(uIElement);
                    double positionY = Canvas.GetTop(uIElement);
                    Y += positionY;
                    X += positionX;
                    Canvas.SetTop(uIElement, Y);
                    Canvas.SetLeft(uIElement, X);
                }
            }
        }

        private void LogicElement_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (logicElement == null)
                return;

            Point position = e.GetPosition(sender as Canvas);
            foreach (UIElement uIElement in logicElement)
            {
                if (position.X < Toolbox.ActualWidth)
                {
                    Canvas.SetTop(uIElement, 50);
                    Canvas.SetLeft(uIElement, 50);
                }
                Panel.SetZIndex(uIElement, 0);
            }
            currentUIElement?.ReleaseMouseCapture();
            currentUIElement = null;
            logicElement = null;
        }
    }
}
