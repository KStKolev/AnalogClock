using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Text;

namespace AnalogClock
{

    public partial class Clock : Form
    {
        // create a timer
        Timer timer = new Timer();
        // height and width of the clock
        private int width = 400;
        private int height = 400;
        // hand values
        private int secondHand = 140;
        private int minuteHand = 110;
        private int hourHand = 80;
        // center point
        private PointF center;
        private Bitmap bulgarianImage;
        private Bitmap italianImage;
        private Graphics graphic;
        // Clock size
        private float size;
        private float radius;

        public Clock()
        {
            InitializeComponent();
            // set value to the clock size variables.
            size = height * 0.99f / 2;
            radius = width / 2;
            // set value to create a center for the clock.
            center = new PointF(width / 2, height / 2);
        }

        // Method for loading the clock.
        private void LoadClock(object sender, EventArgs e)
        {
            // create a new bitmap  
            bulgarianImage = new Bitmap(width + 1, height + 1);
            italianImage = new Bitmap(width + 1, height + 1);
            //backcolor  
            this.BackColor = Color.LightCyan;
            //timer  
            timer.Interval = 1000; // Tick in a second  
            timer.Tick += new EventHandler(this.TimerTick);
            timer.Start();
        }

        // Method timer from current time.
        private void TimerTick(object sender, EventArgs e)
        {
            //get time  
            int seconds = DateTime.Now.Second;
            int minutes = DateTime.Now.Minute;
            int hours = DateTime.Now.Hour;
            //load the bitmap image  
            bulgariaClockBox.Image = 
                CreateClockImage(hours, minutes, seconds, bulgarianImage);
            italyClockBox.Image =
                CreateClockImage(hours - 1, minutes, seconds, italianImage);
            // garbage collector
            graphic.Dispose();
        }

        private Bitmap CreateClockImage(int hours, int minutes, 
            int seconds, Bitmap chosenImage)
        {
            // create an image  
            graphic = Graphics.FromImage(chosenImage);
            graphic.Clear(Color.LightCyan);
            if (hours == DateTime.Now.Hour)
            {
                Color[] surroundFillEllipse = new Color[] { Color.Wheat };
                GradientBrushFilling(hours, minutes, seconds, 
                    Color.AntiqueWhite, surroundFillEllipse, Color.RosyBrown,
                    Color.DarkGoldenrod, Color.SandyBrown);
            }
            else
            {
                Color[] surroundFillEllipse = new Color[] { Color.OldLace };
                GradientBrushFilling(hours, minutes, seconds, 
                    Color.AntiqueWhite, surroundFillEllipse, Color.RosyBrown,
                    Color.SandyBrown, Color.RosyBrown);
            }
            // Use manually created, void method - DigitalClock.
            DigitalClock(seconds, minutes, hours + 1);
            // Print the text for Time zone.
            PrintTimeZoneText();
            return chosenImage;
        }

        private void GradientBrushFilling(int hours, int minutes, int seconds, 
            Color centerFillEllipse, Color[] surroundFillEllipse,
            Color ellipseSurrondLines, Color ellipseSurroundrectangles, 
            Color ellipseColor)
        {
            //Fill the clock with gradient brush
            PathGradientBrush pathGradientBrush = 
                GradientBrushFill(centerFillEllipse, surroundFillEllipse);
            graphic.FillEllipse(pathGradientBrush, 0, 0, width, height);
            // Use manually created, void method - PropertyLines()
            PropertyLines(ellipseSurrondLines, ellipseSurroundrectangles);
            int[] handCoordinationArray = new int[2];
            // set value to the array variable with DrawAnalogClock()
            handCoordinationArray = 
                DrawAnalogClock(seconds, minutes, hours, ellipseColor);
        }

