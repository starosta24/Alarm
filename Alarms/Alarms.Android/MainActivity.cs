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
using Xamarin.Forms.Platform.Android;


namespace TimePickerDemo
{
    [Activity(Label = "Будильник" , MainLauncher=true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
       
        Android.Widget.Button timeSelectButton;
        LinearLayout mainLayout;
        int i = 0; //номер TextView 
        
        TextView nextText;
        
        void handler(object sender, EventArgs args)
        {                                 
            var textView = (TextView)sender;
 
                for (int j = 0; j < textViews.Length; j++)
                {
                    if (textViews[j] == textView)
                    {
                        nextText = textViews[j + 1];
                        if(nextText.Visibility == Android.Views.ViewStates.Visible) //если кнопка выключена
                        {
                            nextText.Visibility = Android.Views.ViewStates.Invisible;   //не видно "выкл"
                            textView.SetTextColor(Xamarin.Forms.Color.Gray.ToAndroid());    //цвет цифр на visible=true
                            //тут пишем обработик для самого будильника
                        }
                        else //если вкл
                        {
                            nextText.Visibility = Android.Views.ViewStates.Visible;
                            textView.SetTextColor(Xamarin.Forms.Color.LightGray.ToAndroid());
                        }
                        break;
                    }
                    else //пока не найдет нужный "вкл"
                    {
                        continue;
                    }
                }
                
        }
            
        
        


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Alarms.Droid.Resource.Layout.Main);

            timeSelectButton = FindViewById<Android.Widget.Button>(Alarms.Droid.Resource.Id.select_button);
            mainLayout = (LinearLayout)FindViewById(Alarms.Droid.Resource.Id.linearLayout);
            timeSelectButton.Click += TimeSelectOnClick;

        }
        public delegate void CallTextClick(object sender, EventArgs args);


        public TextView msg1;
        TextView[] textViews = new TextView[13];

        void TimeSelectOnClick(object sender, EventArgs eventArgs)
        {
            TimePickerFragment frag = TimePickerFragment.NewInstance(
                                      delegate (DateTime time)
                                      {
                                          //создаем для показа времени
                                          TextView msg = new TextView(this);
                                          msg.Text = time.ToShortTimeString();
                                          msg.TextSize = 55;
                                          msg.Clickable = true;
                                          msg.Enabled = true;
                                          //создаем для показа вкл/выкл
                                          msg1 = new TextView(this);
                                          msg1.Text = "Выключен";
                                          msg1.TextSize = 20;
                                          msg1.Visibility = Android.Views.ViewStates.Invisible; //не видно
                                          msg1.Clickable = true;
                                          //добавляем в массив
                                          textViews[i] = msg;
                                          i++;
                                          textViews[i] = msg1;
                                          i++;
                                          //добавляем на экран
                                          mainLayout.AddView(msg);
                                          mainLayout.AddView(msg1);
                                          
                                          msg.Click += handler;
                                         
                                          
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