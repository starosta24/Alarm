using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Util;
using Android.Text.Format;
using Alarms.Droid;
using Xamarin.Forms;
using Android.Content;
using System.Collections.Generic;
using Android.Views;


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
    [Activity(Label = "Будильник" , MainLauncher=true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
       
        Android.Widget.Button timeSelectButton;
        LinearLayout mainLayout;
        int i = 0; //номер TextView с временем
        bool butClick = false;
        
        
        void handler(object sender, EventArgs args)
        {
            
            var textView = (TextView)sender;
            
            if (textView.Text == "Включен")
            {
                msg1.Visibility = Android.Views.ViewStates.Visible;
                //textView.Text = "Выключен";
                textView.Enabled = false;
                butClick = false;
            }
            else
            {
                textView.Enabled = true;
                // textView.Text = "Включен";
                butClick = true;

            }
            
            

        }



        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            timeSelectButton = FindViewById<Android.Widget.Button>(Resource.Id.select_button);
            mainLayout = (LinearLayout)FindViewById(Resource.Id.linearLayout);
            timeSelectButton.Click += TimeSelectOnClick;

        }
        public delegate void CallTextClick(object sender, EventArgs args);

        public TextView msg1;
        void TimeSelectOnClick(object sender, EventArgs eventArgs)
        {
            
            TimePickerFragment frag = TimePickerFragment.NewInstance(
                                      delegate (DateTime time)
                                      {
                                          i++;
                                          TextView msg = new TextView(this);
                                          msg.Text = time.ToShortTimeString();
                                          msg.TextSize = 55;
                                          msg.Clickable = true;
                                          msg.Enabled = true;
                                          msg.Click += handler;
                                          
                                          mainLayout.AddView(msg);

                                          msg1 = new TextView(this);
                                          msg1.Text = "Выключен";
                                          msg1.TextSize = 20;
                                          msg1.Visibility = Android.Views.ViewStates.Invisible;
                                          //msg1.Click += handler;
                                          mainLayout.AddView(msg1);



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