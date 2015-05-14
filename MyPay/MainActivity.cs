using System;
using System.Collections.Generic;

using System.Globalization;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;

//using Android.Support.V4.App;
//using Android.Support.V4.View;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using MyPayLib;

namespace MyPay
{
	[Activity (Label = "MyPay", MainLauncher = true, Icon = "@drawable/ic_action_pie_chart")]
	public class MainActivity : FragmentActivity
	{
		private ViewPager mViewPager;
		private SlidingTabScrollView mScrollView;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);
			mScrollView = FindViewById<SlidingTabScrollView>(Resource.Id.sliding_tabs);
			mViewPager = FindViewById<ViewPager>(Resource.Id.viewPager);

			mViewPager.Adapter = new SamplePagerAdapter(SupportFragmentManager);
			mScrollView.ViewPager = mViewPager;
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.actionbar_main, menu);
			return base.OnCreateOptionsMenu(menu);
		}       
	}

	public class SamplePagerAdapter : FragmentPagerAdapter
	{
		private readonly List<Android.Support.V4.App.Fragment> mFragmentHolder;

		public SamplePagerAdapter (Android.Support.V4.App.FragmentManager fragManager) : base(fragManager)
		{
			mFragmentHolder = new List<Android.Support.V4.App.Fragment>();
			Fragment1 frag1 = new Fragment1 ();
			Fragment2 frag2 = new Fragment2 ();
			Fragment3 frag3 = new Fragment3 ();
			Fragment4 frag4 = new Fragment4 ();

			frag1.PayCheck = frag2.PayCheck = frag3.PayCheck  = frag4.PayCheck  = PayCheck;

			mFragmentHolder.Add(frag1);
			mFragmentHolder.Add(frag2);
			mFragmentHolder.Add(frag3);
			mFragmentHolder.Add(frag4);

		}

		public override int Count
		{
			get { return mFragmentHolder.Count; }
		}

		public override Android.Support.V4.App.Fragment GetItem(int position)
		{
			return mFragmentHolder[position];
		}

		private PayCheck _paycheck;
		public PayCheck PayCheck 
		{
			get {
				if (_paycheck == null)
					_paycheck = new PayCheck ();
				return _paycheck;
			}
			set { _paycheck = value; }
		}
	}

	public class Fragment1 : Android.Support.V4.App.Fragment
	{
		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var view = inflater.Inflate(Resource.Layout.Frag1Layout, container, false);

			CheckBox cbIsMetro = view.FindViewById<CheckBox> (Resource.Id.cbIsMetro);
			PayCheck.IsMetroResident = cbIsMetro.Checked;

			EditText editCtc = view.FindViewById<EditText> (Resource.Id.editCtc);
			if (editCtc.Text.IsNumeric()) PayCheck.Income.CTC = Convert.ToDouble (editCtc.Text);

			EditText editShiftAllowance = view.FindViewById<EditText> (Resource.Id.editShiftAllowance);
			if (editShiftAllowance.Text.IsNumeric()) PayCheck.Income.ShiftAllowance = Convert.ToDouble (editShiftAllowance.Text);

			EditText editBasic = view.FindViewById<EditText> (Resource.Id.editBasic);
			editBasic.Text = PayCheck.Income.Basic.ToString("C" , new CultureInfo("hi-IN"));
			editBasic.Focusable = editBasic.Enabled = false;

			cbIsMetro.CheckedChange += (object sender, CompoundButton.CheckedChangeEventArgs e) => {
				PayCheck.IsMetroResident = e.IsChecked;
				editBasic.Text = PayCheck.Income.Basic.ToString("C" , new CultureInfo("hi-IN"));
			};

			editCtc.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
				if (editCtc.Text.IsNumeric()) PayCheck.Income.CTC = Convert.ToDouble (editCtc.Text);
				editBasic.Text = PayCheck.Income.Basic.ToString("C" , new CultureInfo("hi-IN"));
			};

			editShiftAllowance.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
				if (editShiftAllowance.Text.IsNumeric()) PayCheck.Income.ShiftAllowance = Convert.ToDouble (editShiftAllowance.Text);
			};

			return view;
		}

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			this.RetainInstance = true;
		}

		public void showSoftKeyboard(View view) {
			if (view.RequestFocus()) {
				var imm = (InputMethodManager) this.Activity.GetSystemService (Context.InputMethodService);
				imm.HideSoftInputFromWindow(view.WindowToken, HideSoftInputFlags.ImplicitOnly);
			}
		}

		public override string ToString() //Called on line 156 in SlidingTabScrollView
		{
			return "Income";
		}

		public PayCheck PayCheck {get;set;}
	}

	public class Fragment2 : Android.Support.V4.App.Fragment
	{
		private ListView lstView;

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var view = inflater.Inflate(Resource.Layout.Frag2Layout, container, false);

			lstView = view.FindViewById<ListView> (Resource.Id.listSharedBy);
			//ArrayAdapter<string> adapter = new ArrayAdapter<string> (this, Android.Resource.Layout.SimpleListItem1 , items);

			PropertyViewAdaptor adapter = new PropertyViewAdaptor (container.Context, PayCheck.Deductions.Where(d=> d.DeductionType != DeductionType.Investment).ToList());

			lstView.Adapter = adapter;

			return view;
		}

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			this.RetainInstance = true;
		}

		public override string ToString() //Called on line 156 in SlidingTabScrollView
		{
			return "Deductions";
		}

		public PayCheck PayCheck {get;set;}


	}

	public class Fragment3: Android.Support.V4.App.Fragment
	{
		private ListView lstView;

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var view = inflater.Inflate(Resource.Layout.Frag3Layout, container, false);

			lstView = view.FindViewById<ListView> (Resource.Id.listMyInfo);
			//ArrayAdapter<string> adapter = new ArrayAdapter<string> (this, Android.Resource.Layout.SimpleListItem1 , items);

			PropertyViewAdaptor adapter = new PropertyViewAdaptor (container.Context, PayCheck.Deductions.Where(d=> d.DeductionType == DeductionType.Investment).ToList());

			lstView.Adapter = adapter;
			return view;
		}

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			this.RetainInstance = true;
		}

		public override string ToString() //Called on line 156 in SlidingTabScrollView
		{
			return "Tax Savers";
		}

		public PayCheck PayCheck {get;set;}

	}

	public class Fragment4: Android.Support.V4.App.Fragment
	{
		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var view = inflater.Inflate(Resource.Layout.Frag4Layout, container, false);
			return view;
		}

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			this.RetainInstance = true;
		}

		public override void OnResume ()
		{
			CalculatePayCheck ();
			base.OnResume ();
		}

		public void CalculatePayCheck()
		{
			View view = this.View;

			InputMethodManager imm = (InputMethodManager) Activity.GetSystemService (Context.InputMethodService);
			imm.HideSoftInputFromWindow(this.View.WindowToken,  HideSoftInputFlags.None);

			PayCheck.Calculate();

			EditText viewCtc = view.FindViewById<EditText> (Resource.Id.viewCtc);
			viewCtc.Text = PayCheck.Income.CTC.ToString("C" , new CultureInfo("hi-IN"));
			viewCtc.Focusable = viewCtc.Enabled = false;

			EditText viewVariable = view.FindViewById<EditText> (Resource.Id.viewVariable);
			viewVariable.Text = PayCheck.Income.VariableAmount.ToString("C" , new CultureInfo("hi-IN"));
			viewVariable.Focusable = viewVariable.Enabled = false;

			EditText viewPf = view.FindViewById<EditText> (Resource.Id.viewPf);
			viewPf.Text = PayCheck.Income.PfAmount.ToString("C" , new CultureInfo("hi-IN"));
			viewPf.Focusable = viewPf.Enabled = false;

			EditText viewTaxableIncome = view.FindViewById<EditText> (Resource.Id.viewTaxableIncome);
			viewTaxableIncome.Text = PayCheck.TaxableIncomeValue.ToString("C" , new CultureInfo("hi-IN"));
			viewTaxableIncome.Focusable = viewTaxableIncome.Enabled = false;

			EditText viewTotalTax = view.FindViewById<EditText> (Resource.Id.viewTotalTax);
			viewTotalTax.Text = PayCheck.TaxAmountValue.ToString("C" , new CultureInfo("hi-IN"));
			viewTotalTax.Focusable = viewTotalTax.Enabled = false;

			EditText viewPaycheck = view.FindViewById<EditText> (Resource.Id.viewPaycheck);
			viewPaycheck.Text =  PayCheck.TakeHomeValue.ToString("C" , new CultureInfo("hi-IN"));
			viewPaycheck.Focusable = viewPaycheck.Enabled = false;
		}

		public override string ToString() //Called on line 156 in SlidingTabScrollView
		{
			return "Paycheck";
		}

		public PayCheck PayCheck {get;set;}

	}
}


