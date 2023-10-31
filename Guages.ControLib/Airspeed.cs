using System;
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
    ///     "Add Reference"->"Projects"->[Select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class Airspeed : Control
    {
        #region Private variables

        private int animatingSpeedFactor = 5;
        private double arcradius1;

        private double arcradius2;

        private bool isInitialValueSet = false;
        private Path pointer;

        private Ellipse pointerCap;

        private Path rangeIndicator;

        //Private variables
        private Grid rootGrid;

        #endregion Private variables

        #region Dependency properties

        public static readonly DependencyProperty AboveOptimalRangeColorProperty =
                    DependencyProperty.Register(nameof(AboveOptimalRangeColor), typeof(Color), typeof(Airspeed), null);

        /// <summary>
        /// Dependency property to Get/Set Optimal Range End Value
        /// </summary>
        public static readonly DependencyProperty AboveOptimalRangeEndValueProperty =
           DependencyProperty.Register(nameof(AboveOptimalRangeEndValue), typeof(double), typeof(Airspeed), new PropertyMetadata(new PropertyChangedCallback(Airspeed.OnAboveOptimalRangeEndValuePropertyChanged)));

        /// <summary>
        /// Dependency property to Get/Set the current value
        /// </summary>
        public static readonly DependencyProperty CurrentValueProperty =
            DependencyProperty.Register(nameof(CurrentValue), typeof(double), typeof(Airspeed),
            new PropertyMetadata(Double.MinValue, new PropertyChangedCallback(Airspeed.OnCurrentValuePropertyChanged)));

        public static readonly DependencyProperty DialTextColorProperty =
                    DependencyProperty.Register(nameof(DialTextColor), typeof(Brush), typeof(Airspeed), null);

        public static readonly DependencyProperty DialTextFontSizeProperty =
                    DependencyProperty.Register(nameof(DialTextFontSize), typeof(int), typeof(Airspeed), null);

        public static readonly DependencyProperty DialTextOffsetProperty =
                    DependencyProperty.Register(nameof(DialTextOffset), typeof(double), typeof(Airspeed), null);

        public static readonly DependencyProperty DialTextProperty =
                    DependencyProperty.Register(nameof(DialText), typeof(string), typeof(Airspeed), null);

        /// <summary>
        /// Dependency property to Get/Set the Gauge Background Color
        /// </summary>
        public static readonly DependencyProperty GaugeBackgroundColorProperty =
          DependencyProperty.Register(nameof(GaugeBackgroundColor), typeof(Brush), typeof(Airspeed), null);

        /// <summary>
        /// Dependency property to Get/Set the number of major divisions on the scale
        /// </summary>
        public static readonly DependencyProperty MajorDivisionsCountProperty =
            DependencyProperty.Register(nameof(MajorDivisionsCount), typeof(double), typeof(Airspeed), null);

        /// <summary>
        /// Dependency property to Get/Set the Major Tick Color
        /// </summary>
        public static readonly DependencyProperty MajorTickColorProperty =
           DependencyProperty.Register(nameof(MajorTickColor), typeof(Color), typeof(Airspeed), null);

        /// <summary>
        /// Dependency property to Get/Set the Major Tick Size
        /// </summary>
        public static readonly DependencyProperty MajorTickSizeProperty =
          DependencyProperty.Register(nameof(MajorTickSize), typeof(Size), typeof(Airspeed), null);

        /// <summary>
        /// Dependency property to Get/Set the Maximum Value
        /// </summary>
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register(nameof(MaxValue), typeof(double), typeof(Airspeed), null);

        /// <summary>
        /// Dependency property to Get/Set the number of minor divisions on the scale
        /// </summary>
        public static readonly DependencyProperty MinorDivisionsCountProperty =
            DependencyProperty.Register(nameof(MinorDivisionsCount), typeof(double), typeof(Airspeed), null);

        /// <summary>
        /// Dependency property to Get/Set the Minor Tick Color
        /// </summary>
        public static readonly DependencyProperty MinorTickColorProperty =
          DependencyProperty.Register(nameof(MinorTickColor), typeof(Color), typeof(Airspeed), null);

        /// <summary>
        /// Dependency property to Get/Set the Minor Tick Size
        /// </summary>
        public static readonly DependencyProperty MinorTickSizeProperty =
          DependencyProperty.Register(nameof(MinorTickSize), typeof(Size), typeof(Airspeed), null);

        /// <summary>
        /// Dependency property to Get/Set the Minimum Value
        /// </summary>
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register(nameof(MinValue), typeof(double), typeof(Airspeed), null);

        public static readonly DependencyProperty OptimalRangeColorProperty =
                    DependencyProperty.Register(nameof(OptimalRangeColor), typeof(Color), typeof(Airspeed), null);

        /// <summary>
        /// Dependency property to Get/Set Optimal Range End Value
        /// </summary>
        public static readonly DependencyProperty OptimalRangeEndValueProperty =
           DependencyProperty.Register(nameof(OptimalRangeEndValue), typeof(double), typeof(Airspeed), new PropertyMetadata(new PropertyChangedCallback(Airspeed.OnOptimalRangeEndValuePropertyChanged)));

        /// <summary>
        /// Dependency property to Get/Set Optimal Range Start Value
        /// </summary>
        public static readonly DependencyProperty OptimalRangeStartValueProperty =
           DependencyProperty.Register(nameof(OptimalRangeStartValue), typeof(double), typeof(Airspeed), new PropertyMetadata(new PropertyChangedCallback(Airspeed.OnOptimalRangeStartValuePropertyChanged)));

        /// <summary>
        /// Dependency property to Get/Set the Pointer cap Radius
        /// </summary>
        public static readonly DependencyProperty PointerCapRadiusProperty =
            DependencyProperty.Register(nameof(PointerCapRadius), typeof(double), typeof(Airspeed), null);

        /// <summary>
        /// Dependency property to Get/Set the pointer length
        /// </summary>
        public static readonly DependencyProperty PointerLengthProperty =
            DependencyProperty.Register(nameof(PointerLength), typeof(double), typeof(Airspeed), null);

        /// <summary>
        /// Dependency property to Get/Set the Pointer Thickness
        /// </summary>
        public static readonly DependencyProperty PointerThicknessProperty =
        DependencyProperty.Register(nameof(PointerThickness), typeof(double), typeof(Airspeed), null);

        /// <summary>
        /// Dependency property to Get/Set the Radius of the gauge
        /// </summary>
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register(nameof(Radius), typeof(double), typeof(Airspeed), null);

        /// <summary>
        /// Dependency property to Get/Set the Range Indicator Radius
        /// </summary>
        public static readonly DependencyProperty RangeIndicatorRadiusProperty =
          DependencyProperty.Register(nameof(RangeIndicatorRadius), typeof(double), typeof(Airspeed), null);

        /// <summary>
        /// Dependency property to Get/Set the Range Indicator Thickness
        /// </summary>
        public static readonly DependencyProperty RangeIndicatorThicknessProperty =
         DependencyProperty.Register(nameof(RangeIndicatorThickness), typeof(double), typeof(Airspeed), null);

        /// <summary>
        /// Dependency property to Get/Set the an option to reset the pointer on start up to the minimum value
        /// </summary>
        public static readonly DependencyProperty ResetPointerOnStartUpProperty =
        DependencyProperty.Register(nameof(ResetPointerOnStartUp), typeof(bool), typeof(Airspeed), new PropertyMetadata(false, null));

        /// <summary>
        /// Dependency property to Get/Set the Scale Label FontSize
        /// </summary>
        public static readonly DependencyProperty ScaleLabelFontSizeProperty =
            DependencyProperty.Register(nameof(ScaleLabelFontSize), typeof(double), typeof(Airspeed), null);

        public static readonly DependencyProperty ScaleLabelForegroundProperty =
                    DependencyProperty.Register(nameof(ScaleLabelForeground), typeof(Color), typeof(Airspeed), null);

        /// <summary>
        /// Dependency property to Get/Set the scale label Radius
        /// </summary>
        public static readonly DependencyProperty ScaleLabelRadiusProperty =
            DependencyProperty.Register(nameof(ScaleLabelRadius), typeof(double), typeof(Airspeed), null);

        /// <summary>
        /// Dependency property to Get/Set the Scale Label Size
        /// </summary>
        public static readonly DependencyProperty ScaleLabelSizeProperty =
         DependencyProperty.Register(nameof(ScaleLabelSize), typeof(Size), typeof(Airspeed), null);

        /// <summary>
        /// Dependency property to Get/Set the scale Radius
        /// </summary>
        public static readonly DependencyProperty ScaleRadiusProperty =
            DependencyProperty.Register(nameof(ScaleRadius), typeof(double), typeof(Airspeed), null);

        /// <summary>
        /// Dependency property to Get/Set the starting angle of scale
        /// </summary>
        public static readonly DependencyProperty ScaleStartAngleProperty =
            DependencyProperty.Register(nameof(ScaleStartAngle), typeof(double), typeof(Airspeed), null);

        /// <summary>
        /// Dependency property to Get/Set the sweep angle of scale
        /// </summary>
        public static readonly DependencyProperty ScaleSweepAngleProperty =
            DependencyProperty.Register(nameof(ScaleSweepAngle), typeof(double), typeof(Airspeed), null);

        /// <summary>
        /// Dependency property to Get/Set the Scale Label Foreground
        /// </summary>
        /// <summary>
        /// Dependency property to Get/Set the Scale Value Precision
        /// </summary>
        public static readonly DependencyProperty ScaleValuePrecisionProperty =
        DependencyProperty.Register(nameof(ScaleValuePrecision), typeof(int), typeof(Airspeed), null);

        #endregion Dependency properties

        #region Wrapper properties

        /// <summary>
        /// Gets/Sets Above Optimal Range Color
        /// </summary>
        public Color AboveOptimalRangeColor
        {
            get
            {
                return (Color)GetValue(AboveOptimalRangeColorProperty);
            }
            set
            {
                SetValue(AboveOptimalRangeColorProperty, value);
            }
        }

        /// <summary>
        /// Gets/Sets the Optimal Range Start Value
        /// </summary>
        public double AboveOptimalRangeEndValue
        {
            get
            {
                return (double)GetValue(AboveOptimalRangeEndValueProperty);
            }
            set
            {
                SetValue(AboveOptimalRangeEndValueProperty, value);
            }
        }

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
        /// Gets/Sets Dial Text
        /// </summary>
        public string DialText
        {
            get
            {
                return (string)GetValue(DialTextProperty);
            }
            set
            {
                SetValue(DialTextProperty, value);
            }
        }

        /// <summary>
        /// Gets/Sets Dial Text Color
        /// </summary>
        public Brush DialTextColor
        {
            get
            {
                return (Brush)GetValue(DialTextColorProperty);
            }
            set
            {
                SetValue(DialTextColorProperty, value);
            }
        }

        /// <summary>
        /// Gets/Sets Dial Text Font Size
        /// </summary>
        public int DialTextFontSize
        {
            get
            {
                return (int)GetValue(DialTextFontSizeProperty);
            }
            set
            {
                SetValue(DialTextFontSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets/Sets Dial Text Offset
        /// </summary>
        public double DialTextOffset
        {
            get
            {
                return (double)GetValue(DialTextOffsetProperty);
            }
            set
            {
                SetValue(DialTextOffsetProperty, value);
            }
        }

        /// <summary>
        /// Gets/Sets the Gauge Background color
        /// </summary>
        public Brush GaugeBackgroundColor
        {
            get
            {
                return (Brush)GetValue(GaugeBackgroundColorProperty);
            }
            set
            {
                SetValue(GaugeBackgroundColorProperty, value);
            }
        }

        /// <summary>
        /// Gets/Sets the number of major divisions on the scale
        /// </summary>
        public double MajorDivisionsCount
        {
            get
            {
                return (double)GetValue(MajorDivisionsCountProperty);
            }
            set
            {
                SetValue(MajorDivisionsCountProperty, value);
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
        /// Gets/Sets the Maximum Value
        /// </summary>
        public double MaxValue
        {
            get
            {
                return (double)GetValue(MaxValueProperty);
            }
            set
            {
                SetValue(MaxValueProperty, value);
            }
        }

        /// <summary>
        /// Gets/Sets the number of minor divisions on the scale
        /// </summary>
        public double MinorDivisionsCount
        {
            get
            {
                return (double)GetValue(MinorDivisionsCountProperty);
            }
            set
            {
                SetValue(MinorDivisionsCountProperty, value);
            }
        }

        /// <summary>
        /// Gets/Sets the Minor Tick Color
        /// </summary>
        public Color MinorTickColor
        {
            get
            {
                return (Color)GetValue(MinorTickColorProperty);
            }
            set
            {
                SetValue(MinorTickColorProperty, value);
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
        /// Gets/Sets the Minimum Value
        /// </summary>
        public double MinValue
        {
            get
            {
                return (double)GetValue(MinValueProperty);
            }
            set
            {
                SetValue(MinValueProperty, value);
            }
        }

        /// <summary>
        /// Gets/Sets Optimal Range Color
        /// </summary>
        public Color OptimalRangeColor
        {
            get
            {
                return (Color)GetValue(OptimalRangeColorProperty);
            }
            set
            {
                SetValue(OptimalRangeColorProperty, value);
            }
        }

        /// <summary>
        /// Gets/Sets the Optimal range end value
        /// </summary>
        public double OptimalRangeEndValue
        {
            get
            {
                return (double)GetValue(OptimalRangeEndValueProperty);
            }
            set
            {
                SetValue(OptimalRangeEndValueProperty, value);
            }
        }

        /// <summary>
        /// Gets/Sets the Optimal Range Start Value
        /// </summary>
        public double OptimalRangeStartValue
        {
            get
            {
                return (double)GetValue(OptimalRangeStartValueProperty);
            }
            set
            {
                SetValue(OptimalRangeStartValueProperty, value);
            }
        }

        /// <summary>
        /// Gets/Sets the Pointer cap radius
        /// </summary>
        public double PointerCapRadius
        {
            get
            {
                return (double)GetValue(PointerCapRadiusProperty);
            }
            set
            {
                SetValue(PointerCapRadiusProperty, value);
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
        /// Gets/Sets the Range Indicator Radius
        /// </summary>
        public double RangeIndicatorRadius
        {
            get
            {
                return (double)GetValue(RangeIndicatorRadiusProperty);
            }
            set
            {
                SetValue(RangeIndicatorRadiusProperty, value);
            }
        }

        /// <summary>
        /// Gets/Sets the Range Indicator Thickness
        /// </summary>
        public double RangeIndicatorThickness
        {
            get
            {
                return (double)GetValue(RangeIndicatorThicknessProperty);
            }
            set
            {
                SetValue(RangeIndicatorThicknessProperty, value);
            }
        }

        /// <summary>
        /// Gets/Sets option to reset the pointer to minimum on start up, Default is true
        /// </summary>
        public bool ResetPointerOnStartUp
        {
            get
            {
                return (bool)GetValue(ResetPointerOnStartUpProperty);
            }
            set
            {
                SetValue(ResetPointerOnStartUpProperty, value);
            }
        }

        /// <summary>
        /// Gets/Sets the Scale Label Font Size
        /// </summary>
        public double ScaleLabelFontSize
        {
            get
            {
                return (double)GetValue(ScaleLabelFontSizeProperty);
            }
            set
            {
                SetValue(ScaleLabelFontSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets/Sets the Scale Label Foreground
        /// </summary>
        public Color ScaleLabelForeground
        {
            get
            {
                return (Color)GetValue(ScaleLabelForegroundProperty);
            }
            set
            {
                SetValue(ScaleLabelForegroundProperty, value);
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
        /// Gets/Sets the Scale Label Size
        /// </summary>
        public Size ScaleLabelSize
        {
            get
            {
                return (Size)GetValue(ScaleLabelSizeProperty);
            }
            set
            {
                SetValue(ScaleLabelSizeProperty, value);
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

        /// <summary>
        /// Gets/Sets the scale start angle
        /// </summary>
        public double ScaleStartAngle
        {
            get
            {
                return (double)GetValue(ScaleStartAngleProperty);
            }
            set
            {
                SetValue(ScaleStartAngleProperty, value);
            }
        }

        /// <summary>
        /// Gets/Sets the scale sweep angle
        /// </summary>
        public double ScaleSweepAngle
        {
            get
            {
                return (double)GetValue(ScaleSweepAngleProperty);
            }
            set
            {
                SetValue(ScaleSweepAngleProperty, value);
            }
        }

        /// <summary>
        /// Gets/Sets scale value precision
        /// </summary>
        public int ScaleValuePrecision
        {
            get
            {
                return (int)GetValue(ScaleValuePrecisionProperty);
            }
            set
            {
                SetValue(ScaleValuePrecisionProperty, value);
            }
        }

        #endregion Wrapper properties

        #region Constructor

        static Airspeed()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Airspeed), new FrameworkPropertyMetadata(typeof(Airspeed)));
        }

        #endregion Constructor

        #region Methods

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //Get reference to known elements on the control template
            rootGrid = (Grid)GetTemplateChild("LayoutRoot");
            pointer = (Path)GetTemplateChild("Pointer");
            pointerCap = (Ellipse)GetTemplateChild("PointerCap");

            DrawScale();
            DrawOptimalRangeIndicator();
            DrawAboveOptimalRangeIndicator();

            //Set Zindex of pointer and pointer cap to a really high number so that it stays on top of the
            //scale and the range indicator
            Canvas.SetZIndex(pointer, 100);
            Canvas.SetZIndex(pointerCap, 101);

            if (ResetPointerOnStartUp)
            {
                //Reset Pointer
                MovePointer(ScaleStartAngle);
            }
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

        private static void OnAboveOptimalRangeEndValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Airspeed gauge)
            {
                if ((double)e.NewValue > gauge.MaxValue)
                {
                    gauge.AboveOptimalRangeEndValue = gauge.MaxValue;
                }
            }
        }

        private static void OnCurrentValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Airspeed gauge)
                gauge.OnCurrentValueChanged(e);
        }

        private static void OnOptimalRangeEndValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Get access to the instance of CircularGaugeConrol whose property value changed
            if (d is Airspeed gauge)
            {
                if ((double)e.NewValue > gauge.MaxValue)
                {
                    gauge.OptimalRangeEndValue = gauge.MaxValue;
                }
            }
        }

        private static void OnOptimalRangeStartValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Airspeed gauge)
            {
                if ((double)e.NewValue < gauge.MinValue)
                {
                    gauge.OptimalRangeStartValue = gauge.MinValue;
                }
            }
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

        private void DrawAboveOptimalRangeIndicator()
        {
            DrawRangeIndicator(OptimalRangeEndValue, AboveOptimalRangeEndValue, AboveOptimalRangeColor);
        }

        private void DrawOptimalRangeIndicator()
        {
            DrawRangeIndicator(OptimalRangeStartValue, OptimalRangeEndValue, OptimalRangeColor);
        }

        /// <summary>
        /// Draw the range indicator
        /// </summary>
        private void DrawRangeIndicator(double startValue, double endValue, Color color)
        {
            double realworldunit = (ScaleSweepAngle / (MaxValue - MinValue));
            double startAngle;
            double endAngle;
            double db;

            if (startValue < 0)
            {
                db = MinValue + Math.Abs(startValue);
                startAngle = ((double)(Math.Abs(db * realworldunit)));
            }
            else
            {
                db = startValue - Math.Abs(MinValue);
                startAngle = ((double)(db * realworldunit));
            }

            if (endValue < 0)
            {
                db = MinValue + Math.Abs(endValue);
                endAngle = ((double)(Math.Abs(db * realworldunit)));
            }
            else
            {
                db = endValue - Math.Abs(MinValue);
                endAngle = ((double)(db * realworldunit));
            }

            var startAngleFromStart = (ScaleStartAngle + startAngle);

            var endAngleFromStart = (ScaleStartAngle + endAngle);

            arcradius1 = (RangeIndicatorRadius + RangeIndicatorThickness);
            arcradius2 = RangeIndicatorRadius;

            Point A1 = GetCircumferencePoint(startAngleFromStart, arcradius1);
            Point B1 = GetCircumferencePoint(startAngleFromStart, arcradius2);
            Point C1 = GetCircumferencePoint(endAngleFromStart, arcradius2);
            Point D1 = GetCircumferencePoint(endAngleFromStart, arcradius1);
            bool isReflexAngle1 = Math.Abs(endAngleFromStart - startAngleFromStart) > 180.0;
            DrawSegment(A1, B1, C1, D1, isReflexAngle1, color);
        }

        //Drawing the scale with the Scale Radius
        private void DrawScale()
        {
            //Calculate one major tick angle
            var majorTickUnitAngle = ScaleSweepAngle / MajorDivisionsCount;

            //Obtaining One major ticks value
            var majorTicksUnitValue = (MaxValue - MinValue) / MajorDivisionsCount;

            var minvalue = MinValue;

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
                    FontSize = ScaleLabelFontSize,
                    Foreground = new SolidColorBrush(ScaleLabelForeground),
                    TextAlignment = TextAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                //checking minvalue < maxvalue w.r.t scale precion value
                if (Math.Round(minvalue, ScaleValuePrecision) <= Math.Round(MaxValue, ScaleValuePrecision))
                {
                    minvalue = Math.Round(minvalue, ScaleValuePrecision);
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
                var onedegree = ((i + majorTickUnitAngle) - i) / (MinorDivisionsCount);

                if ((i < (ScaleStartAngle + ScaleSweepAngle)) && (Math.Round(minvalue, ScaleValuePrecision) <= Math.Round(MaxValue, ScaleValuePrecision)))
                {
                    //Drawing the minor scale
                    for (var mi = i + onedegree; mi < (i + majorTickUnitAngle); mi = mi + onedegree)
                    {
                        //here the minortick is drawn as a rectangle
                        Rectangle minorTick = new Rectangle
                        {
                            Height = MinorTickSize.Height,
                            Width = MinorTickSize.Width,
                            Fill = new SolidColorBrush(MinorTickColor),
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

        private void DrawSegment(Point p1, Point p2, Point p3, Point p4, bool reflexangle, Color clr)
        {
            // Segment Geometry
            PathSegmentCollection segments = new PathSegmentCollection();

            // First line segment from pt p1 - pt p2
            segments.Add(new LineSegment() { Point = p2 });

            //Arc drawn from pt p2 - pt p3 with the RangeIndicatorRadius
            segments.Add(new ArcSegment()
            {
                Size = new Size(arcradius2, arcradius2),
                Point = p3,
                SweepDirection = SweepDirection.Clockwise,
                IsLargeArc = reflexangle
            });

            // Second line segment from pt p3 - pt p4
            segments.Add(new LineSegment() { Point = p4 });

            //Arc drawn from pt p4 - pt p1 with the Radius of arcradius1
            segments.Add(new ArcSegment()
            {
                Size = new Size(arcradius1, arcradius1),
                Point = p1,
                SweepDirection = SweepDirection.Counterclockwise,
                IsLargeArc = reflexangle
            });

            // Defining the segment path properties
            Color rangestrokecolor;
            if (clr == Colors.Transparent)
            {
                rangestrokecolor = clr;
            }
            else
                rangestrokecolor = Colors.White;

            rangeIndicator = new Path()
            {
                StrokeLineJoin = PenLineJoin.Round,
                Stroke = new SolidColorBrush(rangestrokecolor),
                Fill = new SolidColorBrush(clr),
                Opacity = 0.65,
                StrokeThickness = 0.25,
                Data = new PathGeometry()
                {
                    Figures = new PathFigureCollection()
                     {
                        new PathFigure()
                        {
                            IsClosed = true,
                            StartPoint = p1,
                            Segments = segments
                        }
                    }
                }
            };

            //Set Z index of range indicator
            rangeIndicator.SetValue(Canvas.ZIndexProperty, 102);

            // Adding the segment to the root grid
            rootGrid.Children.Add(rangeIndicator);
        }

        /// <summary>
        /// Obtaining the Point (x,y) in the circumference
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        private Point GetCircumferencePoint(double angle, double radius)
        {
            var angle_radian = (angle * Math.PI) / 180;
            //Radius-- is the Radius of the gauge
            var X = (double)((Radius) + (radius) * Math.Cos(angle_radian));
            var Y = (double)((Radius) + (radius) * Math.Sin(angle_radian));
            Point p = new Point(X, Y);
            return p;
        }

        /// <summary>
        /// Move pointer without animating
        /// </summary>
        /// <param name="angleValue"></param>
        private void MovePointer(double angleValue)
        {
            if (pointer?.RenderTransform is TransformGroup tg)
            {
                RotateTransform rt = tg.Children[0] as RotateTransform;
                rt.Angle = angleValue;
            }
        }

        #endregion Methods
    }
}