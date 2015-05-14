//
//  Copyright 2012, Xamarin Inc.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using MyPayLib;
using System.Threading;
using Android.Views.InputMethods;

namespace MyPay {
	public class PropertyViewAdaptor : BaseAdapter<MyDeduction>
	{
		#region implemented abstract members of BaseAdapter
		private List<MyDeduction> mItems;
		private Context mContext;

		public override long GetItemId (int position)
		{
			return position;
		}

		public override Android.Views.View GetView (int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			View row = convertView;
			if (row == null)
			{
				row = LayoutInflater.From (mContext).Inflate (Resource.Layout.PropertyListItem,null,false);
			}

			TextView txtDescription = row.FindViewById<TextView> (Resource.Id.txtDescription);
			txtDescription.Text = mItems [position].DeductionName;

			TextView editAmount = row.FindViewById<TextView> (Resource.Id.editAmount);
			editAmount.Text = mItems [position].DeductionAmount.ToString();

			txtDescription.Focusable = false;
			txtDescription.FocusableInTouchMode = false;
			txtDescription.Clickable = false;

			editAmount.TextChanged += (sender, e) => {
				if (editAmount.Text.IsNumeric())  mItems[position].DeductionAmount = Convert.ToDouble(editAmount.Text);
			};
			return row;
		}

		public override int Count {
			get {
				return mItems.Count;
			}
		}

		#endregion

		#region implemented abstract members of BaseAdapter

		public override MyDeduction this [int index] {
			get {
				return mItems [index];
			}
		}

		#endregion

		public PropertyViewAdaptor (Context ctx , List<MyDeduction> items)
		{
			mContext = ctx;
			mItems = items;
		}
	}
}

