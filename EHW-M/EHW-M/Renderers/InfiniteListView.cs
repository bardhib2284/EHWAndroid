﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace EHWM.Renderers {
    public class InfiniteListView : ListView {
        public static readonly BindableProperty LoadMoreCommandProperty = BindableProperty.Create<InfiniteListView, ICommand>(bp => bp.LoadMoreCommand, default(ICommand));

        public ICommand LoadMoreCommand {
            get { return (ICommand)GetValue(LoadMoreCommandProperty); }
            set { SetValue(LoadMoreCommandProperty, value); }
        }

        public InfiniteListView() {
            ItemAppearing += InfiniteListView_ItemAppearing;
        }

        void InfiniteListView_ItemAppearing(object sender, ItemVisibilityEventArgs e) {
            var items = ItemsSource as IList;
            if(items.Count >= 14) {
                if (items != null && e.Item == items[items.Count - 1]) {
                    if (LoadMoreCommand != null && LoadMoreCommand.CanExecute(null))
                        LoadMoreCommand.Execute(null);
                }
            }

        }
    }
}