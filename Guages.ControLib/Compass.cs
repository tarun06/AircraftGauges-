using SharpVectors.Converters;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Guages.ControLib
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Guages.ControLib"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Guages.ControLib;assembly=Guages.ControLib"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:Compass/>
    ///
    /// </summary>
    public class Compass : Control
    {

        #region Private variables

        private static readonly Dictionary<(double, double), string> DirectionAngleMap = new Dictionary<(double, double), string>
        {
            { (0, -90), "E" },
            { (90, 0), "W" },
            { (0, 90), "N" },
            { (-90, 0), "S" },
        };

        private int animatingSpeedFactor = 5;

        private bool isInitialValueSet = false;

        private double MajorDivisionsCount = 12;

        private double MaxValue = 36;

        private double minorDivisionsCount = 5;

        private double MinValue = 0;

        private SvgViewbox pointer;

        private Grid rootGrid;

        private Size ScaleLabelSize = new Size(40, 20);

        private double ScaleStartAngle = 270;

        private double ScaleSweepAngle = 360;

        private int scaleValuePrecision = 5;

        #endregion

        #region Dependency properties

        /// <summary>
        /// Dependency property to Get/Set the current value
        /// </summary>
        public static readonly DependencyProperty CurrentValueProperty =
            DependencyProperty.Register(nameof(CurrentValue), typeof(double), typeof(Compass),
            new PropertyMetadata(Double.MinValue, new PropertyChangedCallback(Compass.OnCurrentValuePropertyChanged)));

        /// <summary>
        /// Dependency property to Get/Set the Major Tick Color
        /// </summary>
        public static readonly DependencyProperty MajorTickColorProperty =
               DependencyProperty.Register(nameof(MajorTickColor), typeof(Color), typeof(Compass), null);

        /// <summary>
        /// Dependency property to Get/Set the Major Tick Size
        /// </summary>
        public static readonly DependencyProperty MajorTickSizeProperty =
          DependencyProperty.Register(nameof(MajorTickSize), typeof(Size), typeof(Compass), null);

        /// <summary>
        /// Dependency property to Get/Set the Minor Tick Size
        /// </summary>
        public static readonly DependencyProperty MinorTickSizeProperty =
          DependencyProperty.Register(nameof(MinorTickSize), typeof(Size), typeof(Compass), null);

        /// <summary>
        /// Dependency property to Get/Set the pointer length
        /// </summary>
        public static readonly DependencyProperty PointerLengthProperty =
            DependencyProperty.Register(nameof(PointerLength), typeof(double), typeof(Compass), null);

        /// <summary>
        /// Dependency property to Get/Set the Pointer Thickness
        /// </summary>
        public static readonly DependencyProperty PointerThicknessProperty =
        DependencyProperty.Register(nameof(PointerThickness), typeof(double), typeof(Compass), null);

        /// <summary>
        /// Dependency property to Get/Set the Radius of the gauge
        /// </summary>
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register(nameof(Radius), typeof(double), typeof(Compass), null);

        /// <summary>
        /// Dependency property to Get/Set the scale label Radius
        /// </summary>
        public static readonly DependencyProperty ScaleLabelRadiusProperty =
            DependencyProperty.Register(nameof(ScaleLabelRadius), typeof(double), typeof(Compass), null);

        /// <summary>
        /// Dependency property to Get/Set the scale Radius
        /// </summary>
        public static readonly DependencyProperty ScaleRadiusProperty =
            DependencyProperty.Register(nameof(ScaleRadius), typeof(double), typeof(Compass), null);

        #endregion

        #region Constructor

        static Compass()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Compass), new FrameworkPropertyMetadata(typeof(Compass)));
        }

        #endregion

        #region Wrapper properties

        /// <summary>
        /// Gets/Sets the current value
        /// </summary>
        public double CurrentValue
        {
            get
            {
                return (double)GetValue(CurrentValueProperty);
            }
            set
            {
                SetValue(CurrentValueProperty, value);
            }
        }
        /// <summary>
        /// Gets/Sets the Major Tick Color
        /// </summary>
        public Color MajorTickColor
        {
            get
            {
                return (Color)GetValue(MajorTickColorProperty);
            }
            set
            {
                SetValue(MajorTickColorProperty, value);
            }
        }

        /// <summary>
        /// Gets/Sets the Major Tick Size
        /// </summary>
        public Size MajorTickSize
        {
            get
            {
                return (Size)GetValue(MajorTickSizeProperty);
            }
            set
            {
                SetValue(MajorTickSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets/Sets the Minor Tick Size
        /// </summary>
        public Size MinorTickSize
        {
            get
            {
                return (Size)GetValue(MinorTickSizeProperty);
            }
            set
            {
                SetValue(MinorTickSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets/Sets the Pointer Length
        /// </summary>
        public double PointerLength
        {
            get
            {
                return (double)GetValue(PointerLengthProperty);
            }
            set
            {
                SetValue(PointerLengthProperty, value);
            }
        }

        /// <summary>
        /// Gets/Sets the Pointer Thickness
        /// </summary>
        public double PointerThickness
        {
            get
            {
                return (double)GetValue(PointerThicknessProperty);
            }
            set
            {
                SetValue(PointerThicknessProperty, value);
            }
        }

        /// <summary>
        /// Gets/Sets the Minimum Value
        /// </summary>
        public double Radius
        {
            get
            {
                return (double)GetValue(RadiusProperty);
            }
            set
            {
                SetValue(RadiusProperty, value);
            }
        }

        /// <summary>
        /// Gets/Sets the Scale Label Radius
        /// </summary>
        public double ScaleLabelRadius
        {
            get
            {
                return (double)GetValue(ScaleLabelRadiusProperty);
            }
            set
            {
                SetValue(ScaleLabelRadiusProperty, value);
            }
        }

        /// <summary>
        /// Gets/Sets the Scale radius
        /// </summary>
        public double ScaleRadius
        {
            get
            {
                return (double)GetValue(ScaleRadiusProperty);
            }
            set
            {
                SetValue(ScaleRadiusProperty, value);
            }
        }

        #endregion

        #region Methods

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //Get reference to known elements on the control template
            rootGrid = (Grid)GetTemplateChild("LayoutRoot");
            pointer = (SvgViewbox)GetTemplateChild("Pointer");

            ////scale and the range indicator
            Canvas.SetZIndex(pointer, 100);

            DrawScale();
        }

        public virtual void OnCurrentValueChanged(DependencyPropertyChangedEventArgs e)
        {
            //Validate and set the new value
            double newValue = (double)e.NewValue;
            double oldValue = (double)e.OldValue;

            if (newValue > this.MaxValue)
            {
                newValue = this.MaxValue;
            }
            else if (newValue < this.MinValue)
            {
                newValue = this.MinValue;
            }

            if (oldValue > this.MaxValue)
            {
                oldValue = this.MaxValue;
            }
            else if (oldValue < this.MinValue)
            {
                oldValue = this.MinValue;
            }

            if (pointer != null)
            {
                double realworldunit = (ScaleSweepAngle / (MaxValue - MinValue));
                //Resetting the old value to min value the very first time.
                if (oldValue == 0 && !isInitialValueSet)
                {
                    oldValue = MinValue;
                    isInitialValueSet = true;
                }

                double oldcurr_realworldunit;
                double db1;

                if (oldValue < 0)
                {
                    db1 = MinValue + Math.Abs(oldValue);
                    oldcurr_realworldunit = ((double)(Math.Abs(db1 * realworldunit)));
                }
                else
                {
                    db1 = Math.Abs(oldValue - Math.Abs(MinValue));
                    oldcurr_realworldunit = ((double)(db1 * realworldunit));
                }
                double newcurr_realworldunit;
                if (newValue < 0)
                {
                    db1 = MinValue + Math.Abs(newValue);
                    newcurr_realworldunit = ((double)(Math.Abs(db1 * realworldunit)));
                }
                else
                {
                    db1 = Math.Abs(newValue - Math.Abs(MinValue));
                    newcurr_realworldunit = ((double)(db1 * realworldunit));
                }

                var oldcurrentvalueAngle = (ScaleStartAngle + oldcurr_realworldunit);
                var newcurrentvalueAngle = (ScaleStartAngle + newcurr_realworldunit);

                //Animate the pointer from the old value to the new value
                AnimatePointer(oldcurrentvalueAngle, newcurrentvalueAngle);
            }
        }

        private static void OnCurrentValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Compass gauge)
                gauge.OnCurrentValueChanged(e);
        }
        /// <summary>
        /// Animates the pointer to the current value to the new one
        /// </summary>
        /// <param name="oldcurrentvalueAngle"></param>
        /// <param name="newcurrentvalueAngle"></param>
        private void AnimatePointer(double oldcurrentvalueAngle, double newcurrentvalueAngle)
        {
            if (pointer != null)
            {
                DoubleAnimation da = new DoubleAnimation();
                da.From = oldcurrentvalueAngle;
                da.To = newcurrentvalueAngle;

                double animDuration = Math.Abs(oldcurrentvalueAngle - newcurrentvalueAngle) * animatingSpeedFactor;
                da.Duration = new Duration(TimeSpan.FromMilliseconds(animDuration));

                Storyboard sb = new Storyboard();
                sb.Children.Add(da);
                Storyboard.SetTarget(da, pointer);
                Storyboard.SetTargetProperty(da, new PropertyPath("(Path.RenderTransform).(TransformGroup.Children)[0].(RotateTransform.Angle)"));

                if (newcurrentvalueAngle != oldcurrentvalueAngle)
                {
                    sb.Begin();
                }
            }
        }

        //Drawing the scale with the Scale Radius
        private void DrawScale()
        {
            //Calculate one major tick angle
            var majorTickUnitAngle = ScaleSweepAngle / MajorDivisionsCount;

            //Obtaining One major ticks value
            var majorTicksUnitValue = (MaxValue - MinValue) / MajorDivisionsCount;

            double minvalue = 0;

            // Drawing Major scale ticks
            for (var i = ScaleStartAngle; i <= (ScaleStartAngle + ScaleSweepAngle); i = i + majorTickUnitAngle)
            {
                //Majortick is drawn as a rectangle
                Rectangle majortickrect = new Rectangle
                {
                    Height = MajorTickSize.Height,
                    Width = MajorTickSize.Width,
                    Fill = new SolidColorBrush(MajorTickColor)
                };

                Point p = new Point(0.5, 0.5);
                majortickrect.RenderTransformOrigin = p;

                TransformGroup majortickgp = new TransformGroup();
                RotateTransform majortickrt = new RotateTransform();

                //Obtaining the angle in radians for calulating the points
                var i_radian = (i * Math.PI) / 180;
                majortickrt.Angle = i;
                majortickgp.Children.Add(majortickrt);

                TranslateTransform majorticktt = new TranslateTransform
                {
                    //Finding the point on the Scale where the major ticks are drawn
                    //here drawing the points with center as (0,0)
                    X = (int)((ScaleRadius) * Math.Cos(i_radian)),
                    Y = (int)((ScaleRadius) * Math.Sin(i_radian))
                };

                //Points for the textblock which hold the scale value
                TranslateTransform majorscalevaluett = new TranslateTransform
                {
                    //here drawing the points with center as (0,0)
                    X = (int)((ScaleLabelRadius) * Math.Cos(i_radian)),
                    Y = (int)((ScaleLabelRadius) * Math.Sin(i_radian))
                };

                //Defining the properties of the scale value textbox
                TextBlock tb = new TextBlock
                {
                    Height = ScaleLabelSize.Height,
                    Width = ScaleLabelSize.Width,
                    FontSize = 14,
                    Foreground = new SolidColorBrush(MajorTickColor),
                    TextAlignment = TextAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                //checking minvalue < maxvalue w.r.t scale precion value
                if (Math.Round(minvalue, scaleValuePrecision) <= Math.Round(MaxValue, scaleValuePrecision))
                {
                    minvalue = Math.Round(minvalue, scaleValuePrecision);

                    if (DirectionAngleMap.TryGetValue((majorscalevaluett.X, majorscalevaluett.Y), out var txt))
                        tb.Text = txt.ToString();
                    else
                        tb.Text = minvalue.ToString();

                    minvalue = minvalue + majorTicksUnitValue;
                }
                else
                {
                    break;
                }
                majortickgp.Children.Add(majorticktt);
                majortickrect.RenderTransform = majortickgp;
                tb.RenderTransform = majorscalevaluett;
                rootGrid.Children.Add(majortickrect);
                Canvas.SetZIndex(majortickrect, 110);
                rootGrid.Children.Add(tb);

                //Drawing the minor axis ticks
                var onedegree = ((i + majorTickUnitAngle) - i) / (minorDivisionsCount);

                if ((i < (ScaleStartAngle + ScaleSweepAngle)) && (Math.Round(minvalue, scaleValuePrecision) <= Math.Round(MaxValue, scaleValuePrecision)))
                {
                    //Drawing the minor scale
                    for (var mi = i + onedegree; mi < (i + majorTickUnitAngle); mi = mi + onedegree)
                    {
                        //here the minortick is drawn as a rectangle
                        Rectangle minorTick = new Rectangle
                        {
                            Height = MinorTickSize.Height,
                            Width = MinorTickSize.Width,
                            Fill = new SolidColorBrush(MajorTickColor),
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center
                        };

                        Point p1 = new Point(0.5, 0.5);
                        minorTick.RenderTransformOrigin = p1;

                        TransformGroup minortickgp = new TransformGroup();
                        RotateTransform minortickrt = new RotateTransform();
                        minortickrt.Angle = mi;
                        minortickgp.Children.Add(minortickrt);
                        TranslateTransform minorticktt = new TranslateTransform();

                        //Obtaining the angle in radians for calulating the points
                        var mi_radian = (mi * Math.PI) / 180;
                        //Finding the point on the Scale where the minor ticks are drawn
                        minorticktt.X = (int)((ScaleRadius) * Math.Cos(mi_radian));
                        minorticktt.Y = (int)((ScaleRadius) * Math.Sin(mi_radian));

                        Canvas.SetZIndex(minorTick, 110);
                        minortickgp.Children.Add(minorticktt);
                        minorTick.RenderTransform = minortickgp;
                        rootGrid.Children.Add(minorTick);
                    }
                }
            }
        }

        #endregion
    }
}