using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Windows;

namespace KR_light_test
{
    class LightControllerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private const bool LightOff = false;
        private const bool LightOn = true;

        private Timer sensorUpdateTimer;
        private TimeSpan sensorUpdatePeriod = TimeSpan.FromMilliseconds(50);
        private TimeSpan lightOutDuration = TimeSpan.FromMilliseconds(500);
        private List<bool> lightSet = new List<bool>(new bool[8]);
        private int currentLightIndex;
        private int nextLightIndex;

        public TimeSpan SensorUpdatePeriod
        {
            get { return lightOutDuration; }
            set
            {
                lightOutDuration = value;
                NotifyChange(nameof(SensorUpdatePeriod));
            }
        }

        public Duration LightOutDuration
        {
            get { return new Duration(lightOutDuration); }
            set
            {
                lightOutDuration = value.TimeSpan;
                NotifyChange(nameof(LightOutDuration));
            }
        }

        public bool IsRunning => sensorUpdateTimer?.Enabled ?? false;

        #region Lights

        public bool Light1
        {
            get { return lightSet[0]; }
            set
            {
                lightSet[0] = value;
                NotifyChange(nameof(Light1));
            }
        }

        public bool Light2
        {
            get { return lightSet[1]; }
            set
            {
                lightSet[1] = value;
                NotifyChange(nameof(Light2));
            }
        }

        public bool Light3
        {
            get { return lightSet[2]; }
            set
            {
                lightSet[2] = value;
                NotifyChange(nameof(Light3));
            }
        }

        public bool Light4
        {
            get { return lightSet[3]; }
            set
            {
                lightSet[3] = value;
                NotifyChange(nameof(Light4));
            }
        }

        public bool Light5
        {
            get { return lightSet[4]; }
            set
            {
                lightSet[4] = value;
                NotifyChange(nameof(Light5));
            }
        }

        public bool Light6
        {
            get { return lightSet[5]; }
            set
            {
                lightSet[5] = value;
                NotifyChange(nameof(Light6));
            }
        }

        public bool Light7
        {
            get { return lightSet[6]; }
            set
            {
                lightSet[6] = value;
                NotifyChange(nameof(Light7));
            }
        }

        public bool Light8
        {
            get { return lightSet[7]; }
            set
            {
                lightSet[7] = value;
                NotifyChange(nameof(Light8));
            }
        }

        #endregion

        public LightControllerViewModel()
        {
            sensorUpdateTimer = new Timer(sensorUpdatePeriod.TotalMilliseconds)
            {
                AutoReset = true,
                Enabled = false,
            };

            sensorUpdateTimer.Elapsed += SensorUpdateTimer_Elapsed;

            Shutdown();
        }

        private void SensorUpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lightSet[currentLightIndex] = LightOff;
            lightSet[nextLightIndex] = LightOn;

            NotifyChange("Light" + (currentLightIndex + 1));
            NotifyChange("Light" + (nextLightIndex + 1));

            // -- update current and determine next --

            var nextIndexSave = nextLightIndex;

            if (currentLightIndex < nextLightIndex)
            {
                // moving right
                if (nextLightIndex == lightSet.Count - 1)
                {
                    // end of the line. Turn around
                    nextLightIndex--;
                }
                else
                {
                    nextLightIndex++;
                }
            }
            else if (currentLightIndex > nextLightIndex)
            {
                // moving left
                if (nextLightIndex == 0)
                {
                    // end of the line. Turn around
                    nextLightIndex++;
                }
                else
                {
                    nextLightIndex--;
                }
            }

            // move current to previous-next 
            currentLightIndex = nextIndexSave;
        }

        public void Start()
        {
            lightSet[currentLightIndex] = LightOn;
            sensorUpdateTimer.Start();
            NotifyChange(nameof(IsRunning));
        }

        public void Shutdown()
        {
            sensorUpdateTimer.Stop();
            NotifyChange(nameof(IsRunning));
            currentLightIndex = 4;
            nextLightIndex = 5;
            
            for (var i = 0; i < lightSet.Count; i++)
            {
                lightSet[i] = LightOff;
                NotifyChange("Light" + (i + 1));
            }
        }

        private void NotifyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
