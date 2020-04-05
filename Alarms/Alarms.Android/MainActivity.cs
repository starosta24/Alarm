using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Util;
using Android.Text.Format;
using Alarms.Droid;
using Xamarin.Forms;
using Android.Content;


//<TextView
//        android:id="@+id/time_display"
//		android:layout_height="568.0dp"
//		android:layout_width="match_parent"
//		android:paddingTop="22dp"
//		android:text=""
//		android:textSize="55dp"
//		android:layout_marginBottom="71.5dp" 
//	/>




namespace TimePickerDemo
{
    [Activity(Label = "Будильник" , MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        TextView timeDisplay;
        Android.Widget.Button timeSelectButton;
        LinearLayout mainLayout;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            var tDisp = "@time_display";
            var but = "@select_button";
            
           // timeDisplay = FindViewById<TextView>(Resource.Id.time_display);
            timeSelectButton = FindViewById<Android.Widget.Button>(Resource.Id.select_button);
            

            mainLayout = (LinearLayout)FindViewById(Resource.Id.linearLayout);
            timeSelectButton.Click += TimeSelectOnClick;       
            
        }

        

        void TimeSelectOnClick(object sender, EventArgs eventArgs)
        {
           
            TimePickerFragment frag = TimePickerFragment.NewInstance(
                                      delegate (DateTime time)
                                      {
                                         
                                          var msg = new TextView(this)
                                          {
                                              Text = time.ToShortTimeString(),
                                              Gravity = Android.Views.GravityFlags.Top,
                                              TextSize = 55
                                          };
                                          mainLayout.AddView(msg);
                                      });



            frag.Show(FragmentManager, TimePickerFragment.TAG);
        }
    }



    public class TimePickerFragment : DialogFragment, TimePickerDialog.IOnTimeSetListener
    {
        public static readonly string TAG = "MyTimePickerFragment";
        Action<DateTime> timeSelectedHandler = delegate { };

        public static TimePickerFragment NewInstance(Action<DateTime> onTimeSelected)
        {
            TimePickerFragment frag = new TimePickerFragment();
            frag.timeSelectedHandler = onTimeSelected;
            return frag;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            DateTime currentTime = DateTime.Now;
            bool is24HourFormat = DateFormat.Is24HourFormat(Activity);
            TimePickerDialog dialog = new TimePickerDialog
                (Activity, this, currentTime.Hour, currentTime.Minute, is24HourFormat);
            return dialog;
        }

        public void OnTimeSet(Android.Widget.TimePicker view, int hourOfDay, int minute)
        {
            DateTime currentTime = DateTime.Now;
            DateTime selectedTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, hourOfDay, minute, 0);
            Log.Debug(TAG, selectedTime.ToLongTimeString());
            timeSelectedHandler(selectedTime);
        }

        
    }
}