        private int[] DrawAnalogClock(int seconds, int minutes, 
            int hours, Color ellipseColor)
        {
            if (hours == DateTime.Now.Hour)
            {
                // Bulgarian colors.
                DrawRomanNumerals(ellipseColor, Brushes.White, 
                    Brushes.Green, Brushes.Red);
            }
            else
            {
                // Italian colors.
                DrawRomanNumerals(ellipseColor, Brushes.Green, 
                    Brushes.Black, Brushes.Red);
            }
            // draw the center dot
            graphic.DrawString(".", new Font("Georgia", 45), 
                Brushes.Firebrick, new PointF(180, 135));
            int[] handCoordinationArray = new int[] { };
            //draw seconds hand  
            handCoordinationArray = 
                MinuteAndSecondCoordination(seconds, secondHand);
            graphic.DrawLine(new Pen(Color.Red, 4f), 
                new Point((int)center.X, (int)center.Y),
                new Point(handCoordinationArray[0], handCoordinationArray[1]));
            //draw minutes hand  
            handCoordinationArray = 
                MinuteAndSecondCoordination(minutes, minuteHand);
            graphic.DrawLine(new Pen(Color.Green, 5f), 
                new Point((int)center.X, (int)center.Y),
                new Point(handCoordinationArray[0], handCoordinationArray[1]));
            //draw hours hand  
            handCoordinationArray = 
                HourCoordination(hours % 12, minutes, hourHand);
            graphic.DrawLine(new Pen(Color.White, 5f), 
                new Point((int)center.X, (int)center.Y),
                new Point(handCoordinationArray[0], handCoordinationArray[1]));
            return handCoordinationArray;
        }

        private void PrintTimeZoneText()
        {
            firstCountryName.Text = "Bulgarian";
            firstCountryTime.Text = "time";
            firstCountryZone.Text = "zone";
            secondCountryName.Text = "Italian";
            secondCountryTime.Text = "time";
            secondCountryZone.Text = "zone";
        }

        private void DrawRomanNumerals(Color EllipseColor, Brush firstFlagColor, 
            Brush secondFlagColor, Brush thirdFlagColor)
        {
            //draw a circle  
            graphic.DrawEllipse(new Pen(EllipseColor, 6f), 0, 0, width, height);
            //draw clock roman numerals  
            graphic.DrawString("I", new Font("Georgia", 13), secondFlagColor, new PointF(280, 35));
            graphic.DrawString("II", new Font("Georgia", 13), thirdFlagColor, new PointF(339, 100));
            graphic.DrawString("III", new Font("Georgia", 13), firstFlagColor, new PointF(356, 188));
            graphic.DrawString("IV", new Font("Georgia", 13), secondFlagColor, new PointF(335, 275));
            graphic.DrawString("V", new Font("Georgia", 13), thirdFlagColor, new PointF(282, 340));
            graphic.DrawString("VI", new Font("Georgia", 13), firstFlagColor, new PointF(185, 365));
            graphic.DrawString("VII", new Font("Georgia", 13), secondFlagColor, new PointF(94, 340));
            graphic.DrawString("VIII", new Font("Georgia", 13), thirdFlagColor, new PointF(33, 275));
            graphic.DrawString("IX", new Font("Georgia", 13), firstFlagColor, new PointF(11, 188));
            graphic.DrawString("X", new Font("Georgia", 13), secondFlagColor, new PointF(38, 100));
            graphic.DrawString("XI", new Font("Georgia", 13), thirdFlagColor, new PointF(96, 38));
            graphic.DrawString("XII", new Font("Georgia", 13), firstFlagColor, new PointF(180, 10));
        }

        private PathGradientBrush GradientBrushFill(Color centerColor, 
            Color[] surroundColor)
        {
            GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.AddEllipse(0, 0, width, height);
            PathGradientBrush pathGradientBrush = 
                new PathGradientBrush(graphicsPath);
            // set the center gradient Brush color. 
            pathGradientBrush.CenterColor = centerColor;
            // set the surround gradient Brush color. 
            pathGradientBrush.SurroundColors = surroundColor;
            return pathGradientBrush;
        }

        //method for drawing lines, representing seconds/minutes.
        private void PropertyLines(Color color, Color secondColor)
        {
            for (int index = 0; index < 60; index++)
            {
                // create lines between the hour roman numerals
                // each line indicates seconds/minutes.
                if (index % 5 != 0)
                {
                    graphic.DrawLine(new Pen(color, radius * 0.022f),
                         (float)Math.Sin(index * 6 * Math.PI / 180) * radius * 0.95f + center.Y,
                         (float)Math.Cos(index * 6 * Math.PI / 180) * radius * 0.95f + center.X,
                         (float)Math.Sin(index * 6 * Math.PI / 180) * radius + center.Y,
                         (float)Math.Cos(index * 6 * Math.PI / 180) * radius + center.X);
                }
                // create space into the roman numerals' position.
                else if (index % 5 == 0)
                {
                    graphic.DrawLine(new Pen(secondColor, size * 0.04f),
                        (float)Math.Sin(index * 6 * Math.PI / 180) * size * 0.95f + center.Y,
                        (float)Math.Cos(index * 6 * Math.PI / 180) * size * 0.95f + center.X,
                        (float)Math.Sin(index * 6 * Math.PI / 180) * size + center.Y,
                        (float)Math.Cos(index * 6 * Math.PI / 180) * size + center.X);
                }
            }
        }

