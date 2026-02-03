using Microsoft.Xaml.Behaviors;
using System.Collections.Specialized;
using System.Windows.Controls;

namespace ToDoList.View.Behaviors
{
    internal class AutoScrollToBottomBehavior : Behavior<ListBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            ((INotifyCollectionChanged)AssociatedObject.Items).CollectionChanged += OnCollectionChanged;
        }

        protected override void OnDetaching()
        {
            ((INotifyCollectionChanged)AssociatedObject.Items).CollectionChanged -= OnCollectionChanged;
            base.OnDetaching();
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add && AssociatedObject.Items.Count > 0)
            {
                var lastItem = AssociatedObject.Items[AssociatedObject.Items.Count - 1];
                AssociatedObject.ScrollIntoView(lastItem);
            }
        }
    }
}
