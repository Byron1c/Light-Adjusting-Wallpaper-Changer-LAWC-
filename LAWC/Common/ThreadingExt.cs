using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Forms;


namespace LAWC.Common
{
    internal static class ThreadingExt
    {

        //https://stackoverflow.com/questions/661561/how-do-i-update-the-gui-from-another-thread
        // Update control via thread
        // Example:
        // myLabel.SetPropertyThreadSafe(() => myLabel.Text, status); // status has to be a string or this will fail to compile

        private delegate void SetPropertyThreadSafeDelegate<TResult>(
            Control @this,
            Expression<Func<TResult>> property,
            TResult value);


        public static void SetPropertyThreadSafe<TResult>(
            this Control @this,
            Expression<Func<TResult>> property,
            TResult value)
        {
            var propertyInfo = (property.Body as MemberExpression).Member
                as PropertyInfo;

            if (propertyInfo == null ||
                !@this.GetType().IsSubclassOf(propertyInfo.ReflectedType) ||
                @this.GetType().GetProperty(
                    propertyInfo.Name,
                    propertyInfo.PropertyType) == null)
            {
                throw new ArgumentException("The lambda expression 'property' must reference a valid property on this Control.");
            }

            if (@this.InvokeRequired)
            {
                @this.Invoke(new SetPropertyThreadSafeDelegate<TResult>
                (SetPropertyThreadSafe),
                new object[] { @this, property, value });
            }
            else
            {
                @this.GetType().InvokeMember(
                    propertyInfo.Name,
                    BindingFlags.SetProperty,
                    null,
                    @this,
                    new object[] { value }, CultureInfo.InvariantCulture);
            }
        }


    }
}