        private int[] MinuteAndSecondCoordination(int value, int minuteOrSecondLength)
        {
            int[] coordinationCenterArray = new int[2];
            value *= 6; // note: each minute and seconds make a 6 degree  
            if (value >= 0 && value <= 100)
            {
                coordinationCenterArray[0] = 
                    (int)center.X + (int)(minuteOrSecondLength * Math.Sin(Math.PI * value / 180));
                coordinationCenterArray[1] = 
                    (int)center.Y - (int)(minuteOrSecondLength * Math.Cos(Math.PI * value / 180));
            }
            else
            {
                coordinationCenterArray[0] = 
                    (int)center.X - (int)(minuteOrSecondLength * -Math.Sin(Math.PI * value / 180));
                coordinationCenterArray[1] = 
                    (int)center.Y - (int)(minuteOrSecondLength * Math.Cos(Math.PI * value / 180));
            }
            return coordinationCenterArray;
        }

        private int[] HourCoordination(int hourvalue, int minuteValue, int hourLength)
        {
            int[] coordinationCenterArray = new int[2];
            //each hour makes 30 degree with min making 0.5 degree  
            int value = (int)((hourvalue * 30) + (minuteValue * 0.5));
            if (value >= 0 && value <= 180)
            {
                coordinationCenterArray[0] = 
                    (int)center.X + (int)(hourLength * Math.Sin(Math.PI * value / 180));
                coordinationCenterArray[1] = 
                    (int)center.Y - (int)(hourLength * Math.Cos(Math.PI * value / 180));
            }
            else
            {
                coordinationCenterArray[0] = 
                    (int)center.X - (int)(hourLength * -Math.Sin(Math.PI * value / 180));
                coordinationCenterArray[1] = 
                    (int)center.Y - (int)(hourLength * Math.Cos(Math.PI * value / 180));
            }
            return coordinationCenterArray;
        }

        private void DigitalClock(int seconds, int minutes, int hours)
        {
            // create objects from StringBuilder class.
            StringBuilder digitalTimeNowBG = new StringBuilder();
            StringBuilder digitalTimeNowItaly = new StringBuilder();
            // condition for appending hours in the variables
            // it represents hours for the digital clocks
            if (hours < 10)
            {
                digitalTimeNowBG.Append("0" + hours);
                // italy zero hour Condition
                if (hours == 0)
                {
                    digitalTimeNowItaly.Append(23);
                }
                else
                {
                    digitalTimeNowItaly.Append("0" + (hours - 1));
                }
            }
            else
            {
                digitalTimeNowBG.Append(hours);
                digitalTimeNowItaly.Append(hours - 1);
            }
            digitalTimeNowBG.Append(":");
            digitalTimeNowItaly.Append(":");
            // condition for appending minutes in the variables
            // it represents minutes for the digital clocks
            if (minutes < 10)
            {
                digitalTimeNowBG.Append("0" + minutes);
                digitalTimeNowItaly.Append("0" + minutes);
            }
            else
            {
                digitalTimeNowBG.Append(minutes);
                digitalTimeNowItaly.Append(minutes);
            }
            // set the time for the digitalClockBG lable
            digitalClockBG.Text = digitalTimeNowBG.ToString();
            // set the time for the digitalTimeNowItaly lable
            digitalClockItaly.Text = digitalTimeNowItaly.ToString();
            // method DigitalClockSeconds sets the value in digitalSeconds
            StringBuilder digitalSeconds = DigitalClockSeconds(seconds);
            // set seconds for both countries.
            secondsDigClockBG.Text = digitalSeconds.ToString();
            secondsDigClockItaly.Text = digitalSeconds.ToString();
        }

        private StringBuilder DigitalClockSeconds(int seconds)
        {
            // create object from class StringBuilder
            // it represents seconds for the digital clocks
            StringBuilder digitalSeconds = new StringBuilder();
            // condition for appending seconds in the variable
            // it represents seconds for the digital clocks
            if (seconds < 10)
            {
                digitalSeconds.Append("0" + seconds);
            }
            else
            {
                digitalSeconds.Append(seconds);
            }
            return digitalSeconds;
        }
    }
}
